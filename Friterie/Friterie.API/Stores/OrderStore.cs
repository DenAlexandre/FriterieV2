namespace Friterie.API.Stores
{
    using Friterie.Shared.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Npgsql;
    using NpgsqlTypes;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;



    public class OrderStore(IConfiguration configuration, ILogger<IOrderStore> logger) : IOrderStore
    {
        private const string FN_GET_COUNT_ALIMENTS_BDD = "select * from friterie.fn_get_count_aliments";
        private const string FN_GET_ALIMENTS_BDD = "select * from friterie.fn_get_aliments";
        private const string FN_GET_GROUPE_ALIMENTS_BDD = "select * from friterie.fn_get_groupes_aliments";

        private const string FN_GET_PRODUCTS_BDD = "select * from friterie.fn_get_products";

        #region variables

        private readonly ILogger<IOrderStore> _logger = logger;

        private readonly string _connectionString = configuration.GetConnectionString("SDRDb") ?? throw new InvalidOperationException("Missing [SDRDb] connection string.");


        #endregion



        #region Order

        // =======================
        // GET BY ID
        // =======================
        public async Task<Orders?> GetByIdOrderAsync(int order_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM friterie.fn_get_orders_by_id(@p_order_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_order_id", NpgsqlDbType.Integer, order_id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Orders
            {
                OrderId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                OrderUserId = reader.IsDBNull(1) ? default : reader.GetInt32(1),
                OrderDatetime = reader.IsDBNull(2) ? default : reader.GetDateTime(2),
                OrderTotal = reader.IsDBNull(3) ? default : reader.GetDecimal(3),
                OrderStatus = reader.IsDBNull(4) ? default : reader.GetInt32(4),
                OrderIntentId = reader.IsDBNull(5) ? default : reader.GetString(5),
                OrderIsPaid = reader.IsDBNull(6) ? default : reader.GetBoolean(6)
            };
        }

        // =======================
        // GET ALL (pagination)
        // =======================
        public async Task<List<Orders>> GetAllOrdersAsync(int userId, int limit, int offset)
        {
            var result = new List<Orders>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM friterie.fn_get_orders(@p_limit, @p_offset)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, offset);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new Orders
                {
                    OrderId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                    OrderUserId = reader.IsDBNull(1) ? default : reader.GetInt32(1),
                    OrderDatetime = reader.IsDBNull(2) ? default : reader.GetDateTime(2),
                    OrderTotal = reader.IsDBNull(3) ? default : reader.GetDecimal(3),
                    OrderStatus = reader.IsDBNull(4) ? default : reader.GetInt32(4),
                    OrderIntentId = reader.IsDBNull(5) ? default : reader.GetString(5),
                    OrderIsPaid = reader.IsDBNull(6) ? default : reader.GetBoolean(6)
                });
            }

            return result;
        }

        // =======================
        // INSERT
        // =======================
        public async Task InsertOrderAsync(Orders entity)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_insert_orders(@p_order_user_id, @p_order_datetime, @p_order_total, @p_order_status, @p_order_intent_id, @p_order_is_paid)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_order_user_id", (object?)entity.OrderUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_datetime", NpgsqlDbType.Timestamp, DateTime.SpecifyKind(entity.OrderDatetime, DateTimeKind.Unspecified));
            cmd.Parameters.AddWithValue("p_order_total", (object?)entity.OrderTotal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_status", (object?)entity.OrderStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_intent_id", (object?)entity.OrderIntentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_is_paid", (object?)entity.OrderIsPaid ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // UPDATE
        // =======================
        public async Task<bool> UpdateOrderAsync(Orders entity)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_update_orders(@p_order_id, @p_order_user_id, @p_order_datetime, @p_order_total, @p_order_status, @p_order_intent_id, @p_order_is_paid)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_order_id", (object?)entity.OrderId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_user_id", (object?)entity.OrderUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_datetime", NpgsqlDbType.Timestamp, DateTime.SpecifyKind(entity.OrderDatetime, DateTimeKind.Unspecified));
            cmd.Parameters.AddWithValue("p_order_total", (object?)entity.OrderTotal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_status", (object?)entity.OrderStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_intent_id", (object?)entity.OrderIntentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_order_is_paid", (object?)entity.OrderIsPaid ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();

            return true;
        }

        // =======================
        // DELETE
        // =======================
        public async Task DeleteOrderAsync(int order_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_delete_orders(@p_order_id);";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_order_id", order_id);

            await cmd.ExecuteNonQueryAsync();
        }




        #endregion


        #region Order Item


        // =======================
        // GET BY ID
        // =======================
        public async Task<OrderItem?> GetOrderItemByIdAsync(int oi_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM friterie.fn_get_order_item_by_id(@p_oi_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_oi_id", NpgsqlDbType.Integer, oi_id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new OrderItem
            {
                OiId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                OiProductId = reader.IsDBNull(1) ? default : reader.GetInt32(1),
                OiProductName = reader.IsDBNull(2) ? default : reader.GetString(2),
                OiQuantity = reader.IsDBNull(3) ? default : reader.GetInt32(3),
                OiPrice = reader.IsDBNull(4) ? default : reader.GetDecimal(4),
                OiOrderId = reader.IsDBNull(5) ? default : reader.GetInt32(5)
            };
        }

        // =======================
        // GET ALL (pagination)
        // =======================
        //public async Task<List<OrderItem>> GetAllOrderItemAsync(int limit, int offset)
        //{
        //    var result = new List<OrderItem>();

        //    await using var conn = new NpgsqlConnection(_connectionString);
        //    await conn.OpenAsync();

        //    var sql = "SELECT * FROM friterie.fn_get_order_item(@p_limit, @p_offset)";

        //    await using var cmd = new NpgsqlCommand(sql, conn);
        //    cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, limit);
        //    cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, offset);

        //    await using var reader = await cmd.ExecuteReaderAsync();
        //    while (await reader.ReadAsync())
        //    {
        //        result.Add(new OrderItem
        //        {
        //            OiId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
        //            OiProductId = reader.IsDBNull(1) ? default : reader.GetInt32(1),
        //            OiProductName = reader.IsDBNull(2) ? default : reader.GetString(2),
        //            OiQuantity = reader.IsDBNull(3) ? default : reader.GetInt32(3),
        //            OiPrice = reader.IsDBNull(4) ? default : reader.GetDecimal(4),
        //            OiOrderId = reader.IsDBNull(5) ? default : reader.GetInt32(5)
        //        });
        //    }

        //    return result;
        //}

        // =======================
        // INSERT
        // =======================
        public async Task InsertOrderItemAsync(OrderItem entity)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_insert_order_item(@p_oi_product_id, @p_oi_product_name, @p_oi_quantity, @p_oi_price, @p_oi_order_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_oi_product_id", (object?)entity.OiProductId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_product_name", (object?)entity.OiProductName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_quantity", (object?)entity.OiQuantity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_price", (object?)entity.OiPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_order_id", (object?)entity.OiOrderId ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // UPDATE
        // =======================
        public async Task UpdateOrderItemAsync(OrderItem entity)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_update_order_item(@p_oi_id, @p_oi_product_id, @p_oi_product_name, @p_oi_quantity, @p_oi_price, @p_oi_order_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_oi_id", (object?)entity.OiId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_product_id", (object?)entity.OiProductId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_product_name", (object?)entity.OiProductName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_quantity", (object?)entity.OiQuantity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_price", (object?)entity.OiPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_oi_order_id", (object?)entity.OiOrderId ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // DELETE
        // =======================
        public async Task DeleteOrderItemAsync(int oi_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_delete_order_item(@p_oi_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_oi_id", oi_id);

            await cmd.ExecuteNonQueryAsync();
        }


        #endregion


    }
}


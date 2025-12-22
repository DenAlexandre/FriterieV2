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



    public class UserStore(IConfiguration configuration, ILogger<IUserStore> logger) : IUserStore
    {


        #region variables

        private readonly ILogger<IUserStore> _logger = logger;

        private readonly string _connectionString = configuration.GetConnectionString("SDRDb") ?? throw new InvalidOperationException("Missing [SDRDb] connection string.");


        #endregion



        // =======================
        // GET BY ID
        // =======================
        public async Task<User?> GetByIdAsync(int user_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM friterie.fn_get_users_by_id(@p_user_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_user_id", NpgsqlDbType.Integer, user_id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new User
            {
                UserId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                Email = reader.IsDBNull(1) ? default : reader.GetString(1),
                Password = reader.IsDBNull(2) ? default : reader.GetString(2),
                FirstName = reader.IsDBNull(3) ? default : reader.GetString(3),
                LastName = reader.IsDBNull(4) ? default : reader.GetString(4),
                PhoneNumber = reader.IsDBNull(5) ? default : reader.GetString(5),
                Address = reader.IsDBNull(6) ? default : reader.GetString(6),
                Created = reader.IsDBNull(7) ? default : reader.GetDateTime(7)
            };
        }

        // =======================
        // GET ALL (pagination)
        // =======================
        public async Task<List<User>> GetAllUsersAsync(int limit, int offset)
        {
            var result = new List<User>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM friterie.fn_get_users(@p_limit, @p_offset)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, offset);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new User
                {
                    UserId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
                    Email = reader.IsDBNull(1) ? default : reader.GetString(1),
                    Password = reader.IsDBNull(2) ? default : reader.GetString(2),
                    FirstName = reader.IsDBNull(3) ? default : reader.GetString(3),
                    LastName = reader.IsDBNull(4) ? default : reader.GetString(4),
                    PhoneNumber = reader.IsDBNull(5) ? default : reader.GetString(5),
                    Address = reader.IsDBNull(6) ? default : reader.GetString(6),
                    Created = reader.IsDBNull(7) ? default : reader.GetDateTime(7)
                });
            }

            return result;
        }

        // =======================
        // INSERT
        // =======================
        public async Task InsertUserAsync(User entity)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_insert_users(@p_email, @p_password, @p_first_name, @p_last_name, @p_phone_number, @p_address, @p_created);";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_password", (object?)entity.Password ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_first_name", (object?)entity.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_last_name", (object?)entity.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_phone_number", (object?)entity.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address", (object?)entity.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_created", NpgsqlDbType.Timestamp, DateTime.SpecifyKind(entity.Created, DateTimeKind.Unspecified));
            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // UPDATE
        // =======================
        public async Task UpdateUserAsync(User entity)
        {
            if (entity.UserId == null)
            {
                throw new ArgumentException("User ID cannot be null for update operation.");
            }


            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_update_users(@p_user_id, @p_email, @p_password, @p_first_name, @p_last_name, @p_phone_number, @p_address, @p_created)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_user_id", (object?)entity.UserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_password", (object?)entity.Password ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_first_name", (object?)entity.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_last_name", (object?)entity.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_phone_number", (object?)entity.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address", (object?)entity.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_created", NpgsqlDbType.Timestamp, DateTime.SpecifyKind(entity.Created, DateTimeKind.Unspecified));

            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // DELETE
        // =======================
        public async Task DeleteUserAsync(int user_id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "CALL friterie.sp_delete_users(@p_user_id);";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue($"p_user_id", user_id);

            await cmd.ExecuteNonQueryAsync();
        }

        // =======================
        // MAPPING
        // =======================
        private static User Map(NpgsqlDataReader reader) => new User
        {
            UserId = reader.IsDBNull(0) ? default : reader.GetInt32(0),
            Email = reader.IsDBNull(1) ? default : reader.GetString(1),
            Password = reader.IsDBNull(2) ? default : reader.GetString(2),
            FirstName = reader.IsDBNull(3) ? default : reader.GetString(3),
            LastName = reader.IsDBNull(4) ? default : reader.GetString(4),
            PhoneNumber = reader.IsDBNull(5) ? default : reader.GetString(5),
            Address = reader.IsDBNull(6) ? default : reader.GetString(6),
            Created = reader.IsDBNull(7) ? default : reader.GetDateTime(7)
        };

        private static string ToPascal(string name) => string.Concat(name.Split('_').Select(s => char.ToUpper(s[0]) + s[1..]));

    }
}



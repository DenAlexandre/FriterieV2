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



    public class ProductStore(IConfiguration configuration, ILogger<IProductStore> logger) : IProductStore
    {

        private const string FN_GET_PRODUCTS_BDD = "select * from friterie.fn_get_products";

        #region variables

        private readonly ILogger<IProductStore> _logger = logger;

        private readonly string _connectionString = configuration.GetConnectionString("SDRDb") ?? throw new InvalidOperationException("Missing [SDRDb] connection string.");
        private List<Aliment> _aliments = new List<Aliment>();


        #region Products




        public async Task<Product> GetProductById(int id)
        {
            var articles = new Product();
            try
            {

                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();


                string fn_call = FN_GET_PRODUCTS_BDD;
                string ps_parameters = "(@in_type ,@in_limit,@in_offset)";

                using NpgsqlCommand command = new(fn_call + ps_parameters, conn);
                {

                    //command.Parameters.AddWithValue("in_type", NpgsqlDbType.Integer, in_type);


                    using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader is not null)
                    {
                        await reader.ReadAsync();
                        {
                            try
                            {
                                //SELECT a.art_id, a.art_nom, a.art_desc, a.art_prix, a.art_url_img, a.art_type, c.id_categorie, c.nom_categorie
                                articles = new Product()
                                {
                                    TypeProduct = new TypeProduct
                                    {
                                        TypeProductCode = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                        TypeProductNom = reader.GetString(8) as string,
                                    },
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1) as string,
                                    Description = reader.GetString(2) as string,
                                    Price = reader.GetDecimal(3),
                                    ImageUrl = reader.GetString(4) as string,
                                    Stock = reader.GetInt32(5),
                                };
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message, ex);
                                throw;
                            }



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            return articles;
        }

        #endregion







        public async Task<List<Product>> GetProducts(int in_type, int in_limit, int in_offset)
        {

            var articles = new List<Product>();
            try
            {

                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();


                string fn_call = FN_GET_PRODUCTS_BDD;
                string ps_parameters = "(@in_type ,@in_limit,@in_offset)";

                using NpgsqlCommand command = new(fn_call + ps_parameters, conn);
                {

                    command.Parameters.AddWithValue("in_type", NpgsqlDbType.Integer, in_type);
                    command.Parameters.AddWithValue("in_limit", NpgsqlDbType.Integer, in_limit);
                    command.Parameters.AddWithValue("in_offset", NpgsqlDbType.Integer, in_offset);

                    using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader is not null)
                    {
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                //SELECT a.art_id, a.art_nom, a.art_desc, a.art_prix, a.art_url_img, a.art_type, c.id_categorie, c.nom_categorie
                                articles.Add(new Product()
                                {
                                    TypeProduct = new TypeProduct
                                    {
                                        TypeProductCode = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                        TypeProductNom = reader.GetString(8) as string,
                                    },
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1) as string,
                                    Description = reader.GetString(2) as string,
                                    Price =  reader.GetDecimal(3),
                                    ImageUrl = reader.GetString(4) as string,
                                    Stock = reader.GetInt32(5),
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message, ex);
                                throw;
                            }



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            return articles;
        }


        #endregion


    }
}



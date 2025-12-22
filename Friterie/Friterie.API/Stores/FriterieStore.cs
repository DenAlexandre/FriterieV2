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



    public class FriterieStore(IConfiguration configuration, ILogger<IFriterieStore> logger) : IFriterieStore
    {
        private const string FN_GET_COUNT_ALIMENTS_BDD = "select * from friterie.fn_get_count_aliments";
        private const string FN_GET_ALIMENTS_BDD = "select * from friterie.fn_get_aliments";
        private const string FN_GET_GROUPE_ALIMENTS_BDD = "select * from friterie.fn_get_groupes_aliments";

        private const string FN_GET_PRODUCTS_BDD = "select * from friterie.fn_get_products";

        #region variables

        private readonly ILogger<IFriterieStore> _logger = logger;

        private readonly string _connectionString = configuration.GetConnectionString("SDRDb") ?? throw new InvalidOperationException("Missing [SDRDb] connection string.");
        private List<Aliment> _aliments = new List<Aliment>();

        #endregion



        #region FriterieStore


        public async Task<long> GetCountAliments()
        {
            long compteur = 0;
            try
            {

                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();


                string fn_call = FN_GET_COUNT_ALIMENTS_BDD;
                string ps_parameters = "()";

                using NpgsqlCommand command = new(fn_call + ps_parameters, conn);
                {
                    using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader is not null)
                    {
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                compteur = (long)reader.GetInt32(0);
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
            return compteur;
        }
        public async Task<List<Aliment>> GetAliments(int in_type, int in_limit, int in_offset)
        {
            var aliments = new List<Aliment>();
            try
            {



                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();


                string fn_call = FN_GET_ALIMENTS_BDD;
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

                                aliments.Add(new Aliment()
                                {
                                    TGroupeAliment = new GroupeAliment
                                    {
                                        TGroupeCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                        TSsGroupeCode = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                        TSsSsGroupeCode = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                        TGroupeNom = reader.GetString(3) as string,
                                        TSsGroupeNom = reader.GetString(4) as string,
                                        TSsSsGroupeNom = reader.GetString(5) as string
                                    },
                                    TAlimentCode = reader.GetInt32(6) as int?,
                                    TAlimentNom = reader.GetString(7) as string,
                                    TProteines = reader.IsDBNull(8) ? null : reader.GetDecimal(8) as decimal?,
                                    TGlucides = reader.IsDBNull(9) ? null : reader.GetDecimal(9) as decimal?,
                                    TLipides = reader.IsDBNull(10) ? null : reader.GetDecimal(10) as decimal?,
                                    TEnergie = reader.IsDBNull(11) ? null : reader.GetDecimal(11) as decimal?,
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
            return aliments;
        }



        public async Task<Dictionary<int, Dictionary<int, List<GroupeAliment>>>> GetGroupesAliments()
        {
            var dico = new Dictionary<int, Dictionary<int, List<GroupeAliment>>>();
            try
            {



                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();


                string fn_call = FN_GET_GROUPE_ALIMENTS_BDD;
                string ps_parameters = "()";

                using NpgsqlCommand command = new(fn_call + ps_parameters, conn);
                {

                    using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader is not null)
                    {
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                GroupeAliment ga = new GroupeAliment
                                {
                                    TGroupeCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    TSsGroupeCode = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                    TSsSsGroupeCode = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                    TGroupeNom = reader.GetString(3) as string,
                                    TSsGroupeNom = reader.GetString(4) as string,
                                    TSsSsGroupeNom = reader.GetString(5) as string
                                };

                                if (!dico.ContainsKey(ga.TGroupeCode))
                                    dico.Add(ga.TGroupeCode, new Dictionary<int, List<GroupeAliment>>());

                                if (!dico[ga.TGroupeCode].ContainsKey(ga.TSsGroupeCode))
                                    dico[ga.TGroupeCode].Add(ga.TSsGroupeCode, new List<GroupeAliment>());
                                
                                dico[ga.TGroupeCode][ga.TSsGroupeCode].Add(ga);
                         

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
            return dico;
        }

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



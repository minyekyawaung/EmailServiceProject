
using Npgsql;
using AplusExtension;
namespace EmailService;
public class PostgresDataContext : IDataContext
{
    IConfiguration _config;

    public PostgresDataContext(IConfiguration cofig)
    {
        _config = cofig;
    }

    public async Task<ListResponse> GetListAsync(SelectContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.getAsync(data, connection);

            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }

    }

      public async Task<ListResponse> GetListAsync1()
    {

        string a = _config["Email"];

        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
               // return await PostgresFunction.getAsync(data, connection);

            }
            catch (Exception e)
            {
                return new ListResponse
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }

        return null;

    }

    public async Task<Response> AddAsync(InsertContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.insertAsync(data, connection);

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }

    }

    public async Task<Response> UpdateAsync(UpdateContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.setAsync(data, connection);


            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
        }

    }

    public async Task<Response> RemoveAsync(DeleteContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                return await PostgresFunction.deleteAsync(data, connection);

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.InternalServerError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }

    ///this is for procedure call transaction control already set, not need to do in store procedure
    public async Task<Response> RunProcedureAsync(RunContext data)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await PostgresFunction.runAsync(data, connection);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Response
                        {
                            code = ResultCode.DatabaseError,
                            message = ex.Message
                        };
                    }

                }

            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }


    //in this methods all queries will execute under transaction control
    //to fetch data from one query result use extra value params
    public async Task<Response> TransactionAsync(List<QueryContext> requests)
    {
        using (var connection = new NpgsqlConnection(_config["DbConnection"]))
        {
            try
            {
                connection.Open();
                Dictionary<string,Response> responses = new Dictionary<string,Response>();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       

                        foreach (var request in requests)
                        {
                            
                            
                            if (request.GetType() == typeof(SelectContext))
                            {
                                var select = (SelectContext)request;
                                select.whereParams.setValues(responses);
                                var result = await PostgresFunction.getAsync(select, connection, transaction);
                                if(select.tag.IsNotNullOrEmpty()) responses.Add(select.tag,result);
                            }
                            
                            if (request.GetType() == typeof(InsertContext))
                            {         
                                var insert = (InsertContext)request;                         
                                insert.data.setValues(responses);
             
                                var result = await PostgresFunction.insertAsync(insert, connection, transaction);
                                if(insert.tag.IsNotNullOrEmpty()) responses.Add(insert.tag,result);
                            }
                            if (request.GetType() == typeof(UpdateContext))
                            {
                                var update = (UpdateContext)request;
                                update.data.setValues(responses);
                                update.whereParams.setValues(responses);
                               
                                var result = await PostgresFunction.setAsync(update, connection, transaction);
                                if(update.tag.IsNotNullOrEmpty()) responses.Add(update.tag,result);
                            }
                            if (request.GetType() == typeof(DeleteContext))
                            {
                                var delete = (DeleteContext)request;
                                delete.whereParams.setValues(responses);
                                var result = await PostgresFunction.deleteAsync(delete, connection, transaction);
                                if(delete.tag.IsNotNullOrEmpty()) responses.Add(delete.tag,result);
                            }
                          
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Response
                        {
                            code = ResultCode.DatabaseError,
                            message = ex.Message
                        };
                    }
                    finally{
                        
                    }
                }

                return responses.Last().Value;
            }
            catch (Exception e)
            {
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = e.Message
                };
            }
            finally
            {
                connection.Close();
            }
        }
    }

    


}
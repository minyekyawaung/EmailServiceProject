using System.Runtime.InteropServices;
using AplusExtension;
using Dapper;
using EmailService;
using Newtonsoft.Json;
using Npgsql;

public class LogDbFunction {
     public static async Task<ListResponse> getAsync(SelectContext data,  [Optional]NpgsqlTransaction transaction){
        string query = $"Select {data.fields} from {data.tables}";
                object parameters = new object{};
                //adding where clause 
                //instead of using value directly
                //consider using parameter to prevent SQL injection
                //eg where name = @name and then add @name value in request parameters
               if (data.where.IsNotNullOrEmpty())
                {
                    query = $"{query} where {data.where}";
                    parameters = data.whereParams;
                }
     
                //adding group by clause
                if (data.groupBy.IsNotNullOrEmpty())
                {
                    query = $"{query} group by {data.groupBy}";
                }

                //adding order by clause
                if (data.orderBy.IsNotNullOrEmpty())
                {
                    query = $"{query}  order by {data.orderBy}";
                }


                query = $"{query} offset {((data.page - 1) * data.pageSize)} limit {data.pageSize};";



                Console.WriteLine(query);
                Console.WriteLine(JsonConvert.SerializeObject(parameters));

                
                return new ListResponse
                {
                    code = ResultCode.DatabaseError,
                    page = data.page,
                    pageSize = data.pageSize,
                };
    }
    public static async Task<Response> insertAsync(InsertContext data,  [Optional]NpgsqlTransaction transaction){
         string columns = String.Join(",", data.data.Select(x=>x.Key));
                string values = String.Join(",", data.data.Select(x=>$"@{x.Key}"));
                Dictionary<string,object> parameters = data.data;
                
      
                string query = $"INSERT INTO {data.table} ({columns}) VALUES ({values}) RETURNING *;";

                Console.WriteLine(query);
                Console.WriteLine(JsonConvert.SerializeObject(parameters));
                
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = "ok",
                    rows = new List<IDictionary<string,object>>()
                };
    }
    
    public static async Task<Response> setAsync(UpdateContext data,  [Optional]NpgsqlTransaction transaction)
    {
        string values = String.Join(",", data.data.Select(x=>$"{x.Key} = @{x.Key}"));
                Dictionary<string,object> updatedata = data.data;
                Dictionary<string,object> wherevalues = data.whereParams;

                var parameters = updatedata.Concat(wherevalues);

                string query = $"UPDATE {data.table} SET {values} WHERE {data.where} RETURNING *;";

                Console.WriteLine(query);
                Console.WriteLine(JsonConvert.SerializeObject(parameters));
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = "ok",
                   rows = new List<IDictionary<string,object>>()
                };
    }

    public static async Task<Response> deleteAsync(DeleteContext data, [Optional]NpgsqlTransaction transaction){
         Dictionary<string,object> parameters = data.whereParams;

                string query = $"DELETE FROM {data.table}  WHERE {data.where} RETURNING *;";

                Console.WriteLine(query);
                Console.WriteLine(JsonConvert.SerializeObject(parameters));
                
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = "ok",
                    rows = new List<IDictionary<string,object>>()
                };
     }

     public static async Task<Response> runAsync(RunContext context,  [Optional]NpgsqlTransaction transaction){
         
                
                Console.WriteLine(context.procedure);
                Console.WriteLine(JsonConvert.SerializeObject(context.values));
                
                return new Response
                {
                    code = ResultCode.DatabaseError,
                    message = "ok",
                   rows = new List<IDictionary<string,object>>()
                };
     }

}
using AplusExtension;
using Newtonsoft.Json;

namespace EmailService;

public class QueryInsert
{
    public List<Parameter> _data { get; set; }
    private Query _query { get; set; }

    private string _tag { get; set; }

   public QueryInsert As(string tag)
    {
        _tag = tag;
        return this;
    }

    public QueryInsert(Query query, object data)
    {
        _data = data.ToDictionary().toParameterList();
        _query = query;
    }

    public CreateRequest Request(){
        return new CreateRequest {
            table = this._query._tables,
            data = this._data,
           tag = this._tag,
        };
    }

    public DataServiceContract Contract(){
        return new QueryContract{
            type = QueryTypes.Create,
            request = JsonConvert.SerializeObject(this.Request())
        };
    }

    public async Task<Response> ExecuteAsync(IDataContext db){
        return await db.AddAsync(this.Request().toInsert());
    }
}
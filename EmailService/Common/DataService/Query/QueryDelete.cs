
using AplusExtension;
using Newtonsoft.Json;

namespace EmailService;
public class QueryDelete
{

    private Query _query { get; set; }
    private string _tag { get; set; }

    private string _where { get; set; }
    public List<Parameter>? _parameters { get; set; }

    public QueryDelete Where(string where, object parameter)
    {
        _where = where.isParameterized();
        _parameters = parameter.ToDictionary().toParameterList();
        return this;
    }


    public QueryDelete(Query query)
    {
        _query = query;
    }

    public QueryDelete As(string tag)
    {
        _tag = tag;
        return this;
    }

    public RemoveRequest Request()
    {
        if (this._query.IsNotNullOrEmpty() && this._where.IsNotNullOrEmpty())
        {
            return new RemoveRequest
            {
                table = this._query._tables,
                tag = this._tag,
                filter = new Filter
                {
                    where = this._where,
                    parameters = this._parameters,

                },


            };
        }
        else
        {
            throw new Exception("Where clause is required for delete query");
        }


    }

    public DataServiceContract Contract(){
        return new QueryContract{
            type = QueryTypes.Remove,
            request = JsonConvert.SerializeObject(this.Request())
        };
    }

    public async Task<Response> ExecuteAsync(IDataContext db){
        return await db.RemoveAsync(this.Request().toDelete());
    }

}
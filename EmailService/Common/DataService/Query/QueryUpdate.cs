using AplusExtension;
using Newtonsoft.Json;

namespace EmailService;
public class QueryUpdate
{
    private List<Parameter> _data { get; set; }
    private Query _query { get; set; }

    private string _tag { get; set; }

    public QueryUpdate As(string tag)
    {
        _tag = tag;
        return this;
    }

    private string _where { get; set; }
    public List<Parameter>? _parameters { get; set; }

    public QueryUpdate Where(string where, object parameter)
    {
        _where = where.isParameterized();
        _parameters = parameter.ToDictionary().toParameterList();
        return this;
    }

    public QueryUpdate(Query query, object data)
    {
        _data = data.ToDictionary().toParameterList();
        _query = query;
    }

    public UpdateRequest Request()
    {
        if (this._query.IsNotNullOrEmpty() && this._where.IsNotNullOrEmpty())
        {
            return new UpdateRequest
            {
                table = this._query._tables,
                filter = new Filter
                {
                    where = this._where,
                    parameters = this._parameters
                },
                data = this._data,
                tag = this._tag,
            };
        }
        else
        {
            throw new Exception("Where clause is required for update query");
        }
    }

    public DataServiceContract Contract(){
        return new QueryContract{
            type = QueryTypes.Update,
            request = JsonConvert.SerializeObject(this.Request())
        };
    }
    public async Task<Response> ExecuteAsync(IDataContext db){
        return await db.UpdateAsync(this.Request().toUpdate());
    }
}
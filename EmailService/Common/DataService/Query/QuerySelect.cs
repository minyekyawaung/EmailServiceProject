using AplusExtension;
using Newtonsoft.Json;

namespace EmailService;
public class QuerySelect
{
   

    private string fields { get; set; }
    private Query query { get; set; }
    private string? orderBy { get; set; }
    private string? groupBy { get; set; }

    private int limit { get; set; } = 10;

    private int page { get; set; } = 1;

    private string _tag { get; set; }


    private string _where { get; set; }
    public List<Parameter>? _parameters { get; set; }

    public QuerySelect Where(string where, object parameter)
    {
        _where = where.isParameterized(true);
        _parameters = parameter.ToDictionary().toParameterList();
        return this;
    }

    public QuerySelect As(string tag)
    {
        _tag = tag;
        return this;
    }



    public QuerySelect(Query _query, string _fields = "*")
    {
        fields = _fields;
        query = _query;
    }

    public QuerySelect Order(string order)
    {
        orderBy = order;
        return this;
    }

    public QuerySelect Group(string group)
    {
        groupBy = group;
        return this;
    }

    public QuerySelect Page(int page)
    {
        page = page;
        return this;
    }

    public QuerySelect Limit(int limit)
    {
        limit = limit;
        return this;
    }

    public GetRequest Request()
    {
        return new GetRequest
        {
            page = this.page,
            pageSize = this.limit,
            tables = this.query._tables,
            fields = this.fields,
            groupBy = this.groupBy,
            orderBy = this.orderBy,
            filter = new Filter
            {
                where = this._where,
                parameters = this._parameters
            },
            tag = this._tag,

        };
    }

    public DataServiceContract Contract(){
        return new QueryContract{
            type = QueryTypes.Listing,
            request = JsonConvert.SerializeObject(this.Request())
        };
    }

    public async Task<ListResponse> ExecuteAsync(IDataContext db){
        return await db.GetListAsync(this.Request().toSelect());
    }

}

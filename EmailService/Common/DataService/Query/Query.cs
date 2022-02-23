
using AplusExtension;
namespace EmailService;


public class Query
{
    
    public string _tables { get; set; }
    public Query(string tables)
    {
        _tables = tables;
    }
    
    

    public QuerySelect Select(string fields)
    {
        return new QuerySelect(this, fields);
    }

    public QueryInsert Insert(object data)
    {
        return new QueryInsert(this, data);
    }

   
    public QueryUpdate Update(object data)
    {
        return new QueryUpdate(this, data);
    }

    public QueryDelete Delete()
    {
        return new QueryDelete(this);
    }

    

}











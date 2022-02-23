namespace EmailService;

public interface DataServiceContract{
    QueryTypes type {get;set;}
    string request{get;set;}
}

public class QueryContract : DataServiceContract
{
    public QueryTypes type { get;set; }
    public string request { get;set; }
}
namespace EmailService;
public class ListResponse : Response{

    public int page {get;set;}
    public int pageSize{get;set;}

    public long total{get;set;}
    
}
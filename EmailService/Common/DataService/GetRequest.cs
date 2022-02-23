namespace EmailService;
public class GetRequest : QueryRequest 
{
   public int page {get;set;} = 1;
   public int pageSize{get;set;} = 10;
   public string fields{get;set;} = "*";
   public Filter? filter{get;set;}
   public string? tables {get;set;}

   public string? orderBy {get;set;}
   public string? groupBy {get;set;}


}
namespace EmailService;
public class SelectContext : QueryContext{
   public int page {get;set;} = 1;
   public int pageSize{get;set;} = 10;
   public string fields{get;set;} = "*";

   public string where{get;set;}
   public Dictionary<string,object> whereParams {get;set;}
   public string? tables {get;set;}

   public string? orderBy {get;set;}
   public string? groupBy {get;set;}
   public string? tag { get; set; }
}
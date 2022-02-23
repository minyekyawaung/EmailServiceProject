namespace EmailService;
public class DeleteContext : QueryContext{

   public string? table {get;set;}
   public string where{get;set;}
   public Dictionary<string,object> whereParams {get;set;}
   public string? tag { get; set; }
}
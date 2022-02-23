namespace EmailService;
public class UpdateContext : QueryContext
{

   public string? table {get;set;}
   public Dictionary<string,object> data {get;set;}

   public string where{get;set;}
   public Dictionary<string,object> whereParams {get;set;}

   public string? tag { get; set; }
}
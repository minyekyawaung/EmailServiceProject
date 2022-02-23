namespace EmailService;
public class InsertContext : QueryContext {

   public string? table {get;set;}
   public Dictionary<string,object>? data{get;set;} 
   public string? tag { get; set; }
}
namespace EmailService;
public class RunContext : QueryContext
{


   public string? procedure {get;set;}
   public Dictionary<string,object> values {get;set;}
   public string? tag { get; set; }
}
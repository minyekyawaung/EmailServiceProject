namespace EmailService;
public class UpdateRequest  : QueryRequest{

   public string? table {get;set;}
   public List<Parameter>? data{get;set;} 
   public Filter? filter{get;set;}
   
}
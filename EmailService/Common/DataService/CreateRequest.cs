namespace EmailService;
public class CreateRequest : QueryRequest {

   public string? table {get;set;}
   public List<Parameter>? data{get;set;} 

  

}
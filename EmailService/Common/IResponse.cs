public interface IResponse {
 
   ResultCode code{get;set;}
   string? message{get;set;}
   
   List<IDictionary<string,object>>? rows{get;set;} 

}
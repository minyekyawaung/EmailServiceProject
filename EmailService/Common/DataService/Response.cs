namespace EmailService;
public class Response : IResponse{
    public ResultCode code { get;set; }
    public string? message { get;set; }
    public List<IDictionary<string,object>>? rows{get;set;} 
}
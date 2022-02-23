using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
using Newtonsoft.Json;

namespace EmailService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    IRequestClient<DataServiceContract> _client;

    private readonly ILogger<TestController> _logger;
    private IDataContext _db;
    public TestController(ILogger<TestController> logger, IDataContext db,IRequestClient<DataServiceContract> client)
    {
        _client = client;
        _logger = logger;
        _db = db; //http://localhost:5033/AddContext
    }

       [HttpPost]
        public async Task<EmailService.Response> Add()
        {
            var newuser = new {
                uid = 4,
                nrc = "12/pzt(N)9400",
                pin = "2000033",
                mobile_no = "090400440",
                email = "thiargy@abank.com.mm",
                createdat = (DateTimeOffset)DateTime.Now,
                editedat = (DateTimeOffset)DateTime.Now,
                deleteflag = false
            };
            var contract = new Query("users").Insert(newuser).Contract();

            //direct db calling
           var dbresult = await new Query("users").Insert(newuser).ExecuteAsync(_db);
            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            

            var result = await _client.GetResponse<ResultData>(contract);
            var data = result.Message.response.rows.toList<users>();
            return   (Response)result.Message.response;
        
        }

    [HttpPost]
    public async Task<object> List(int page, int pageSize)
    {
       
 var paras = new
        {
            uid = 2
        };
//var contract = new Query("users").Select("id,uid,nrc,mobile_no,deviceid").Where("id = users.id", paras).Order("id").Limit(10).Page(1).Contract();
//var contract = new Query("users").Select("id,count(nrc)").Group("id").Where("id = users.id", paras).Order("id").Limit(10).Page(1).Contract();
//var contract = new Query("users INNER JOIN otp ON users.mobile_no = otp.Mobile_no").Select("users.id,count(users.mobile_no) as mobile_no").Group("users.id having users.id = 6").Where("users.id = users.id", paras).Order("users.id").Limit(10).Page(1).Contract();

var contract = new Query("users INNER JOIN otp ON users.mobile_no = otp.Mobile_no").
Select("users.id,count(users.mobile_no) as Noofcount").Group("users.id having users.id = 2" ).
Where("users.id = users.id", paras).Order("users.id").Limit(10).Page(1).Contract();


        var result = await _client.GetResponse<ListData>(contract);

        //var result1 = await _db.GetListAsync1();
        var data = result.Message.response.rows.toList<users>();
        return result.Message.response;
      
       return null;
    }

      [HttpPost]
        public async Task<EmailService.Response> Update()
        {
            
            var edituser = new users{
                UID = 250,
                NRC = "12/pzt(N)940045"
            };

            var contract = new Query("users").Update(edituser).Where("id=@id and uid=@uid",new {id=15,uid=234}).Contract();
             
            var result = await _client.GetResponse<ResultData>(contract);
          
            return result.Message.response;
        
        }

    [HttpPost]
    public async Task<EmailService.Response> Remove(int id = 10)
    {
        var contract = new Query("users").Delete().Where("id = @nid", new { nid = id }).Contract();

        var result = await _client.GetResponse<ResultData>(contract);

        return result.Message.response;

    }

    [HttpPost]
        public async Task<EmailService.Response> Transaction()
        {
         
            List<dynamic> arr = new List<dynamic>();

            var r1 = new Query("users")
            .Update(new{uid = 72,deviceid = "123"})
            .Where("id = @id",new { id= 2, })
            .As("u1").Request();

            var r2 = new Query("users").Update(new{deviceid = "@u1.deviceid"}).Where("id = @userid",new{userid = 7}).Request();
             
            arr.Add(r1);
            arr.Add(r2);
            var contract = arr.toContract();


            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            var result = await _client.GetResponse<ResultData>(contract);
            return   (Response)result.Message.response;
        
        }

    [HttpPost]
    public async Task<Dictionary<string,string>> Concurrent()
    {
        Dictionary<string,string> results = new Dictionary<string, string>();
        var series = Enumerable.Range(1, 10).ToList();
        foreach(var i in series) {
            var result = await _client.GetResponse<TestData>(new QueryContract{
                type = QueryTypes.Test,
                request = "i-" + i.ToString() 
            });
            results.Add("i"+i, result.Message.response);
            
        }

        foreach(var j in series) {
            var result = await _client.GetResponse<TestData>(new QueryContract{
                type = QueryTypes.Test,
                request = "j-" + j.ToString() 
            });
            results.Add("j"+ j, result.Message.response);
        }
        

       return results;

    }
        
}



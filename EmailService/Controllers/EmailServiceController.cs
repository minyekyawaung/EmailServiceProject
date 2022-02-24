using Microsoft.AspNetCore.Mvc;
using AplusExtension;
using MassTransit;
using Newtonsoft.Json;
using System.Net;  
using System.Net.Mail;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;



namespace EmailService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EmailServiceController : ControllerBase
{
    IRequestClient<DataServiceContract> _client;
     IConfiguration _config;

  

    private readonly ILogger<TestController> _logger;
    private IDataContext _db;
    public EmailServiceController(ILogger<TestController> logger, IDataContext db,IRequestClient<DataServiceContract> client,IConfiguration cofig)
    {
        _client = client;
        _logger = logger;
        _db = db; //http://localhost:5033/AddContext
         _config = cofig;
       
    }

       [HttpPost]
        public async Task<EmailService.Response> SendEmail(emails _emails)
        {
          
            try
            {
            
            string Email = _config["Email"];
            string Password = _config["Password"];

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(Email);
            msg.To.Add(_emails.Address);
            msg.Subject = _emails.Title;
            msg.Body = _emails.Body;
            //msg.Priority = MailPriority.High;


            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Email,Password);
                client.Host =  _config["Host"];
                client.Port = Convert.ToInt16(_config["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }

          
             var newuser = new {
               
                title = _emails.Title,
                body = _emails.Body,
                address = _emails.Address
             
            };
            var contract = new Query("emails").Insert(newuser).Contract();

            //direct db calling
           //var dbresult = await new Query("emails").Insert(newuser).ExecuteAsync(_db);
            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            
           
            var result = await _client.GetResponse<ResultData>(contract);
            var data = result.Message.response.rows.toList<emails>();
            return   (Response)result.Message.response;
            }
            catch (IOException e)
            {
                throw;
            }
        
        }


        [HttpPost]
        public async Task<EmailService.Response> SendEmailWithAttachment(emails _emails)
        {
          
            try
            {
            
            string Email = _config["Email"];
            string Password = _config["Password"];

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(Email);
            msg.To.Add(_emails.Address);
            msg.Subject = _emails.Title;
            msg.Body = _emails.Body;
            //msg.Priority = MailPriority.High;

              System.Net.Mail.Attachment attachment;
              attachment = new System.Net.Mail.Attachment(_emails.Attachment);
              msg.Attachments.Add(attachment);


            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Email,Password);
                client.Host =  _config["Host"];
                client.Port = Convert.ToInt16(_config["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }

          
             var newuser = new {
               
                title = _emails.Title,
                body = _emails.Body,
                address = _emails.Address
             
            };
            var contract = new Query("emails").Insert(newuser).Contract();

            //direct db calling
           //var dbresult = await new Query("emails").Insert(newuser).ExecuteAsync(_db);
            //direct test 
          /*   var result = await _db.AddAsync(new CreateRequest{
                 table ="users",
                 data = parameters
             });
             */
            
           
            var result = await _client.GetResponse<ResultData>(contract);
            var data = result.Message.response.rows.toList<emails>();
            return   (Response)result.Message.response;
            }
            catch (IOException e)
            {
                throw;
            }
        
        }

        [HttpPost]
        public async Task<EmailService.Response> MultiSendEmail(emails _emails)
        {
          
            try
            {
            
            string Email = _config["Email"];
            string Password = _config["Password"];

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(Email);
            //Adding Multiple recipient email id logic
            string[] Multi =_emails.Address.Split(','); //spiliting input Email id string with comma(,)
            foreach (string Multiemailid in Multi)
            {
                msg.To.Add(new MailAddress(Multiemailid)); //adding multi reciver's Email Id
            }
            msg.To.Add(_emails.Address);
            msg.Subject = _emails.Title;
            msg.Body = _emails.Body;
            //msg.Priority = MailPriority.High;


            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Email,Password);
                client.Host =  _config["Host"];
                client.Port = Convert.ToInt16(_config["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }

            //var result ;
            var contract = new object();
            var result = new object();
            var data = new object();

            foreach (string Multiemailid in Multi)
            {
                var newuser = new {
                                
                title = _emails.Title,
                body = _emails.Body,
                address = Multiemailid
                
             
            };
            contract = new Query("emails").Insert(newuser).Contract();
            result = await _client.GetResponse<ResultData>(contract);
            //data = result.Message.response.rows.toList<emails>();

         
            }
          
            
            //return   (Response)result.Message.response;
            return null;


            }
            catch (IOException e)
            {
                throw;
            }
        
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

        //var result = await _db.GetListAsync(data);
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



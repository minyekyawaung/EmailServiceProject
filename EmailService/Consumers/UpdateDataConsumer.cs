using MassTransit;
using AplusExtension;
namespace EmailService;

public class UpdateDataConsumer : IConsumer<UpdateData> 
{
    private IDataContext _db;
    public UpdateDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<UpdateData> context)
    {   
        
        var result = await _db.UpdateAsync(RequestTransformer.toUpdate(context.Message.request));
       
        await context.RespondAsync<ResultData>(new
        {
            response = result
        });
    }

    
}


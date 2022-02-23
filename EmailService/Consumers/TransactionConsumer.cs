using MassTransit;
namespace EmailService;

public class TransactionConsumer : IConsumer<TransactionData> 
{
    private IDataContext _db;
    public TransactionConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<TransactionData> context)
    {   
        var data = RequestTransformer.toListRequests(context.Message.requests);
        var result = await _db.TransactionAsync(data);
       
        await context.RespondAsync<ResultData>(new
        {
            response = (Response)result
        });
    }
}


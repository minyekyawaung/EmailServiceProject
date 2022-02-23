using MassTransit;
using AplusExtension;
namespace EmailService;

public class GetListDataConsumer : IConsumer<GetList> 
{
    private IDataContext _db;
    public GetListDataConsumer( IDataContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<GetList> context)
    {   
      
        var result = await _db.GetListAsync(RequestTransformer.toSelect(context.Message.request));
       
        await context.RespondAsync<ListData>(new
        {
            response = result
        });
    }

   
}



using MassTransit;
using AplusExtension;
namespace EmailService;

public class QueryConsumer : IConsumer<DataServiceContract> 
{
    private IDataContext _db;
    public QueryConsumer( IDataContext db)
    {
        _db = db;
    }


    public async Task Consume(ConsumeContext<DataServiceContract> context)
    {
        await context.Process(_db);
    }
}


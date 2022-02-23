using MassTransit;
using Newtonsoft.Json;

namespace EmailService;
public static partial class SQLExtensions
{
    public static async Task Process(this ConsumeContext<DataServiceContract> context,IDataContext _db){

       
        if( context.Message.type == QueryTypes.Listing){
            var result = await _db.GetListAsync(RequestTransformer.toSelect(JsonConvert.DeserializeObject<GetRequest>(context.Message.request)));   
            await context.RespondAsync<ListData>(new 
            {
                response = result
            });
        }
        else if(context.Message.type == QueryTypes.Create) {
            var result = await _db.AddAsync(RequestTransformer.toInsert(JsonConvert.DeserializeObject<CreateRequest>(context.Message.request)));
       
            await context.RespondAsync<ResultData>(new 
            {
                response = result
            });
        }
        else if(context.Message.type == QueryTypes.Update) {
            var result = await _db.UpdateAsync(RequestTransformer.toUpdate(JsonConvert.DeserializeObject<UpdateRequest>(context.Message.request)));
       
            await context.RespondAsync<ResultData>(new 
            {
                response = result
            });
        }
        else if(context.Message.type == QueryTypes.Remove) {
            var result = await _db.RemoveAsync(RequestTransformer.toDelete(JsonConvert.DeserializeObject<RemoveRequest>(context.Message.request)));
       
            await context.RespondAsync<ResultData>(new
            {
                response = result
            });
        }
        else if (context.Message.type == QueryTypes.Transaction) {
            var result = await _db.TransactionAsync(RequestTransformer.toListRequests(JsonConvert.DeserializeObject<List<TypedQuery>>(context.Message.request)));
       
            await context.RespondAsync<ResultData>(new
            {
                response = result
            });
        }
        else if(context.Message.type == QueryTypes.Test) {
            await context.RespondAsync<TestData>(new {response=context.Message.request});
        }
       
    }
    

   
}


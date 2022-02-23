using AplusExtension;
using Newtonsoft.Json;
namespace EmailService;

public static partial class RequestTransformer {
     public static SelectContext toSelect(this GetRequest request){
        return new SelectContext {
            page =  request.page,
            pageSize = request.pageSize,
            fields = request.fields,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            tables = request.tables,
            orderBy = request.orderBy,
            groupBy = request.groupBy,
            tag = request.tag,
        };
    }

    public static InsertContext toInsert(this CreateRequest request){

        return new InsertContext {
            table = request.table,
            tag = request.tag,
            data = request.data.IsNotNullOrEmpty() ? request.data.toDictionaryList() : null
        };
    }

    public static UpdateContext toUpdate(this UpdateRequest request){
        return new UpdateContext {
            table = request.table,
            tag = request.tag,
            data = request.data.IsNotNullOrEmpty() ? request.data.toDictionaryList() : null,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            
        };
    }

    public static DeleteContext toDelete(this RemoveRequest request){
        return new DeleteContext {
            table = request.table,
            tag = request.tag,
            where = request.filter.IsNotNullOrEmpty() ? request.filter.where : null,
            whereParams = request.filter.IsNotNullOrEmpty() ?request.filter.parameters.toDictionaryList() : null,
            
        };
    }

    public static List<QueryContext> toListRequests(this List<TypedQuery> requests){
      
       List<QueryContext> _results = new List<QueryContext>();

       requests.ForEach(x=>{
           if(x.type == typeof(GetRequest)){
               _results.Add(JsonConvert.DeserializeObject<GetRequest>(x.request).toSelect());
           }
           if(x.type == typeof(CreateRequest)){
               _results.Add(JsonConvert.DeserializeObject<CreateRequest>(x.request).toInsert());
           }
           if(x.type == typeof(UpdateRequest)){
                _results.Add(JsonConvert.DeserializeObject<UpdateRequest>(x.request).toUpdate());
           }
           if(x.type == typeof(RemoveRequest)){
               _results.Add(JsonConvert.DeserializeObject<RemoveRequest>(x.request).toDelete());
           }
       });

       return _results;
    }
}
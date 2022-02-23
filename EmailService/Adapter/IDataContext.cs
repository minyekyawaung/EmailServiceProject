using EmailService;


public interface IDataContext  {

    Task<ListResponse>  GetListAsync(SelectContext data);

   // Task<ListResponse>  GetListAsync1();


    Task<Response> AddAsync(InsertContext data);

    Task<Response> UpdateAsync(UpdateContext request);

    Task<Response> RemoveAsync(DeleteContext request);

    Task<Response> TransactionAsync(List<QueryContext> requests);
}
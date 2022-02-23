
namespace EmailService;



public interface GetList{
    GetRequest request {get;}
    
}

public interface ListData{
    ListResponse response {get;}
}

public interface AddData{
    CreateRequest request {get;}
    
}

public interface UpdateData{
    UpdateRequest request {get;}
    
}

public interface RemoveData{
    RemoveRequest request {get;}
    
}

public interface TransactionData{
    List<TypedQuery> requests {get;}
    
}

public interface ResultData{
    Response response {get;}
}

public interface TestData{
    string response {get;}
}
public enum ResultCode : ushort
{
    Continue = 100,
    Processing = 102,
    OK = 200,
    Created = 201,
    Accepted = 202,
    NoContent = 204,

    ResetContent = 205,

    MultiChoice = 300,

    MovedPermanently = 301,

    BadRequest = 400,
    Unauthorized = 401,

    Forbidden = 403,

    NotFound = 404,

    MethodNotAllowed = 405,

    NotAcceptable = 406,

    RequestTimeout = 408,

    Conflicts = 409,

    LengthRequired = 411,

    PayloadTooLarge = 413,

    UnsupportedMediaType = 415,
    ExpectationFailed = 417,
    UnporcessableEntity = 422,
    UpgradeRequired = 426,
    PreconditionRequired = 427,
    TooManyRequests = 429,

    InternalServerError = 500,
    NotImplemented = 501,

    ServiceUnavailable = 503,

    NetworkAuthenticationRequired = 511,

    DatabaseError = 520,

    TimeoutError = 521,

}
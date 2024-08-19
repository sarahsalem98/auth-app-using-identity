namespace AuthApp.Models
{
    public class ResultMV<T> where T : class 
    {
        public Status Status { get; set; }=Status.Success;
        public T Data { get; set; } 
        public ServerInfo ServerInfo { get; set; }  


    }

    public class ServerInfo
    {
        public CustomeStatusCodes CustomeStatusCode { get; set; }
        public string? Message { get; set; }
    }

    public enum Status
    {
        Success = 1,
        notActive = 5,
        duplicated = -2,
        Error = -1,
        Unauthorized = -6,
        NotFound = -9,
        notValid = -5

    }
    public enum CustomeStatusCodes
    {
        ExistedBefore = -1,
        NotConfrimed = -2,
        InternalServerError = 500,
        Success = 200,
        NotFound = 404,
        BadRequest = 400,
        Conflict = 409,
        Unauthorized = 401,
        duplicated = 412,
        notValid = 413

    }
}

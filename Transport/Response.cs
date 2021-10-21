namespace mmksi_middleware.Transport
{
    public class ResponseBadRequest
    {
        public int StatusCode {get; set;}
        public string Message {get; set;}
    }

    public class UserResponse
    {
        public string IdToken { get; set; }        
        public string RefreshToken { get; set; }
    }
}
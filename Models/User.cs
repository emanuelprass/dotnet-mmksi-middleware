namespace mmksi_middleware.Models
{    
    public class UserItem
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string IdToken { get; set; }        
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}
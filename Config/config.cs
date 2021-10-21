using Amazon;

namespace mmksi_middleware.Config
{
    public class AwsCognito
    {
        public string PoolId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public RegionEndpoint Region { get; set; }
    }    
}
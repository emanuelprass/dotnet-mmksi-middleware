using Newtonsoft.Json;

namespace mmksi_middleware.Models
{
    public class Vehicle
    {
      [JsonProperty("Data")]
      public Data[] Data { get; set; }
    }

    public class Data
    {
      [JsonProperty("BrandId")]
      public int BrandId { get; set; }

      [JsonProperty("BrandName")]
      public string BrandName { get; set; }

      [JsonProperty("Models")]
      public Models[] Models { get; set; }
    }

    public class Models
    {
      [JsonProperty("ModelId")]
      public int ModelId { get; set; }

      [JsonProperty("ModelName")]
      public string ModelName { get; set; }

      [JsonProperty("Variants")]
      public Variants[] Variants { get; set; }
   }

    public class Variants
    {
      [JsonProperty("id")]
      public int Id { get; set; }
      
      [JsonProperty("name")]
      public string Name { get; set; }
   }

}
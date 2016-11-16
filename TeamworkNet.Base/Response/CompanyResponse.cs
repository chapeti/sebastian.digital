using Newtonsoft.Json;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class CompanyResponse
  {
    [JsonProperty("company")]
    public Company Company { get; set; }

    [JsonProperty("STATUS")]
    public string STATUS { get; set; }
  }
}

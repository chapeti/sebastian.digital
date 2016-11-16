using System.Collections.Generic;
using Newtonsoft.Json;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class CompaniesResponse
  {
    [JsonProperty("companies")]
    public List<Company> Companies { get; set; }

    [JsonProperty("STATUS")]
    public string STATUS { get; set; }
  }
}

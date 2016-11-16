using System.Collections.Generic;
using Newtonsoft.Json;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class PeopleResponse
  {
    [JsonProperty("people")]
    public List<Person> People { get; set; }

    [JsonProperty("STATUS")]
    public string STATUS { get; set; }

    [JsonIgnore]
    public int TotalRecords { get; set; }
    [JsonIgnore]
    public int Pages { get; set; }
    [JsonIgnore]
    public int Page { get; set; }
  }

  public class PersonResponse
  {
    [JsonProperty("person")]
    public Person Person { get; set; }

    [JsonProperty("STATUS")]
    public string STATUS { get; set; }


  }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkNet.Base.Response
{
  public class TagsResponse
  {

      [JsonProperty("tags")]
      public Model.Tag[] Tags { get; set; }

      [JsonProperty("STATUS")]
      public string STATUS { get; set; }

  }
}

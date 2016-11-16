using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{
  public class CategoryHandler
  {
    private readonly TeamWorkClient _client;
    public CategoryHandler(TeamWorkClient client)
    {
      _client = client;
    }


    public async Task<CategoriesResponse> GetProjectCategoriesAsync()
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<CategoriesResponse>("projectCategories.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (CategoriesResponse)data.ContentObj;
      }
      return new CategoriesResponse() { STATUS = "ERROR", Categories = null };
    }

    public async Task<bool> AddProjectCategory(Category company)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
          var post = JsonConvert.SerializeObject(company);
          await client.PostAsync("categories.json", new StringContent("{\"category\":" + post + "}", Encoding.UTF8));
      }
        return false;
    }

    public async Task<bool> UpdateProjectCategory(Category company)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
          var post = JsonConvert.SerializeObject(company);
          await client.PutAsync("/category/" + company.Id + ".json", new StringContent("{\"category\":" + post + "}", Encoding.UTF8));  
      }
        return false;
    }

    public async Task<bool> DeleteCProjectCategory(Category company)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
          JsonConvert.SerializeObject(company);
          await client.DeleteAsync("/category/" + company.Id + ".json");
      }
        return false;
    }

  }
}

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{
  public class CompanyHandler
  {
   private readonly TeamWorkClient _client;
   public CompanyHandler(TeamWorkClient client)
    {
      _client = client;
    }


   /// <summary>
   /// Returns all projects the user has access to
   /// </summary>
   /// <returns></returns>
   public async Task<CompaniesResponse> GetAllCompaniesAsync()
   {
     using (var client = new AuthorizedHttpClient(_client))
     {
       var data = await client.GetAsync<CompaniesResponse>("companies.json", null);
       if (data.StatusCode == HttpStatusCode.OK) return (CompaniesResponse)data.ContentObj;
     }
     return new CompaniesResponse() {STATUS = "ERROR"};
   }


   public async Task<CompanyResponse> GetCompanyAsync(int companyId)
   {
     using (var client = new AuthorizedHttpClient(_client))
     {
       var data = await client.GetAsync<ProjectsResponse>("/companies/" + companyId +".json", null);
       if (data.StatusCode == HttpStatusCode.OK) return (CompanyResponse)data.ContentObj;
     }
     return null;
   }


   public async Task<bool> AddCompany(Company company)
   {
     using (var client = new AuthorizedHttpClient(_client))
     {
         var post = JsonConvert.SerializeObject(company);
         await client.PostAsync("companies.json", new StringContent("{\"company\": " + post + "}", Encoding.UTF8));
     }
       return false;
   }

   public async Task<bool> UpdateCompany(Company company)
   {
     using (var client = new AuthorizedHttpClient(_client))
     {
         var post = JsonConvert.SerializeObject(company);
         await client.PutAsync("/companies/" + company.Id + ".json", new StringContent("{\"company\": " + post + "}", Encoding.UTF8));
     }
       return false;
   }

   public async Task<bool> DeleteCompany(Company company)
   {
     using (var client = new AuthorizedHttpClient(_client))
     {
         JsonConvert.SerializeObject(company);
         await client.DeleteAsync("/companies/" + company.Id + ".json");
     }
       return false;
   }



  }
}

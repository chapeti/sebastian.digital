using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Model;
using TeamWorkNet.Base.Response;

namespace TeamWorkNet.Base.Handler
{
  public class TagHandler
  {
    private readonly TeamWorkClient _client;
    
    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public TagHandler(TeamWorkClient client)
    {
      _client = client;
    }


    /// <summary>
    ///   Returns all tags the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<TagsResponse> GetAllTagsAsync()
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<TagsResponse>("tags.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (TagsResponse)data.ContentObj;
      }
      return null;
    }


    /// <summary>
    ///   Returns all tags the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<TagResponse> GetTagAsync(int tag_id)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<TagResponse>("/tags/" + tag_id + ".json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (TagResponse)data.ContentObj;
      }
      return null;
    }


    /// <summary>
    ///   Add a new Tag
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponse<int>> AddTagAsync(Tag theTag)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(theTag);
        return
          await client.PostWithReturnAsync("/tags.json",new StringContent("{\"tag\": " + theTag + "}", Encoding.UTF8));
      }
    }


    /// <summary>
    ///   Add a new Tag
    /// </summary>
    /// <returns></returns>
    public async Task<bool> UpdateTagAsync(Tag theTag)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          string post = JsonConvert.SerializeObject(theTag);
          await client.PutAsync("/tags.json", new StringContent("{\"tag\": " + theTag + "}", Encoding.UTF8));
          return true;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }


    /// <summary>
    /// Delete a Task
    /// </summary>
    /// <param name="tagid"></param>
    /// <returns></returns>
    public async Task<bool> DeleteTask(int tagid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.DeleteAsync("/tags/" + tagid + ".json");
      }
      return false;
    }

  }
}

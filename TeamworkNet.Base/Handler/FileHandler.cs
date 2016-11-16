using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{

  /// <summary>
  /// 
  /// </summary>
  public class PendingFile
  {
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty(PropertyName = "ref")]
    public string Reference { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class FileUploadResponse
  {
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty(PropertyName = "pendingFile")]
    public PendingFile pendingFile { get; set; }
  }

  /// <summary>
  /// Handler for Activities
  /// </summary>
  public class FileHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public FileHandler(TeamWorkClient client)
    {
      _client = client;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public byte[] FileToByteArray(string fileName)
    {
      byte[] buff = null;
      var fs = new FileStream(fileName,
                                     FileMode.Open,
                                     FileAccess.Read);
      var br = new BinaryReader(fs);
      long numBytes = new FileInfo(fileName).Length;
      buff = br.ReadBytes((int)numBytes);
      return buff;
    }

    /// <summary>
    /// Upload a file to a given project
    /// </summary>
    /// <param name="projectID"></param>
    /// <param name="description"></param>
    /// <param name="filepath"></param>
    /// <param name="filename"></param>
    /// <param name="isPrivate">default false</param>
    /// <param name="categoryID">default 0</param>
    /// <returns></returns>
    public async Task<BaseResponse<bool>> UploadFileToProject(int projectID,string description, string filepath, string filename,bool isPrivate = false, int categoryID = 0)
    {

      using (var client = new AuthorizedHttpClient(_client))
      {
        using (var content = new MultipartFormDataContent())
        {
            FileStream fs = File.OpenRead(filepath);

            var streamContent = new StreamContent(fs);
            streamContent.Headers.Add("Content-Type", "application/octet-stream");
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(filepath) + "\"");
            content.Add(streamContent, "file", Path.GetFileName(filepath));

            HttpResponseMessage message = await client.PostAsync("pendingfiles.json", content);

            if (message.StatusCode != HttpStatusCode.OK) { 
            using (Stream responseStream = await message.Content.ReadAsStreamAsync())
            {
              string jsonMessage = new StreamReader(responseStream).ReadToEnd();
              var result = JsonConvert.DeserializeObject<FileUploadResponse>(jsonMessage);

              var file = new TeamWorkFile()
              {
                CategoryId = categoryID.ToString(CultureInfo.InvariantCulture),
                CategoryName = "",
                Description = description,
                Name = filename,
                PendingFileRef = result.pendingFile.Reference,
                Isprivate = isPrivate == false ? "0" : "1"
              };

              var response = await client.PostWithReturnAsync("/projects/" + projectID + "/files.json", 
                new StringContent("{\"file\": " + JsonConvert.SerializeObject(file) + "}", Encoding.UTF8));

              if (response.StatusCode == HttpStatusCode.OK) return new BaseResponse<bool>(true,HttpStatusCode.OK);
             
            }
            }
         }
      }
      return new BaseResponse<bool>(false,HttpStatusCode.InternalServerError);
    }
  }
}      

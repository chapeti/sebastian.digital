// ==========================================================
// File: TeamWorkNet.Base.TeamWorkHTTPRequest.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;
using TeamWorkNetBase.Base.Generic;

namespace TeamWorkNet.Base
{
  public class ResponseBase
  {
    public int code { get; set; }
    public object data { get; set; }
  }

  public class AuthorizedHttpClient : HttpClient
   {
    protected readonly string serviceURL;
    public string ApiKey { get; set; }
    /// <summary>
    ///   Initialize a new Instance of the Client
    /// </summary>
    /// <param name="client">TeamWorkNet Base Client Reference</param>
    public AuthorizedHttpClient(TeamWorkClient client)
    {
      BaseAddress = new Uri(client.BaseUrl);
      DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", client.APiKey, "x"))));
      DefaultRequestHeaders.Accept.Clear();
      DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<BaseResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, string> paramsDictionary, RequestFormat format = RequestFormat.Json)
    {
      try
      {

        var data2 = Task.Run(() => GetAsync(endpoint)).Result;

       // var data = await GetAsync(endpoint);
        if (!data2.IsSuccessStatusCode) return new BaseResponse<T>(HttpStatusCode.InternalServerError);
        using (Stream responseStream = await data2.Content.ReadAsStreamAsync())
        {
          string jsonMessage = new StreamReader(responseStream).ReadToEnd();
          var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
          return new BaseResponse<T>(HttpStatusCode.OK) { ContentObj = JsonConvert.DeserializeObject(jsonMessage, typeof(T), settings), Headers = data2.Headers };
        }
      }
      catch (Exception ex)
      {
       if(Debugger.IsAttached) Console.WriteLine(ex.Message);
      }
      return new BaseResponse<T>(HttpStatusCode.InternalServerError);
    }


    public async Task<BaseResponse<int>> PostWithReturnAsync(string requestUri, HttpContent content)
    {

      var response = await PostAsync(requestUri, content);
      if (response.StatusCode == HttpStatusCode.InternalServerError)
      {
        using (Stream responseStream = await response.Content.ReadAsStreamAsync()) {
          var message = GetError(new StreamReader(responseStream).ReadToEnd());
          return new BaseResponse<int>(response.StatusCode, null) { Error = new Exception(message) };
         }
      }
        
      using (Stream responseStream = await response.Content.ReadAsStreamAsync())
      {
        string jsonMessage = new StreamReader(responseStream).ReadToEnd();
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var result = (AddResponse)JsonConvert.DeserializeObject(jsonMessage, typeof(AddResponse),settings);
        if (!string.IsNullOrEmpty(result.Message))
        {
          return new BaseResponse<int>(HttpStatusCode.BadRequest, null) { Content = result.Message };
        }
        else
        {
        return new BaseResponse<int>(HttpStatusCode.OK, null){Headers = response.Headers, ContentObj = result.id};
        }
      }

    }

    public async Task<BaseResponse<ProjectMailResponse>> PutWithReturnObjectAsync(string requestUri, HttpContent content)
    {

      var response = await PutAsync(requestUri, content);
      if (response.StatusCode == HttpStatusCode.InternalServerError)
      {
        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
        {
          var message = GetError(new StreamReader(responseStream).ReadToEnd());
          return new BaseResponse<ProjectMailResponse>(response.StatusCode, null) { Error = new Exception(message) };
        }
      }

      using (Stream responseStream = await response.Content.ReadAsStreamAsync())
      {
        string jsonMessage = new StreamReader(responseStream).ReadToEnd();
        var result = (AddResponse)JsonConvert.DeserializeObject(jsonMessage, typeof(AddResponse));
        if (!string.IsNullOrEmpty(result.Message))
        {
          return new BaseResponse<ProjectMailResponse>(HttpStatusCode.BadRequest, null) { Content = result.Message };
        }
        else
        {
          return new BaseResponse<ProjectMailResponse>(HttpStatusCode.OK, null) { ContentObj = result.id };
        }
      }

    }

    public async Task<BaseResponse<bool>> PutWithReturnAsync(string requestUri, HttpContent content)
    {

      var response = await PutAsync(requestUri, content);
      if (response.StatusCode == HttpStatusCode.InternalServerError)
      {
        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
        {
          var message = GetError(new StreamReader(responseStream).ReadToEnd());
          return new BaseResponse<bool>(response.StatusCode, null) { Error = new Exception(message) };
        }
      }

      using (Stream responseStream = await response.Content.ReadAsStreamAsync())
      {
        string jsonMessage = new StreamReader(responseStream).ReadToEnd();
        var result = (AddResponse)JsonConvert.DeserializeObject(jsonMessage, typeof(AddResponse));
        if (!string.IsNullOrEmpty(result.Message))
        {
          return new BaseResponse<bool>(HttpStatusCode.BadRequest, null) { Content = result.Message };
        }
        else
        {
          return new BaseResponse<bool>(HttpStatusCode.OK, null) { ContentObj = result.id };
        }
      }

    }



    public string GetError(string content)
    {
      var error = (ErrorResponse) JsonConvert.DeserializeObject<ErrorResponse>(content);
      return error.Message;
    }

    public BaseResponse<T> Get<T>(string endpoint, Dictionary<string, string> paramsDictionary, RequestFormat format = RequestFormat.Json)
    {
        var data = GetAsync(endpoint).Result;
        if (!data.IsSuccessStatusCode) return new BaseResponse<T>(HttpStatusCode.InternalServerError);
        using (Stream responseStream = data.Content.ReadAsStreamAsync().Result)
        {
          string jsonMessage = new StreamReader(responseStream).ReadToEnd();
          return new BaseResponse<T>(HttpStatusCode.OK) { ContentObj = JsonConvert.DeserializeObject(jsonMessage, typeof(T)) };
        }
    }
  }




}
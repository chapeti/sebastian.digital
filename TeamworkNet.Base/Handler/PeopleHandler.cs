// ==========================================================
// File: TeamWorkNet.Base.CalendarHandler.cs
// Created: 08.04.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Model;
using System.Linq;
using TeamWorkNet.Base.Response;

namespace TeamWorkNet.Base.Handler
{
  public class PeopleHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    ///   Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public PeopleHandler(TeamWorkClient client)
    {
      _client = client;
    }


    /// <summary>
    ///   Return all Peoples
    /// </summary>

    /// <returns></returns>
    public async Task<PeopleResponse> GetAllPeople(int page)
    {

      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<PeopleResponse>("people.json" + "?page=" + page + "&pageSize=5000", null);

        if (data.StatusCode == HttpStatusCode.OK)
        {
           var response =  (PeopleResponse)data.ContentObj;
           response.Pages = int.Parse(data.Headers.GetValues("X-Pages").First());
           response.Page = int.Parse(data.Headers.GetValues("X-Page").First());
           response.TotalRecords = int.Parse(data.Headers.GetValues("X-Records").First());
          return response;
        }

        return new PeopleResponse() { People = null, STATUS = "ERROR" };
     }

    }

    public async Task<PersonResponse> GetPerson(int personID)
    {

      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<PersonResponse>("/people/" + personID  + ".json", null);

        if (data.StatusCode == HttpStatusCode.OK)
        {
          var response = (PersonResponse)data.ContentObj;
          return response;
        }

        return new PersonResponse() { Person = null, STATUS = "ERROR" };
      }

    }

    public async Task<BaseResponse<int>> AddPerson(Person company)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(company);
        return await client.PostWithReturnAsync("/people.json", new StringContent("{\"person\": " + post + "}", Encoding.UTF8));
      }
    }

    public async Task<TodoItemsResponse> GetTasksForPerson(int id)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<TodoItemsResponse>("tasks.json" + "?responsible-party-id=" + id, null);

        if (data.StatusCode == HttpStatusCode.OK)
        {
          var response = (TodoItemsResponse)data.ContentObj;
          response.Pages = int.Parse(data.Headers.GetValues("X-Pages").First());
          response.Page = int.Parse(data.Headers.GetValues("X-Page").First());
          response.TotalRecords = int.Parse(data.Headers.GetValues("X-Records").First());
          return response;
        }

        return new TodoItemsResponse() { TodoLists = null, STATUS = "ERROR" };
      }

   }

  }
}
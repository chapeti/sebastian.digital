// ==========================================================
// File: TeamWorkNet.Base.CalendarHandler.cs
// Created: 08.04.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Model;
using System.Linq;

namespace TeamWorkNet.Base.Handler
{
  public class CalendarHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    ///   Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public CalendarHandler(TeamWorkClient client)
    {
      _client = client;
    }


    /// <summary>
    ///   Return all Calendar Events for APIKey
    /// http://developer.teamwork.com/events#create_event
    /// </summary>
    /// <param name="deleted"></param>
    /// <param name="dtFrom"></param>
    /// <param name="dtUntil"></param>
    /// <param name="updatedafter"></param>
    /// <param name="typeId"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<EventsResponse> GetAllEvents(bool deleted, DateTime dtFrom, DateTime dtUntil, DateTime updatedafter, int typeId, int page)
    {

      string start = dtFrom.ToString("yyyyMMdd");
      string end = dtUntil.ToString("yyyyMMdd");
      string updated = updatedafter.ToString("yyyyMMddHHmmss");
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<EventsResponse>("calendarevents.json?startdate=" + start + 
                                                                            "&endDate=" + end + 
                                                                            "&showDeleted=" + deleted + 
                                                                            "&updatedAfterDate=" + updated + 
                                                                            "&page=" + page, null);

        if (data.StatusCode == HttpStatusCode.OK)
        {
           var response =  (EventsResponse)data.ContentObj;
           response.Pages = int.Parse(data.Headers.GetValues("X-Pages").First());
           response.Page = int.Parse(data.Headers.GetValues("X-Page").First());
           response.TotalRecords = int.Parse(data.Headers.GetValues("X-Records").First());
          return response;
        }

        return new EventsResponse() { Events = null, STATUS = "ERROR" };
     }

    }



    /// <summary>
    ///   Return specific Calendar Events for APIKey
    /// http://developer.teamwork.com/events#create_event
    /// </summary>
    /// <param name="eventId"></param>
    public async Task<EventResponse> GetEvents(int eventId)
    {

      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<EventResponse>("calendarevents/" + eventId + ".json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (EventResponse)data.ContentObj;
      }

      return new EventResponse() { Event = null, STATUS = "ERROR" };
    }

    /// <summary>
    /// Add event to user´s calendar
    /// http://developer.teamwork.com/events#create_event
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<int> AddCalendarEntry(EventCreate data)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
          string post = JsonConvert.SerializeObject(data,settings);
          var response = await client.PostWithReturnAsync("calendarevents.json", new StringContent("{\"event\":" + post + "}", Encoding.UTF8));
          if (response.StatusCode == HttpStatusCode.OK)
          {
            var id = response.ContentObj;
            int returnval;

            if (int.TryParse(id.ToString(), out returnval)) return returnval;
          }
        }
        return -1;
      }
      catch (Exception)
      {
        return -1;
      }

    }


    /// <summary>
    /// Add event to user´s calendar
    /// http://developer.teamwork.com/events#create_event
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async void UpdateCalendarEntry(Event data)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
          var post = JsonConvert.SerializeObject(data,settings);
          await client.PutAsync("calendarevents/" + data.id + ".json", new StringContent("{\"event\":" + post + "}", Encoding.UTF8));
        }
      }
      catch (Exception)
      {
          // ignored
      }
    }

     /// <summary>
     /// Delete a Calendar Entry
    /// http://developer.teamwork.com/events#create_event
     /// </summary>
     /// <param name="id"></param>
    public async void DeleteEntry(int id)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        await client.DeleteAsync("/calendarevents/" + id + ".json");
      }
    }


  }
}
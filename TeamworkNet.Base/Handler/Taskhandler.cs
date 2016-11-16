// ==========================================================
// File: TeamWorkNet.Portable.ProjectHandler.cs
// Created: 14.02.2015
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
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{
  /// <summary>
  /// Handler for Projects
  /// </summary>
  public class TaskHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public TaskHandler(TeamWorkClient client)
    {
      _client = client;
    }

    /// <summary>
    /// Delete a Task
    /// </summary>
    /// <param name="taskid"></param>
    /// <returns></returns>
    public async Task<bool> DeleteTask(int taskid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.DeleteAsync("/tasks/" + taskid + ".json");
      }
      return false;
    }

    /// <summary>
    /// Delete a Task
    /// </summary>
    /// <param name="taskid"></param>
    /// <returns></returns>
    public async Task<bool>CompleteTask(int taskid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.PutAsync("/tasks/" + taskid + "/complete.json",null);
      }
      return false;
    }

    /// <summary>
    /// Delete a Task
    /// </summary>
    /// <param name="taskid"></param>
    /// <returns></returns>
    public async Task<bool> UnCompleteTask(int taskid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.PutAsync("/tasks/" + taskid + "/uncomplete.json", null);
      }
      return false;
    }



    /// <summary>
    ///   Returns all tags the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<TodoItemResponse> GetTagAsync(int tag_id)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<TodoItemResponse>("/tasks/" + tag_id + ".json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (TodoItemResponse)data.ContentObj;
      }
      return null;
    }



    /// <summary>
    ///   Add a new Tag
    /// </summary>
    /// <returns></returns>
    public async Task<bool> UpdateTask(TodoItem theTag,bool notify)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          theTag.notify = notify;
          var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
          string post = JsonConvert.SerializeObject(theTag, settings);
          await client.PutAsync("/tasks/" + theTag.id + ".json", new StringContent("{\"todo-item\": " + post + "}", Encoding.UTF8));
          return true;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }


  }
}
// ==========================================================
// File: TeamWorkNet.Portable.ProjectHandler.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;
using TeamWorkNetBase.Base.Generic;

namespace TeamWorkNet.Base.Handler
{
  /// <summary>
  /// Handler for Projects
  /// </summary>
  public class TodoListHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public TodoListHandler(TeamWorkClient client)
    {
      _client = client;
    }


    /// <summary>
    ///   Add a Tasklist to the given project
    ///   http://developer.teamwork.com/tasklists#create_list
    /// </summary>
    /// <param name="list"></param>
    /// <param name="projectId">Project ID (int)</param>
    /// <returns>Milestone ID</returns>
    public async Task<TodoList> AddTodoList(TodoList list, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(list);
        var newList = await client.PostWithReturnAsync("/projects/" + projectId + "/todo_lists.json", new StringContent("{\"todo-list\": " + post + "}", Encoding.UTF8));

        var id = newList.Headers.First(p => p.Key == "id");
        if (id.Value != null)
        {
          var listresponse =
            await
              client.GetAsync<TaskListResponse>("/todo_lists/" + int.Parse((string)id.Value.First()) + ".json", null,
                RequestFormat.Json);
          if (listresponse != null && listresponse.ContentObj != null)
          {
            var response = (TaskListResponse) listresponse.ContentObj;
            return response.TodoList;
          }                                                                          
        }
        return null;
      }
     }

    /// <summary>
    ///   Add a Tasklist to the given project
    ///   http://developer.teamwork.com/tasklists#create_list
    /// </summary>
    /// <param name="list"></param>
    /// <param name="projectId">Project ID (int)</param>
    /// <returns>Milestone ID</returns>
    public async Task<int> AddTodoListReturnOnlyID(TodoList list, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(list);
        var newList = await client.PostWithReturnAsync("/projects/" + projectId + "/todo_lists.json", new StringContent("{\"todo-list\": " + post + "}", Encoding.UTF8));

        var id = newList.Headers.First(p => p.Key == "id");
        if (id.Value != null)
        {
          return int.Parse((string) id.Value.First());
        }
        return -1;
      }
    }


    /// <summary>
    ///   Add a Task to the tasklist
    ///   http://developer.teamwork.com/todolistitems#add_a_task
    /// </summary>
    /// <param name="todo">The Task</param>
    /// <param name="taskListId">Tasklist to add the task to</param>
    /// <returns>Milestone ID</returns>
    public async Task<TodoItem> AddTodoItem(TodoItem todo, int taskListId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
        string post = JsonConvert.SerializeObject(todo, settings);
        var newList = await client.PostWithReturnAsync("/tasklists/" + taskListId + "/tasks.json", new StringContent("{\"todo-item\": " + post + "}", Encoding.UTF8));

        var id = newList.Headers.First(p => p.Key == "id");
        if (id.Value != null)
        {
          var listresponse =
            await
              client.GetAsync<TaskResponse>("/tasks/" + int.Parse((string)id.Value.First()) + ".json", null,
                RequestFormat.Json);
          if (listresponse != null && listresponse.ContentObj != null)
          {
            var response = (TaskResponse)listresponse.ContentObj;
            return response.TodoItem;
          }
        }
        return null;
      } 
    }

   
  }
}
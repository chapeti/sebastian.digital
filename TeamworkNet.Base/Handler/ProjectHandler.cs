// ==========================================================
// File: TeamWorkNet.Portable.ProjectHandler.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamWorkNet.Base.Response;
using TeamWorkNet.Model;
using TeamWorkNetBase.Base.Generic;
using TeamWorkNetBase.Generic;

namespace TeamWorkNet.Base.Handler
{
  /// <summary>
  /// Handler for Projects
  /// </summary>
  public class ProjectHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public ProjectHandler(TeamWorkClient client)
    {
      _client = client;
    }


    /// <summary>
    ///   Returns all projects the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<ProjectsResponse> GetAllProjectsAsync(bool useDefaultParser = false)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var requestString = "projects.json";
        if (useDefaultParser) requestString += "?useNativeParser=true";
        var data = await client.GetAsync<ProjectsResponse>(requestString, null);
        if (data.StatusCode == HttpStatusCode.OK) return (ProjectsResponse)data.ContentObj;
      }
      return null;
    }


    /// <summary>
    ///   Returns all projects the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<ProjectsResponse> GetAllRecentProjectsAsync(DateTime updatedSince)
    {
      string updatedafter = updatedSince.ToString("yyyyMMddhhmmss");

      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<ProjectsResponse>("projects.json?updatedAfterDate=" + updatedafter, null);
        if (data.StatusCode == HttpStatusCode.OK) return (ProjectsResponse)data.ContentObj;
      }
      return null;
    }



    /// <summary>
    ///   Return all persons of a given project
    ///   http://developer.teamwork.com/people
    /// </summary>
    /// <param name="projectID">Project ID (int)</param>
    /// <returns>Person List</returns>
    public async Task<PeopleResponse> GetPeople(int projectID)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          var data = await client.GetAsync<PeopleResponse>("/projects/" + projectID + "/people.json", null);
          if (data.StatusCode == HttpStatusCode.OK) return (PeopleResponse)data.ContentObj;
        }
        return new PeopleResponse { STATUS = "ERROR", People = null };
      }
      catch (Exception ex)
      {
        return new PeopleResponse { STATUS = "ERROR:" + ex.Message, People = null };
      }
    }


    /// <summary>
    ///   Return all persons of a given project
    ///   http://developer.teamwork.com/people
    /// </summary>
    /// <param name="projectID">Project ID (int)</param>
    /// <param name="personID"></param>
    /// <returns>Person List</returns>
    public async Task<PeopleResponse> GetPeople(int projectID, int personID)
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          var data = await client.GetAsync<PeopleResponse>("/projects/" + projectID + "/people/" + personID + ".json", null);
          if (data.StatusCode == HttpStatusCode.OK) return (PeopleResponse)data.ContentObj;
        }
        return new PeopleResponse { STATUS = "ERROR", People = null };
      }
      catch (Exception ex)
      {
        return new PeopleResponse { STATUS = "ERROR:" + ex.Message, People = null };
      }
    }


    /// <summary>
    ///   Returns a single Project, returns null if the user can not access the project
    /// </summary>
    /// <param name="projectID">the project id</param>
    /// <param name="includePeople">include people on project</param>
    /// <param name="includeTaskLists">include task lists</param>
    /// <param name="includeMilestones">include all milestones</param>
    /// <param name="useDefaultParser"></param>
    /// <returns></returns>
    public async Task<ProjectResponse> GetProject(int projectID, bool includePeople, bool includeTaskLists, bool includeMilestones = true, bool useDefaultParser = false)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var requestString = "/projects/" + projectID + ".json";
        if (useDefaultParser) requestString += "?useNativeParser=true";
        var data =
          await
            client.GetAsync<ProjectResponse>(requestString, null);
        if (data.StatusCode == HttpStatusCode.OK)
        {
          var prj = (ProjectResponse)data.ContentObj;

          if (includeTaskLists)
          {
            var taskRequestString = "/projects/" + projectID + "/todo_lists.json?nestSubTasks=yes&" + includePeople;
            if (useDefaultParser) taskRequestString += "&useNativeParser=true";

            var tasks = await client.GetAsync<TaskListsResponse>(taskRequestString, null);
            if (tasks.StatusCode == HttpStatusCode.OK)
            {
              var tasklist = (TaskListsResponse)tasks.ContentObj;
              prj.project.Tasklists = tasklist.TodoLists;
            }
          }

          if (includePeople)
          {
            var tasks = await client.GetAsync<PeopleResponse>(
                  "/projects/" + projectID + "/people.json", null);
            if (tasks.StatusCode == HttpStatusCode.OK)
            {
              var tasklist = (PeopleResponse)tasks.ContentObj;
              prj.project.People = tasklist.People;
            }
          }

          if (includeMilestones)
          {
            var tasks = await client.GetAsync<MileStonesResponse>(
                  "/projects/" + projectID + "/milestones.json?find=all&getProgress=true", null);
            if (tasks.StatusCode == HttpStatusCode.OK)
            {
              var tasklist = (MileStonesResponse)tasks.ContentObj;
              prj.project.Milestones = tasklist.Milestones.ToList();
            }
          }



          return new ProjectResponse { STATUS = "OK", project = prj.project };
        }
      }
      return null;
    }


    public async Task<StatsResponse> GetProjectStatus(int projectID)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<StatsResponse>("/projects/" + projectID + "/stats.json", null);
        if (data.StatusCode == HttpStatusCode.OK)
        {
          var prj = (StatsResponse)data.ContentObj;

          return new StatsResponse { Status = "OK", Stats = prj.Stats };
        }
      }
      return null;
    }











    /// <summary>
    ///   Return all Milestones of a given project
    ///   http://developer.teamwork.com/milestones
    /// </summary>
    /// <param name="projectid">Project ID (int)</param>
    /// <param name="type">Milestone Search Type</param>
    /// <returns>Milestone List</returns>
    public async Task<MileStonesResponse> GetProjectMilestones(int projectid, MilestoneFindType type)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data =
          await
            client.GetAsync<MileStonesResponse>(
              "/projects/" + projectid + "milestones.json?getProgress=true&find=" + type, null);
        if (data.StatusCode == HttpStatusCode.OK) return (MileStonesResponse)data.ContentObj;
      }
      return null;
    }

    /// <summary>
    ///   Returns all Companies within the given Project
    ///   Details: http://developer.teamwork.com/companies
    /// </summary>
    /// <param name="projectid"></param>
    /// <returns></returns>
    public async Task<CompaniesResponse> GetProjectCompanies(int projectid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<CompaniesResponse>("/projects/" + projectid + "/companies.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (CompaniesResponse)data.ContentObj;
      }
      return null;
    }

    /// <summary>
    ///   Returns all Roles of a project together with the persons in the roles
    ///   Details: http://developer.teamwork.com/projectroles
    /// </summary>
    /// <param name="projectid"></param>
    /// <returns></returns>
    public async Task<RoleResponse> GetProjectRoles(int projectid)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<RoleResponse>("/projects/" + projectid + "/projectroles.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (RoleResponse)data.ContentObj;
      }
      return null;
    }



    /// <summary>
    ///   Add a Milestone to the given project
    ///   http://developer.teamwork.com/milestones#create_a_single_m
    /// </summary>
    /// <param name="milestone">Milestone ID (int)</param>
    /// <param name="projectId">Project ID (int)</param>
    /// <returns>Milestone ID</returns>
    public async Task<BaseResponse<int>> AddMilestone(Milestone milestone, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(milestone);
        return
          await
            client.PostWithReturnAsync("/projects/" + projectId + "/milestones.json",
              new StringContent("{\"milestone\": " + post + "}", Encoding.UTF8));
      }
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
            var response = (TaskListResponse)listresponse.ContentObj;
            return response.TodoList;
          }
        }
        return null;
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
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
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


    /// <summary>
    ///   Add a message to the project
    ///   http://developer.teamwork.com/messages#create_a_message
    /// </summary>
    /// <param name="message">The Task</param>
    /// <param name="projectID">Tasklist to add the task to</param>
    /// <returns>Milestone ID</returns>
    public async Task<TodoItem> AddMessage(Post message, int projectID)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(message);
        var newList = await client.PostWithReturnAsync("/projects/" + projectID + "/posts.json", new StringContent("{\"post\": " + post + "}", Encoding.UTF8));

        var id = newList.Headers.First(p => p.Key == "id");
        if (id.Value != null)
        {
          var listresponse =
            await
              client.GetAsync<PostResponse>("/posts/" + int.Parse((string)id.Value.First()) + ".json", null,
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

    /// <summary>
    ///   Add a message to the project
    ///   http://developer.teamwork.com/messages#create_a_message
    /// </summary>
    /// <param name="message">The Task</param>
    /// <param name="projectID">Tasklist to add the task to</param>
    /// <returns>Milestone ID</returns>
    public async Task<bool> AddMessageNoCallback(NewPost message, int projectID)
    {

      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          string post = JsonConvert.SerializeObject(message);
          await client.PostWithReturnAsync("/projects/" + projectID + "/posts.json", new StringContent("{\"post\": " + post + "}", Encoding.UTF8));
          return true;
        }
      }
      catch (Exception)
      {
        return false;
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
          return int.Parse((string)id.Value.First());
        }
        return -1;
      }
    }


    /// <summary>
    ///   Create a new Project
    ///   http://developer.teamwork.com/projects
    /// </summary>
    /// <param name="project">New Project Object</param>
    /// <returns>Project ID</returns>
    public async Task<BaseResponse<int>> AddProject(Newproject project)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(project);
        return
          await
            client.PostWithReturnAsync("projects.json", new StringContent("{\"project\": " + post + "}", Encoding.UTF8));
      }
    }

    /// <summary>
    ///   Update details of given Project
    ///   http://developer.teamwork.com/projects
    /// </summary>
    /// <param name="project">Project Object</param>
    /// <returns>Project ID</returns>
    public async Task<BaseResponse<bool>> UpdateProject(Project project)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(project);
        return
          await
            client.PutWithReturnAsync("/projects/" + project.id + ".json",
              new StringContent("{\"project\": " + post + "}", Encoding.UTF8));
      }
    }


    /// <summary>
    ///   Update details of given Project
    ///   http://developer.teamwork.com/projects
    /// </summary>
    /// <param name="code">code eg: code@tasks.teamwork.com</param>
    /// <param name="projectId"></param>
    /// <returns>Project ID</returns>
    public async Task<BaseResponse<ProjectMailResponse>> SetProjectEmailAddressCode(string code, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        return
          await
            client.PutWithReturnObjectAsync("/projects/" + projectId + "/emailaddress.json",
              new StringContent("{\"emailaddress\": {\"code\":\"" + code + "\"}", Encoding.UTF8));
      }
    }

    /// <summary>
    ///   Delete given Project
    ///   http://developer.teamwork.com/projects
    /// </summary>
    /// <param name="project">Project Object</param>
    /// <returns>true/false</returns>
    public async Task<bool> DeleteProject(Project project)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        await client.DeleteAsync("/projects/" + project.id + ".json");
        return true;
      }
    }

    /// <summary>
    ///   Deprecated, should not be used!
    /// </summary>
    /// <returns></returns>
    public ProjectsResponse GetAllProjects(bool useDefaultParser = false)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var requestString = "projects.json";
        if (useDefaultParser) requestString += "?useDefaultParser=true";
        var data = client.Get<ProjectsResponse>(requestString, null);
        if (data.StatusCode == HttpStatusCode.OK) return (ProjectsResponse)data.ContentObj;
      }
      return null;
    }
  }
}
// ==========================================================
// File: TeamWorkNet.Portable.MilestoneHandler.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Purpose:
// Handles Milestones Add/Edit/Delete
//
// History:
// 14.02.2015 TCA - Created
// 15.05.2015 TCA - Fixed a bug in Edit
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
using TeamWorkNetBase.Generic;

namespace TeamWorkNet.Base.Handler
{
  public class MilestoneHandler : IDisposable
  {
    private readonly TeamWorkClient _client;

    public MilestoneHandler(TeamWorkClient client)
    {
      _client = client;
    }

    /// <summary>
    ///   Returns all projects the user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<MileStonesResponse> GetAllMilstones(MilestoneFindType type)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<MileStonesResponse>("milestones.json?find=" + type + "&getProgress=true", null);


        if (data.StatusCode == HttpStatusCode.OK)
        {
          var response = (MileStonesResponse)data.ContentObj;
          response.Etag = data.Headers.GetValues("ETag").First();
          response.LastUpdate = DateTime.Parse(data.Headers.GetValues("X-lastUpdated").First());
          response.Pages = int.Parse(data.Headers.GetValues("X-Pages").First());
          response.Page = int.Parse(data.Headers.GetValues("X-Page").First());
          response.TotalRecords = int.Parse(data.Headers.GetValues("X-Records").First());
          return response;
        }
        return null;
      }
    }

    /// <summary>
    ///   Returns a single milestone, returns null if the user can not access the project
    /// </summary>
    /// <param name="milestoneID">the project id</param>
    /// <param name="includeTasks">include people on project</param>
    /// <returns></returns>
    public async Task<MileStoneResponse> GetMilestone(int milestoneID, bool includeTasks)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data =
          await
            client.GetAsync<MileStoneResponse>("/milestones/" + milestoneID + ".json?showTaskLists=" + includeTasks,
              null);
        return (MileStoneResponse)data.ContentObj;
      }
    }

    /// <summary>
    ///   Mark the specifiec Milestone as completed
    /// </summary>
    /// <param name="milestoneID"></param>
    /// <returns></returns>
    public async Task<bool> MarkCommplete(int milestoneID)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.PutAsync("/milestones/" + milestoneID + "/complete.json", null);
      }
      return false;
    }

    /// <summary>
    ///   Mark the specifiec Milestone as Uncomplete
    /// </summary>
    /// <param name="milestoneID"></param>
    /// <returns></returns>
    public async Task<bool> MarkUnCommplete(int milestoneID)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var response = await client.PutAsync("/milestones/" + milestoneID + "/uncomplete.json", null);
      }
      return false;
    }

    /// <summary>
    ///   Adds a milestone to the given project
    /// </summary>
    /// <param name="milestone"></param>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public async Task<bool> AddMilestone(Milestone milestone, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(milestone);
        var response =
          await client.PostAsync("/projects/" + projectId + "/milestones.json", new StringContent("{\"milestone\":" + post + "}", Encoding.UTF8));
      }
      return false;
    }


    public async Task<bool> UpdateMilestone(Milestone milestone)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(milestone);
        var response =
          await client.PutAsync("/milestones/" + milestone.id + ".json", new StringContent("{\"milestone\":" + post + "}", Encoding.UTF8));
      }
      return false;
    }

    public async Task<bool> DeleteMilestone(int project)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        string post = JsonConvert.SerializeObject(project);
        var response = await client.DeleteAsync("/milestones/" + project + ".json");
      }
      return false;
    }

    /// <summary>
    ///   Deprecated, should not be used!
    /// </summary>
    /// <returns></returns>
    public ProjectsResponse GetAllProjects()
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = client.Get<MileStonesResponse>("milestones.json", null);
      }
      return null;
    }

    public void Dispose()
    {
      Dispose();
    }
  }
}
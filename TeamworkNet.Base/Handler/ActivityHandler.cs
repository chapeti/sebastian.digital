// ==========================================================
// File: TeamWorkNet.Portable.ActivityHandler.cs
// Created: 25.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System.Net;
using System.Threading.Tasks;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{
  /// <summary>
  /// Handler for Activities
  /// </summary>
  public class ActivityHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    /// Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public ActivityHandler(TeamWorkClient client)
    {
      _client = client;
    }



    /// <summary>
    /// Returns the list of latest activities across all projects
    /// </summary>
    /// <param name="maxItems">default 60 max 200</param>
    /// <param name="onlyStarred"></param>
    /// <returns>List of Activities</returns>
    public async Task<ActivityResponse> GetLatestActivities(int maxItems, bool onlyStarred)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var starred = onlyStarred == false ? "no" : "yes";
        var data = await client.GetAsync<ActivityResponse>("latestActivity.json?maxItems=" + maxItems + "&onlyStarred=" + starred, null);
        if (data.StatusCode == HttpStatusCode.OK) return (ActivityResponse)data.ContentObj;
        return new ActivityResponse() {STATUS = data.Content};
      }
    }


    /// <summary>
    /// Returns the list of latest activities for a given project
    /// </summary>
    /// <param name="maxItems">default 60 max 200</param>
    /// <param name="onlyStarred"></param>
    /// <param name="projectId"></param>
    /// <returns>List of Activities</returns>
    public async Task<ActivityResponse> GetLatestActivities(int maxItems, bool onlyStarred, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.GetAsync<ActivityResponse>("/projects/" + projectId + "/latestActivity.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (ActivityResponse)data.ContentObj;
        return new ActivityResponse() { STATUS = data.Content };
      }
    }


    /// <summary>
    /// Delete the given entry
    /// </summary>
    /// <returns>List of Activities</returns>
    public async Task<bool> DeleteActivity(int activityId, int projectId)
    {
      using (var client = new AuthorizedHttpClient(_client))
      {
        var data = await client.DeleteAsync("/projects/" + projectId + "/latestActivity.json");
        if (data.StatusCode == HttpStatusCode.OK) return true;
        return false;
      }
    }

  }
}
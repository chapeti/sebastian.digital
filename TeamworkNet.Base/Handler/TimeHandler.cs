// ==========================================================
// File: TeamWorkNet.Portable.TimeHandler.cs
// Created: 01.04.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using System.Net;
using System.Threading.Tasks;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Handler
{
  public class TimeHandler
  {
    private readonly TeamWorkClient _client;

    /// <summary>
    ///   Constructor for Project Handler
    /// </summary>
    /// <param name="client">TeamworkClient (Init needed!)</param>
    public TimeHandler(TeamWorkClient client)
    {
      _client = client;
    }

    /// <summary>
    ///   Return time totals for a project or tasklist
    ///   http://developer.teamwork.com/timetracking#time_totals
    /// </summary>
    /// <param name="projectID">Project ID (int)</param>
    /// <param name="userId"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="fromTime"></param>
    /// <param name="toTime"></param>
    /// <returns>Person List</returns>
    public async Task<TimeTotalsResponse> GetTotals_Project(int projectID,int userId = 0, 
                                                            DateTime? fromDate = null,
                                                            DateTime? toDate = null,
                                                            string fromTime = "",
                                                            string toTime = "")
    {
      try
      {
        using (var client = new AuthorizedHttpClient(_client))
        {
          var data = await client.GetAsync<TimeTotalsResponse>("/projects/" + projectID + "/time/total.json", null);
          if (data.StatusCode == HttpStatusCode.OK) return (TimeTotalsResponse) data.ContentObj;
        }
        return new TimeTotalsResponse {STATUS = "ERROR", Projects = null};
      }
      catch (Exception ex)
      {
        return new TimeTotalsResponse {STATUS = "ERROR:" + ex.Message, Projects = null};
      }
    }
  }
}
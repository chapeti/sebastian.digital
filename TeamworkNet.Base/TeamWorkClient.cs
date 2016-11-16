// ==========================================================
// File: TeamWorkNet.Base.TeamWorkClient.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base
{
  public class TeamWorkClient
  {
    public string InstallName { get; set; }
    private string baseUrl;
    public string BaseUrl
    {
      get
      {
        if (string.IsNullOrEmpty(baseUrl)) throw new Exception("You need to Initialize the client first");
        return baseUrl;
      }
      private set { baseUrl = value; }
    }

    private string apiKey;
    public string APiKey
    {
      get
      {
        if (string.IsNullOrEmpty(apiKey)) throw new Exception("You need to Initialize the client first");
        return apiKey;
      }
      private set
      {
        if (value == null) throw new ArgumentNullException("APiKey");
        apiKey = value;
      }
    }


    /// <summary>
    /// Initialize the Client and set basic parameters
    /// </summary>
    /// <param name="apiKey">Your API Key see http://http://developer.teamwork.com/introduction first</param>
    /// <param name="installationName">Your Installation Name</param>
    /// <param name="domain">If you´re not using "teamwork.com" tell your domain here</param>
    public void Init(string apiKey, string installationName, string domain = "teamwork.com")
    {
      InstallName = installationName;
      BaseUrl = "https://" + installationName + ".teamwork.com/";
      APiKey = apiKey;
    }

    /// <summary>
    /// Initialize the Client and set basic parameters
    /// </summary>
    /// <param name="apiKey">Your API Key see http://http://developer.teamwork.com/introduction first</param>
    /// <param name="installation">Your Installation Name</param>
    public void Init(string apiKey, string installation)
    {
      InstallName = installation;
      BaseUrl = installation;
      APiKey = apiKey;
    }
  }
}
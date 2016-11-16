// ==========================================================
// File: TeamWorkNet.Base.TasksResponse.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using Newtonsoft.Json;
using TeamWorkNet.Base.Model;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class TagResponse
  {
    [JsonProperty("tag")]
    public TodoItem Tag { get; set; }

    [JsonProperty("STATUS")]
    public string STATUS { get; set; }
  }

  

}
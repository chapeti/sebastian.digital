// ==========================================================
// File: TeamWorkNet.Base.ProjectsResponse.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System.Collections.Generic;
using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class ProjectsResponse
  {
    public string STATUS { get; set; }
    public List<Project> projects { get; set; }
  }
}
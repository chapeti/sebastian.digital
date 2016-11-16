// ==========================================================
// File: TeamWorkNet.Base.ProjectsResponse.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using TeamWorkNet.Model;

namespace TeamWorkNet.Base.Response
{
  public class ProjectResponse
  {
    public string STATUS { get; set; }
    public Project project { get; set; }
  }
}
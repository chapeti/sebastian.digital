#region FileHeader
// ==========================================================
// File: TeamWorkNet.Base.TeamWorkHTTPRequest.cs
// Last Mod:      23.05.2016
// Created:        17.05.2016
// Created By:   Tim cadenbach
//  
// Copyright (C) 2016 Digital Crew Limited
// History:
//  17.05.2016 - Created
//  ==========================================================
#endregion
namespace TeamWorkNetBase.Base.Generic
{
  public enum RequestFormat
  {
    /// <summary>
    ///   XML format
    /// </summary>
    Xml,

    /// <summary>
    ///   JSON format
    /// </summary>
    Json,

    /// <summary>
    ///   Protocol Buffer format
    /// </summary>
    ProtoBuf,

    /// <summary>
    ///   Comma-separated format
    /// </summary>
    Csv,

    CustomBinary
  }
}
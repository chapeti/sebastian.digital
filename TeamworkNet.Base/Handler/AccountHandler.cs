// ==========================================================
// File: TeamWorkNet.Portable.AccountHandler.cs
// Created: 14.02.2015
// Created By: Tim cadenbach
// 
// Copyright (C) 2015 Tim Cadenbach
// License: Apache License 2.0
// ==========================================================

using System;
using System.Net;
using System.Threading.Tasks;
using TeamWorkNet.Base.Response;

namespace TeamWorkNet.Base.Handler
{
  /// <summary>
  /// 
  /// </summary>
  public class AccountHandler : BaseHandler
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public AccountHandler(TeamWorkClient client) : base(client) {}

    /// <summary>
    /// Returns your account details if you´re an Administrator
    /// </summary>
    /// <returns></returns>
    public async Task<AccountResponse> GetAccountDetails()
    {
      using (var client = new AuthorizedHttpClient(Client))
      {
        var data = await client.GetAsync<AccountResponse>("account.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (AccountResponse)data.ContentObj;
      }
      return new AccountResponse(){Status = "Error", Account = null};
    }


    public async Task<PersonResponse> GetMyDetails()
    {
      using (var client = new AuthorizedHttpClient(Client))
      {
        var data = await client.GetAsync<PersonResponse>("me.json", null).ConfigureAwait(continueOnCapturedContext:false);
        if (data.StatusCode == HttpStatusCode.OK) return (PersonResponse)data.ContentObj;
      }
      return new PersonResponse() { Person = null };
    } 



    /// <summary>
    /// Returns authentication details and infos of the current user
    /// </summary>
    /// <returns></returns>
    public async Task<AccountResponse> GetAuthenticationtDetails()
    {
      using (var client = new AuthorizedHttpClient(Client))
      {
        client.BaseAddress = new Uri("http://authenticate.teamworkpm.net/");
        var data = await client.GetAsync<AccountResponse>("authenticate.json", null);
        if (data.StatusCode == HttpStatusCode.OK) return (AccountResponse)data.ContentObj;
      }
      return new AccountResponse() { Status = "Error", Account = null };
    }
  }
}
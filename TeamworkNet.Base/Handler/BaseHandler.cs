using System;

namespace TeamWorkNet.Base.Handler
{
  public class BaseHandler: IDisposable
  {
    public TeamWorkClient Client { get; private set; }

    public BaseHandler(TeamWorkClient client)
    {
      Client = client;
    }

    void IDisposable.Dispose()
    {
      
    }
  }
}

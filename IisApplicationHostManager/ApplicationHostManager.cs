using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Web.Administration;

namespace IisApplicationHostManager
{
  public class ApplicationHostManager
  {
    private readonly string _applicationHostPath;

    public static ApplicationHostManager IIS = new ApplicationHostManager(Path.Combine(Environment.ExpandEnvironmentVariables("%windir%\\system32\\inetsrv\\config"), "applicationhost.config"));
    public static ApplicationHostManager IISExpress = new ApplicationHostManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IISExpress", "config", "applicationhost.config"));

    public ApplicationHostManager(string applicationHostPath)
    {
      _applicationHostPath = applicationHostPath;
    }

    public IEnumerable<string> SiteNames
    {
      get
      {
        using (ServerManager manager = new ServerManager(_applicationHostPath))
        {
          return manager.Sites.Select(site => site.Name).ToList();
        }
      }
    }
 
    public bool Exists(string siteName)
    {
      using (ServerManager manager = new ServerManager(_applicationHostPath))
      {
        return manager.Sites.Any(s => s.Name == siteName);
      }
    }

    public void AddBinding(string siteName, Binding binding)
    {
      AddBindings(siteName, new [] { binding });
    }

    public void AddBindings(string siteName, IEnumerable<Binding> bindings)
    {
      using (ServerManager manager = new ServerManager(_applicationHostPath))
      {
        Site site = manager.Sites.SingleOrDefault(s => s.Name == siteName);
        if (site == null)
        {
          throw new InvalidOperationException("Site does not exist: " + siteName);
        }

        bool configurationChanged = false;
        foreach (Binding binding in bindings)
        {
          bool bindingChanged = binding.AddTo(site.Bindings);
          if (bindingChanged)
          {
            configurationChanged = true;
          }
        }

        if (configurationChanged)
        {
          manager.CommitChanges();
        }
      }
    }
  }
}

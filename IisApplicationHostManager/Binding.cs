using System;
using System.Linq;

namespace IisApplicationHostManager
{
  public class Binding
  {
    private readonly string _protocol;
    private readonly int _port;
    private readonly string _hostName;

    private Binding(string protocol, int port, string hostName)
    {
      _protocol = protocol;
      _port = port;
      _hostName = hostName;
    }

    public static Binding HttpBinding(int port, string hostName)
    {
      return new Binding(Uri.UriSchemeHttp, port, hostName);
    }

    public static Binding HttpsBinding(int port, string hostName)
    {
      return new Binding(Uri.UriSchemeHttps, port, hostName);
    }

    internal bool AddTo(Microsoft.Web.Administration.BindingCollection collection)
    {
      string bindingInformation = "*:" + _port + ":" + _hostName;
      Microsoft.Web.Administration.Binding existingBinding = collection.SingleOrDefault(b => b.BindingInformation.Equals(bindingInformation, StringComparison.InvariantCultureIgnoreCase));
      bool changed = false;
      if (existingBinding != null)
      {
        if (existingBinding.Protocol != _protocol)
        {
          existingBinding.Protocol = _protocol;
          changed = true;
        }

        if (existingBinding.BindingInformation != bindingInformation)
        {
          existingBinding.BindingInformation = bindingInformation;
          changed = true;
        }
      }
      else
      {
        collection.Add(bindingInformation, _protocol);
        changed = true;
      }

      return changed;
    }
  }
}

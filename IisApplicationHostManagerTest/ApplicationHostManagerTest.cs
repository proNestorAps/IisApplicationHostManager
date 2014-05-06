using System.Linq;
using IisApplicationHostManager;
using NUnit.Framework;

namespace IisApplicationHostManagerTest
{
  [TestFixture]
  public class ApplicationHostManagerTest
  {
    [Test]
    public void Test()
    {
      //var sites = ApplicationHostManager.SiteNames.ToList();
      bool exists = ApplicationHostManager.IISExpress.Exists("RoomMate.Web");
    }

    [Test]
    public void AddBinding()
    {
      ApplicationHostManager.IISExpress.AddBinding("RoomMate.Web", Binding.HttpBinding(2136, "kabam.pndtest.com"));
    }
  }
}

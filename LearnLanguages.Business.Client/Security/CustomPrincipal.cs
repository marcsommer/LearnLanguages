using System;
using System.Net;

using Csla.Security;
using Csla.Serialization;

namespace LearnLanguages.Business.Security
{
  [Serializable]
  public class CustomPrincipal : CslaPrincipal
  {
    /// <summary>
    /// CustomPrincipal must have a default constructor for the data portal to serialize it with every data portal call.
    /// </summary>
    public CustomPrincipal()
      : base()
    {

    }
    private CustomPrincipal(CustomIdentity identity)
      : base(identity)
    { }

    public static void BeginLogin(string username, string clearUnsaltedPassword, Action<Exception> completed)
    {
      CustomIdentity.GetCustomIdentity(username, clearUnsaltedPassword, (s, r) =>
        {
          if (r.Error != null)
            Logout();
          else
            Csla.ApplicationContext.User = new CustomPrincipal(r.Object); //r.Object is CustomIdentity

          completed(r.Error);
        });
    }
#if !SILVERLIGHT
    public static void Login(string username, string clearUnsaltedPassword)
    {
      var identity = CustomIdentity.GetCustomIdentity(username, clearUnsaltedPassword); 
      //if credentials dont pass, identity will not be IsAuthenticated.
      Csla.ApplicationContext.User = new CustomPrincipal(identity);
    }
    public static void Load(string username)
    {
      var identity = CustomIdentity.GetCustomIdentity(username);
      Csla.ApplicationContext.User = new CustomPrincipal(identity);
    }
#endif
    public static void Logout()
    {
      Csla.ApplicationContext.User = new UnauthenticatedPrincipal();
    }
  }
}

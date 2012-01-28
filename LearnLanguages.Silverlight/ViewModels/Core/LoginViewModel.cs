using System.ComponentModel.Composition;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LoginViewModel))]
  [ViewModelMetadata("Login")]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class LoginViewModel : ViewModelBase
  {
    private string _Username;
    public string Username
    {
      get { return _Username; }
      set
      {
        if (value != _Username)
        {
          _Username = value;
          NotifyOfPropertyChange(() => Username);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    private string _Password;
    public string Password
    {
      get { return _Password; }
      set
      {
        if (value != _Password)
        {
          _Password = value;
          NotifyOfPropertyChange(() => Password);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }

    public void Login()
    {
      LoggingIn = true;
      CustomPrincipal.BeginLogin(Username, Password, (e) =>
        {
          if (e != null)
            throw e;
          Events.Publish.AuthenticationChanged();
          LoggingIn = false;
        });
    }
    public bool CanLogin
    {
      get
      {
        return (!string.IsNullOrEmpty(Username) && 
                !string.IsNullOrEmpty(Password) &&
                !LoggingIn);
      }
    }

    private bool _LoggingIn = false;
    public bool LoggingIn
    {
      get { return _LoggingIn; }
      set
      {
        if (value != _LoggingIn)
        {
          _LoggingIn = value;
          NotifyOfPropertyChange(() => LoggingIn);
          NotifyOfPropertyChange(() => CanLogin);
        }
      }
    }
  }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LearnLanguages.Business.Security;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  public class _LoginViewModel : ViewModelBase
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
      CustomPrincipal.BeginLogin(Username, Password, (e) =>
        {
          if (e != null)
            throw e;
          Services.EventAggregator.Publish(new Events.AuthenticationChangedEventMessage());
        });
    }
    public bool CanLogin
    {
      get
      {
        return (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
      }
    }
  }
}

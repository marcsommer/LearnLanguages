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
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AuthenticationStatusViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AuthenticationStatusViewModel : ViewModelBase, IHandle<Interfaces.IAuthenticationChangedEventMessage>, IDisposable
  {
    public AuthenticationStatusViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      //THIS INITIALIZES PROPERTIES INTERNALLY - IT DOES NOT _*PUBLISH*_ THE EVENT
      var message = new Events.AuthenticationChangedEventMessage();
      Handle(message);
    }

    private string _CurrentPrincipalName;
    public string CurrentPrincipalName
    {
      get { return _CurrentPrincipalName; }
      set
      {
        if (value != _CurrentPrincipalName)
        {
          _CurrentPrincipalName = value;
          NotifyOfPropertyChange(() => CurrentPrincipalName);
        }
      }
    }

    private bool _IsAuthenticated;
    public bool IsAuthenticated
    {
      get { return _IsAuthenticated; }
      set
      {
        if (value != _IsAuthenticated)
        {
          _IsAuthenticated = value;
          NotifyOfPropertyChange(() => IsAuthenticated);
        }
      }
    }

    private bool _IsInAdminRole;
    public bool IsInAdminRole
    {
      get { return _IsInAdminRole; }
      set
      {
        if (value != _IsInAdminRole)
        {
          _IsInAdminRole = value;
          NotifyOfPropertyChange(() => IsInAdminRole);
        }
      }
    }

    private bool _IsInUserRole;
    public bool IsInUserRole
    {
      get { return _IsInUserRole; }
      set
      {
        if (value != _IsInUserRole)
        {
          _IsInUserRole = value;
          NotifyOfPropertyChange(() => IsInUserRole);
        }
      }
    }

    public void Handle(Interfaces.IAuthenticationChangedEventMessage message)
    {
      CurrentPrincipalName = message.CurrentPrincipalName;
      IsAuthenticated = message.IsAuthenticated;
      IsInAdminRole = message.IsInRole(DataAccess.DalResources.RoleAdmin);
      IsInUserRole = message.IsInRole(DataAccess.DalResources.RoleUser);
    }

    public void Dispose()
    {
      Services.EventAggregator.Unsubscribe(this);
    }
  }
}

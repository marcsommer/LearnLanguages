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
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace LearnLanguages.Silverlight.ViewModels
{

  [Export(typeof(ShellViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ShellViewModel : ViewModelBase,
                                IHandle<Interfaces.IPartSatisfiedEventMessage>
  {
    public ShellViewModel()
      : base()
    {
      ReloadNavigationPanel();
      Services.Container.SatisfyImportsOnce(this);
    }

    /// <summary>
    /// Reloads the navigation panel depending on the current user's roles.
    /// </summary>
    public void ReloadNavigationPanel()
    {
      NavigationPanel = Services.Container.GetExportedValue<NavigationPanelViewModel>();
    }

    [Import]
    public Interfaces.INavigationController NavigationController { get; set; }

    //private LoginViewModel _Login;
    //public LoginViewModel Login
    //{
    //  get { return _Login; }
    //  set
    //  {
    //    if (value != _Login)
    //    {
    //      _Login = value;
    //      NotifyOfPropertyChange(() => Login);
    //    }
    //  }
    //}

    //private AuthenticationStatusViewModel _AuthenticationStatus;
    //public AuthenticationStatusViewModel AuthenticationStatus
    //{
    //  get { return _AuthenticationStatus; }
    //  set
    //  {
    //    if (value != _AuthenticationStatus)
    //    {
    //      _AuthenticationStatus = value;
    //      NotifyOfPropertyChange(() => AuthenticationStatus);
    //    }
    //  }
    //}

    private ViewModelBase _Main;
    public ViewModelBase Main
    {
      get { return _Main; }
      set
      {
        if (value != _Main)
        {
          _Main = value;
          NotifyOfPropertyChange(() => Main);
        }
      }
    }

    private NavigationPanelViewModel _NavigationPanel;
    public NavigationPanelViewModel NavigationPanel
    {
      get { return _NavigationPanel; }
      set
      {
        if (value != _NavigationPanel)
        {
          _NavigationPanel = value;
          NotifyOfPropertyChange(() => NavigationPanel);
        }
      }
    }

    //private bool ShellModelSatisfied = false;
    //private bool NavigationControllerSatisfied = false;
    //private bool ICareAboutPartsSatisfiedMessages = true;

    public void Handle(Interfaces.IPartSatisfiedEventMessage message)
    {
      //  if (!ICareAboutPartsSatisfiedMessages)
      //    return;

      //  if (message.Part == GetCoreViewModelName())
      //    ShellModelSatisfied = true;
      //  else if (message.Part == "NavigationController")
      //    NavigationControllerSatisfied = true;

      //  if (ShellModelSatisfied && NavigationControllerSatisfied)
      //  {
      //    ICareAboutPartsSatisfiedMessages = false;
      //    //Services.EventAggregator.Publish(new Events.NavigationRequestedEventMessage("Login"));
      //  }
    }
  }
}


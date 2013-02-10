using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.Interfaces;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation.ViewModels;
using LearnLanguages.Silverlight.Pages;
using LearnLanguages.Common.EventMessages;
using LearnLanguages.Navigation.EventMessages;

namespace LearnLanguages.Silverlight.ViewModels
{

  //[Export(typeof(ShellViewModel))]
  [Export(typeof(IShellViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ShellViewModel : ViewModelBase, 
                                IShellViewModel,
                                IHandle<EnableNavigationRequestedEventMessage>,
                                IHandle<DisableNavigationRequestedEventMessage>,
                                IHandle<NavigatingEventMessage>,
                                IHandle<NavigatedEventMessage>,
                                IHandle<NavigationFailedEventMessage>,
                                IHandle<IncrementApplicationBusyEventMessage>,
                                IHandle<DecrementApplicationBusyEventMessage>

  {
    public ShellViewModel()
      : base()
    {
      Title = AppResources.DefaultAppTitle;
      //NavigationPanelViewModel = Services.Container.GetExportedValue<NavigationPanelViewModel>();
      Services.EventAggregator.Subscribe(this);
    }

    ///// <summary>
    ///// Reloads the navigation panel depending on the current user's roles.
    ///// </summary>
    //public void ReloadNavigationPanel()
    //{
    //  NavigationPanel = Services.Container.GetExportedValue<NavigationPanelViewModel>();
    //}

    //[Import]
    //public INavigationController NavigationController { get; set; }

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

    private IViewModelBase _Main;
    public IViewModelBase Main
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

    private ThinkingPanelViewModel _ThinkingPanel;
    [Import]
    public ThinkingPanelViewModel ThinkingPanel
    {
      get { return _ThinkingPanel; }
      set
      {
        if (value != _ThinkingPanel)
        {
          _ThinkingPanel = value;
          NotifyOfPropertyChange(() => ThinkingPanel);
        }
      }
    }

    [Import]
    public NavigationPanelViewModel _NavigationPanel;
    public INavigationPanelViewModel NavigationPanelViewModel
    {
      get { return _NavigationPanel; }
      set
      {
        if (value != _NavigationPanel)
        {
          _NavigationPanel = (NavigationPanelViewModel)value;
          NotifyOfPropertyChange(() => NavigationPanelViewModel);
        }
      }
    }

    private bool _NavPanelIsEnabled = true;
    public bool NavPanelIsEnabled
    {
      get { return _NavPanelIsEnabled; }
      set
      {
        if (value != _NavPanelIsEnabled)
        {
          _NavPanelIsEnabled = value;
          NotifyOfPropertyChange(() => NavPanelIsEnabled);
        }
      }
    }
    //private bool ShellModelSatisfied = false;
    //private bool NavigationControllerSatisfied = false;
    //private bool ICareAboutPartsSatisfiedMessages = true;

    public override void OnImportsSatisfied()
    {
      IPage loginPage = Services.Container.GetExportedValue<LoginPage>();
      //Navigation.Navigator.Ton.RegisterPage(loginPage);
      Navigation.Navigator.Ton.NavigateTo(loginPage);
    }

    private string _Title;
    public string Title
    {
      get { return _Title; }
      set
      {
        if (value != _Title)
        {
          _Title = value;
          NotifyOfPropertyChange(() => Title);
        }
      }
    }

    public void Handle(EnableNavigationRequestedEventMessage message)
    {
      NavigationPanelViewModel.IsEnabled = true;
      NavPanelIsEnabled = true;
    }

    public void Handle(DisableNavigationRequestedEventMessage message)
    {
      NavigationPanelViewModel.IsEnabled = false;
      NavPanelIsEnabled = false;
    }

    public void Handle(NavigatingEventMessage message)
    {
      NavPanelIsEnabled = false;
    }

    public void Handle(NavigatedEventMessage message)
    {
      NavPanelIsEnabled = true;
    }

    public void Handle(NavigationFailedEventMessage message)
    {
      NavPanelIsEnabled = true;
    }

    public void Handle(IncrementApplicationBusyEventMessage message)
    {
      IncrementApplicationBusy();
    }

    public void Handle(DecrementApplicationBusyEventMessage message)
    {
      DecrementApplicationBusy();
    }

    #region private int ApplicationBusyReferenceCount

    private object __ApplicationBusyReferenceCountLock = new object();
    private int _ApplicationBusyReferenceCount;
    private int ApplicationBusyReferenceCount
    {
      get
      {
        lock (__ApplicationBusyReferenceCountLock)
        {
          return _ApplicationBusyReferenceCount;
        }
      }
      set
      {
        lock (__ApplicationBusyReferenceCountLock)
        {
          _ApplicationBusyReferenceCount = value;
        }
      }
    }

    #endregion
    private void IncrementApplicationBusy()
    {
      ApplicationBusyReferenceCount++;
      IsBusy = ApplicationBusyReferenceCount > 0;
    }

    private void DecrementApplicationBusy()
    {
      ApplicationBusyReferenceCount--;
      IsBusy = ApplicationBusyReferenceCount > 0;
    }
  }
}


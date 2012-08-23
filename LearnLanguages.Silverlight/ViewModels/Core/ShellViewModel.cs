using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.Interfaces;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{

  [Export(typeof(ShellViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class ShellViewModel : ViewModelBase
  {
    public ShellViewModel()
      : base()
    {
      ReloadNavigationPanel();
      Services.Container.SatisfyImportsOnce(this);
      Title = AppResources.DefaultAppTitle;
      ThinkingPanel = Services.Container.GetExportedValue<ThinkingPanelViewModel>();
    }

    /// <summary>
    /// Reloads the navigation panel depending on the current user's roles.
    /// </summary>
    public void ReloadNavigationPanel()
    {
      NavigationPanel = Services.Container.GetExportedValue<NavigationPanelViewModel>();
    }

    [Import]
    public INavigationController NavigationController { get; set; }

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

    
    public override void OnImportsSatisfied()
    {
      //
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
  }
}


using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(NavigationPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationPanelViewModel : Conductor<ViewModelBase>.Collection.AllActive,
                                          IHandle<Interfaces.IAuthenticationChangedEventMessage>,
                                          IViewModelBase
  {
    public NavigationPanelViewModel()
    {
      //Services.Container.SatisfyImportsOnce(this);
      Services.EventAggregator.Subscribe(this);
      PopulateButtons();
    }

    #region Populate Buttons
    
    private void PopulateButtons()
    {
      Items.Clear();
      var user = Csla.ApplicationContext.User.Identity;
      if (!user.IsAuthenticated)
        AddUnauthenticatedButtons();
      else
        AddAuthenticatedButtons(user);
    }

    private void AddUnauthenticatedButtons()
    {
      var loginNavButtonViewModel = Services.Container.GetExportedValue<LoginNavigationButtonViewModel>();
      Items.Add(loginNavButtonViewModel);
    }

    private void AddAuthenticatedButtons(System.Security.Principal.IIdentity user)
    {
      var logoutNavButtonViewModel = Services.Container.GetExportedValue<LogoutNavigationButtonViewModel>();
      Items.Add(logoutNavButtonViewModel);
      var authStatusNavButtonViewModel = Services.Container.GetExportedValue<AuthenticationStatusNavigationButtonViewModel>();
      Items.Add(authStatusNavButtonViewModel);
      var addUserNavButtonViewModel = Services.Container.GetExportedValue<AddUserNavigationButtonViewModel>();
      Items.Add(addUserNavButtonViewModel);
      var addPhraseNavButtonViewModel = Services.Container.GetExportedValue<AddPhraseNavigationButtonViewModel>();
      Items.Add(addPhraseNavButtonViewModel);
    }

    #endregion

    public void Handle(Interfaces.IAuthenticationChangedEventMessage message)
    {
      PopulateButtons();
    }
    
    public void OnImportsSatisfied()
    {
      var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationPanelViewModel));
      Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }


    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
  }
}

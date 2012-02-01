using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(NavigationPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationPanelViewModel : Conductor<ViewModelBase>.Collection.AllActive,
                                          //IHandle<Interfaces.IAuthenticationChangedEventMessage>,
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
        AddAuthenticatedButtons((CustomIdentity)user);
    }

    private void AddUnauthenticatedButtons()
    {
      var loginNavButtonViewModel = Services.Container.GetExportedValue<LoginNavigationButtonViewModel>();
      Items.Add(loginNavButtonViewModel);
    }

    private void AddAuthenticatedButtons(CustomIdentity user)
    {
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleAdmin))
        AddAdminButtons();
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleUser))
        AddUserButtons();

      
      var logoutNavButtonViewModel = Services.Container.GetExportedValue<LogoutNavigationButtonViewModel>();
      Items.Add(logoutNavButtonViewModel);
    }

    private void AddAdminButtons()
    {
      var addUserNavButtonViewModel = Services.Container.GetExportedValue<AddUserNavigationButtonViewModel>();
      Items.Add(addUserNavButtonViewModel);
     
      var authStatusNavButtonViewModel = Services.Container.GetExportedValue<AuthenticationStatusNavigationButtonViewModel>();
      Items.Add(authStatusNavButtonViewModel);
    }

    private void AddUserButtons()
    {
      var addLanguage = Services.Container.GetExportedValue<AddLanguageNavigationButtonViewModel>();
      Items.Add(addLanguage);
      var viewLanguages = Services.Container.GetExportedValue<ViewLanguagesNavigationButtonViewModel>();
      Items.Add(viewLanguages);
      var viewTranslations = Services.Container.GetExportedValue<ViewTranslationsNavigationButtonViewModel>();
      Items.Add(viewTranslations);
      var viewPhrases = Services.Container.GetExportedValue<ViewPhrasesNavigationButtonViewModel>();
      Items.Add(viewPhrases);
      var addTranslation = Services.Container.GetExportedValue<AddTranslationNavigationButtonViewModel>();
      Items.Add(addTranslation);
      var addPhrase = Services.Container.GetExportedValue<AddPhraseNavigationButtonViewModel>();
      Items.Add(addPhrase);
      var study = Services.Container.GetExportedValue<StudyNavigationButtonViewModel>();
      Items.Add(study);
    }

    #endregion

    //public void Handle(Interfaces.IAuthenticationChangedEventMessage message)
    //{
    //  PopulateButtons();
    //}
    
    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationPanelViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
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

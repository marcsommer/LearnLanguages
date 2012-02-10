using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(NavigationPanelViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationPanelViewModel : Conductor<IViewModelBase>.Collection.AllActive,
                                          IHandle<EventMessages.AuthenticationChangedEventMessage>,
                                          IViewModelBase
  {
    public NavigationPanelViewModel()
    {
      Services.EventAggregator.Subscribe(this);
      PopulatePanel();
    }

    #region Populate Panel
    
    private void PopulatePanel()
    {
      Items.Clear();
      var user = Csla.ApplicationContext.User.Identity;
      if (!user.IsAuthenticated)
        AddUnauthenticatedItems();
      else
        AddAuthenticatedItems((CustomIdentity)user);

      //sort the items
      var tmp = new List<IViewModelBase>(Items);
      Comparison<IViewModelBase> comparison = (a, b) =>
        {
          //put Logout last always
          if (a is LogoutNavigationButtonViewModel)
            return 1;
          else if (b is LogoutNavigationButtonViewModel)
            return -1;
          else if (a.GetType().Name[0] < b.GetType().Name[0])
            return -1;
          else
            return 1;
        };
      tmp.Sort(comparison);
      Items.Clear();
      for (int i = 0; i < tmp.Count; i++)
      {
        Items.Insert(i, tmp[i]);
      }

      
    }

    private void AddAuthenticatedItems(CustomIdentity user)
    {
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleAdmin))
        AddAdminItems();
      if (Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleUser))
        AddUserItems();

      var logoutNavButtonViewModel = Services.Container.GetExportedValue<LogoutNavigationButtonViewModel>();
      Items.Add(logoutNavButtonViewModel);
    }
    private void AddUnauthenticatedItems()
    {
      var loginNavButtonViewModel = Services.Container.GetExportedValue<LoginNavigationButtonViewModel>();
      Items.Add(loginNavButtonViewModel);
    }
    
    private void AddAdminItems()
    {
      //INITIALIZE NAVIGATION ITSELF
      var adminNavigationSet = Services.Container.GetExportedValue<NavigationSetViewModel>();
      var adminTitle = Services.Container.GetExportedValue<AdminNavigationSetTitleViewModel>();
      adminNavigationSet.TitleControl = adminTitle;

      //CREATE BUTTONS TO GO INTO NAVIGATION SET
      var authStatusNavButtonViewModel = 
        Services.Container.GetExportedValue<AuthenticationStatusNavigationButtonViewModel>();
      var addUserNavButtonViewModel = Services.Container.GetExportedValue<AddUserNavigationButtonViewModel>();

      //INSERT BUTTONS IN LIST ***IN THE ORDER IN WHICH YOU WANT THEM TO APPEAR***
      List<ViewModelBase> orderedAdminButtons = new List<ViewModelBase>();
      orderedAdminButtons.Insert(0, authStatusNavButtonViewModel);
      orderedAdminButtons.Insert(1, addUserNavButtonViewModel);

      //ADD ORDERED LIST OF NAV BUTTONS TO NAVIGATION SET
      adminNavigationSet.AddControls(orderedAdminButtons);

      //ADD THE SET TO THE ITEMS
      Items.Add(adminNavigationSet);
    }
    private void AddUserItems()
    {
      AddStudyNavigationSet();
      AddEditNavigationSet();
      AddTestNavigationSet();
      AddReviewNavigationSet();
      AddProgressNavigationSet();
      AddChugNavigationSet();

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
      var iWantToLearn = Services.Container.GetExportedValue<IWantToLearnNavigationButtonViewModel>();
      Items.Add(iWantToLearn);
      var addSong = Services.Container.GetExportedValue<AddSongNavigationButtonViewModel>();
      Items.Add(addSong);
    }

    private void AddStudyNavigationSet()
    {
      //throw new NotImplementedException();
    }

    private void AddEditNavigationSet()
    {
      throw new NotImplementedException();
    }

    private void AddTestNavigationSet()
    {
      throw new NotImplementedException();
    }

    private void AddReviewNavigationSet()
    {
      throw new NotImplementedException();
    }

    private void AddProgressNavigationSet()
    {
      throw new NotImplementedException();
    }

    private void AddChugNavigationSet()
    {
      throw new NotImplementedException();
    }

    #endregion

    public void Handle(EventMessages.AuthenticationChangedEventMessage message)
    {
      PopulatePanel();
    }
    
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

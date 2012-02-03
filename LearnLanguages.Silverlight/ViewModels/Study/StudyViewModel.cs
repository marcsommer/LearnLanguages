using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyViewModel : ViewModelBase,
                                IHandle<Navigation.EventMessages.NavigatedEventMessage>
  {
    public StudyViewModel()
    {
      //_AskViewModel = Services.Container.GetExportedValue<AskDoYouKnowThisViewModel>();
      if (_StudyPartner == null)
        Services.Container.SatisfyImportsOnce(this);

      if (_StudyPartner == null)
        throw new Common.Exceptions.PartNotSatisfiedException("StudyPartner");

      Services.EventAggregator.Subscribe(this);
    }

    private void HandleStudyCompleted(object sender, System.EventArgs e)
    {
      Navigation.Publish.NavigationRequest<StudyCompletedViewModel>(AppResources.BaseAddress);
    }

    //private AskDoYouKnowThisViewModel _AskViewModel;
    //public AskDoYouKnowThisViewModel AskViewModel
    //{
    //  get { return _AskViewModel; }
    //  set
    //  {
    //    if (value != _AskViewModel)
    //    {
    //      _AskViewModel = value;
    //      NotifyOfPropertyChange(() => AskViewModel);
    //    }
    //  }
    //}
    private IStudyPartner _StudyPartner;
    [Import]
    public IStudyPartner StudyPartner
    {
      get { return _StudyPartner; }
      set
      {
        if (value != _StudyPartner)
        {
          UnhookFrom(_StudyPartner);
          _StudyPartner = value;
          HookInto(_StudyPartner);
          NotifyOfPropertyChange(() => StudyPartner);
        }
      }
    }

    private void HookInto(IStudyPartner studyPartner)
    {
      studyPartner.StudyCompleted += HandleStudyCompleted;
    }

    private void UnhookFrom(IStudyPartner studyPartner)
    {
      if (studyPartner != null)
        studyPartner.StudyCompleted -= HandleStudyCompleted;
    }

    public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    {
      //WE ARE LISTENING FOR A MESSAGE THAT SAYS WE WERE SUCCESSFULLY NAVIGATED TO (SHELLVIEW.MAIN == STUDYVIEWMODEL)
      //SO WE ONLY CARE ABOUT NAVIGATED EVENT MESSAGES ABOUT OUR CORE AS DESTINATION.
      if (message.NavigationInfo.ViewModelCoreNoSpaces != ViewModelBase.GetCoreViewModelName(typeof(StudyViewModel)))
        return;

      //WE HAVE BEEN SUCCESSFULLY NAVIGATED TO.

      //SINCE THIS IS A NONSHARED COMPOSABLE PART, WE ONLY CARE ABOUT NAVIGATED MESSAGE ONCE, SO UNSUBSCRIBE
      Services.EventAggregator.Unsubscribe(this);

      //START STUDYING
      StudyPartner.Study(Services.EventAggregator);
    }
  }
}

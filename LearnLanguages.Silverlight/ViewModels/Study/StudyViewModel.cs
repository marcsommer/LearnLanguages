using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyViewModel : ViewModelBase
  {
    public StudyViewModel()
    {
      _AskViewModel = Services.Container.GetExportedValue<AskDoYouKnowThisViewModel>();
    }
    private AskDoYouKnowThisViewModel _AskViewModel;
    public AskDoYouKnowThisViewModel AskViewModel
    {
      get { return _AskViewModel; }
      set
      {
        if (value != _AskViewModel)
        {
          _AskViewModel = value;
          NotifyOfPropertyChange(() => AskViewModel);
        }
      }
    }
  }
}

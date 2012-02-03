using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(StudyCompletedViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyCompletedViewModel : ViewModelBase
  {
    private string _StudyCompletedMessage = AppResources.StudyCompletedMessage;
    public string StudyCompletedMessage
    {
      get { return _StudyCompletedMessage; }
      set
      {
        if (value != _StudyCompletedMessage)
        {
          _StudyCompletedMessage = value;
          NotifyOfPropertyChange(() => StudyCompletedMessage);
        }
      }
    }
  }
}

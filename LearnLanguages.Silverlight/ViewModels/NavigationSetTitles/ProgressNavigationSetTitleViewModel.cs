using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ProgressNavigationSetTitleViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ProgressNavigationSetTitleViewModel : NavigationSetTitleViewModelBase
  {
    public override string GetLabelText()
    {
      return ViewViewModelResources.TitleLabelTextProgress;
    }
  }
}

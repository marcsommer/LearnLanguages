using LearnLanguages.Business.Security;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Common;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AccountSettingsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
  public class AccountSettingsViewModel : PageViewModelBase
  {
    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsAccountSettingsPage;
      Title = ViewViewModelResources.TitleAccountSettingsPage;
      Description = ViewViewModelResources.DescriptionAccountSettingsPage;
      ToolTip = ViewViewModelResources.ToolTipAccountSettingsPage;
    }
  }
}

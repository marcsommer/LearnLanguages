using System;
using System.Net;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewPhrasesItemViewModel : ViewModelBase<PhraseEdit, PhraseDto>
  {
    public ViewPhrasesItemViewModel()
    {
      _Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      HookInto(_Languages);
    }

    private void HookInto(LanguageSelectorViewModel _Languages)
    {
      _Languages.SelectedItemChanged += HandleLanguageChanged;
    }

    void HandleLanguageChanged(object sender, EventArgs e)
    {
      //sender is the new LanguageEdit
      Model.Language = (LanguageEdit)sender;
    }

    private LanguageSelectorViewModel _Languages;
    public LanguageSelectorViewModel Languages
    {
      get { return _Languages; }
      set
      {
        if (value != _Languages)
        {
          _Languages = value;
          NotifyOfPropertyChange(() => Languages);
        }
      }
    }
    
  }
}

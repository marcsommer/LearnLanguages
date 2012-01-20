using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(PhraseEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class PhraseEditViewModel : ViewModelBase
  {
    private PhraseEdit _PhraseEdit;
    public PhraseEdit PhraseEdit
    {
      get { return _PhraseEdit; }
      set
      {
        if (value != _PhraseEdit)
        {
          _PhraseEdit = value;
          NotifyOfPropertyChange(() => PhraseEdit);
        }
      }
    }

    public string PhraseEditText
    {
      get { return PhraseEdit.Text; }
      set
      {
        if (value != PhraseEditText)
        {
          PhraseEditText = value;
          NotifyOfPropertyChange(() => PhraseEditText);
        }
      }
    }
  }
}

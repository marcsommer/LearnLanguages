using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Windows.Controls;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewPhrasesViewModel : ViewModelBase<PhraseList, PhraseEdit, PhraseDto>
  {
    public ViewPhrasesViewModel()
    {
      PhraseList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          Model = r.Object;
        });
    }

  }
}

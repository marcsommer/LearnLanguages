using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Windows.Controls;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Events;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class ViewPhrasesViewModel : ViewModelBase<PhraseList, PhraseEdit, PhraseDto>
  public class ViewPhrasesViewModel : Conductor<ViewModelBase>.Collection.AllActive, 
                                      Interfaces.IViewModelBase
  {
    public ViewPhrasesViewModel()
    {
      PhraseList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var allPhrases = r.Object;
          foreach (var phraseEdit in allPhrases)
          {
            var itemViewModel = Services.Container.GetExportedValue<ViewPhrasesItemViewModel>();
            itemViewModel.Model = phraseEdit;
            Items.Add(itemViewModel);
          }
        });
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }

    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

    public void OnImportsSatisfied()
    {
      Publish.PartSatisfied("ViewPhrases");
    }
  }
}

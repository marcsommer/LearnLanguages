using System;
using System.Net;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Linq;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewPhrasesItemViewModel : ViewModelBase<PhraseEdit, PhraseDto>
  {
    #region Ctors and Init
    public ViewPhrasesItemViewModel()
    {
      _Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      HookInto(_Languages);
    }

    #endregion

    #region Properties

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

    #endregion

    #region Methods

    private void HookInto(LanguageSelectorViewModel _Languages)
    {
      //_Languages.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_Languages_PropertyChanged);
    }

    void _Languages_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      //System.Windows.MessageBox.Show("languages_property changed");
    }

    public override void SetModel(PhraseEdit model)
    {
      base.SetModel(model);
      if (model != null)
      {
        var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
        languageViewModel.Model = model.Language;
        Languages.SelectedItem = (from l in Languages.Items
                                  where l.Model.Id == model.LanguageId
                                  select l).FirstOrDefault();
        Languages.SelectedItem = languageViewModel;
      }
      else
      {
        Languages.SelectedItem = null;
      }
    }
    #endregion

  }
}

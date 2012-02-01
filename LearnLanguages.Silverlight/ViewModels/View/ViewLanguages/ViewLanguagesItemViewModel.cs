using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewLanguagesItemViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewLanguagesItemViewModel : ViewModelBase<LanguageEdit, LanguageDto>
  {
    private bool _IsChecked;
    public bool IsChecked
    {
      get { return _IsChecked; }
      set
      {
        if (value != _IsChecked)
        {
          _IsChecked = value;
          NotifyOfPropertyChange(() => IsChecked);
        }
      }
    }

    public override void Save()
    {
      try
      {
        Model.BeginSave((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          Model = (LanguageEdit)r.NewObject;
          NotifyOfPropertyChange(() => CanSave);
        });
      }
      catch (Csla.DataPortalException dpex)
      {
        var errorMsgLanguageTextAlreadyExists = 
          DataAccess.Exceptions.LanguageTextAlreadyExistsException.GetDefaultErrorMessage(Model.Text);
        if (dpex.Message.Contains(errorMsgLanguageTextAlreadyExists))
          System.Windows.MessageBox.Show(errorMsgLanguageTextAlreadyExists);
      }
    }
  }
}

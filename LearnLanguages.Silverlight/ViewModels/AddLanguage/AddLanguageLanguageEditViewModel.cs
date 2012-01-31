using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddLanguageLanguageEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageLanguageEditViewModel : LanguageEditViewModel
  {
    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddLanguageViewLanguageText; } }

    public override void Save()
    {
      Model.BeginSave((s, r) =>
       {
         try
         {
           if (r.Error != null)
             throw r.Error;
           Model = (LanguageEdit)r.NewObject;
           NotifyOfPropertyChange(() => CanSave);
         }
         catch (Csla.DataPortalException dpex)
         {
           var errorMsgLanguageTextAlreadyExists =
             DataAccess.Exceptions.LanguageTextAlreadyExistsException.GetDefaultErrorMessage(Model.Text);
           if (dpex.Message.Contains(errorMsgLanguageTextAlreadyExists))
             System.Windows.MessageBox.Show(errorMsgLanguageTextAlreadyExists);
         }
       });  
    }
  }
}

using System.Windows;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using System.Text.RegularExpressions;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddUserViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddUserViewModel : ViewModelBase
  {
    public AddUserViewModel()
    {
    }

    private string _NewUsername;
    public string NewUsername
    {
      get { return _NewUsername; }
      set
      {
        if (value != _NewUsername)
        {
          _NewUsername = value;
          NotifyOfPropertyChange(() => NewUsername);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _NewPassword;
    public string NewPassword
    {
      get { return _NewPassword; }
      set
      {
        if (value != _NewPassword)
        {
          _NewPassword = value;
          NotifyOfPropertyChange(() => NewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    private string _ConfirmNewPassword;
    public string ConfirmNewPassword
    {
      get { return _ConfirmNewPassword; }
      set
      {
        if (value != _ConfirmNewPassword)
        {
          _ConfirmNewPassword = value;
          NotifyOfPropertyChange(() => ConfirmNewPassword);
          NotifyOfPropertyChange(() => CanAddUser);
        }
      }
    }

    public bool CanAddUser
    {
      get
      {
        //PASSWORDS MATCH
        bool passwordsMatch = NewPassword == ConfirmNewPassword;

        //PASSWORD IS VALID
        bool newPasswordIsValid = Common.CommonHelper.PasswordIsValid(NewPassword);

        //USERNAME IS VALID
        bool newUsernameIsValid = Common.CommonHelper.UsernameIsValid(NewUsername);

        bool canAddUser = Csla.ApplicationContext.User.Identity.IsAuthenticated &&
                          Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleAdmin) &&
                          !string.IsNullOrEmpty(NewPassword) && 
                          !string.IsNullOrEmpty(NewUsername) &&
                          passwordsMatch &&
                          newUsernameIsValid;
        
        return canAddUser;
      }
    }
    public void AddUser()
    {
      MessageBox.Show("This function is not yet implemented.");
    }
  }
}

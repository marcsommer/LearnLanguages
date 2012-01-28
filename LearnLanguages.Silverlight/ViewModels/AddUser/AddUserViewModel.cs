using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using System.ComponentModel.Composition;

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
        return (NewPassword == ConfirmNewPassword &&
                !string.IsNullOrEmpty(NewPassword) && 
                !string.IsNullOrEmpty(NewUsername) &&
                Csla.ApplicationContext.User.Identity.IsAuthenticated &&
                Csla.ApplicationContext.User.IsInRole(DataAccess.DalResources.RoleAdmin));
      }
    }
    public void AddUser()
    {
      MessageBox.Show("This function is not yet implemented.");
    }
  }
}

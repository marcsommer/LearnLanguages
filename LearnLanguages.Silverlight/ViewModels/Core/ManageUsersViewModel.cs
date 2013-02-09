using System.Windows;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using System.Text.RegularExpressions;
using System;
using LearnLanguages.Silverlight.Common;
using LearnLanguages.Common;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ManageUsersViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ManageUsersViewModel : PageViewModelBase
  {
    public ManageUsersViewModel()
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
        bool newPasswordIsValid = CommonHelper.PasswordIsValid(NewPassword);

        //USERNAME IS VALID
        bool newUsernameIsValid = CommonHelper.UsernameIsValid(NewUsername);

        bool canAddUser = DataAccess.DalHelper.IsInRoleToAddUser() &&
                          passwordsMatch &&
                          newUsernameIsValid &&
                          newPasswordIsValid;
        
        return canAddUser;
      }
    }
    public async void AddUser()
    {
      #region Thinking
      var thinkingId = Guid.NewGuid();
      History.Events.ThinkingAboutTargetEvent.Publish(thinkingId);
      #endregion
      var criteria = new Csla.Security.UsernameCriteria(NewUsername, NewPassword);
      try
      {
        var result = await Business.NewUserCreator.CreateNewAsync(criteria);
        MessageBox.Show(string.Format(ViewViewModelResources.MessageNewUserAdded, NewUsername));
      }
      catch (Exception ex)
      {
        
      }
      finally
      {
        #region Thinked
        History.Events.ThinkedAboutTargetEvent.Publish(thinkingId);
        #endregion
      }
    }

    protected override void InitializePageViewModelPropertiesImpl()
    {
      Instructions = ViewViewModelResources.InstructionsManageUsersPage;
      Title = ViewViewModelResources.TitleManageUsersPage;
      Description = ViewViewModelResources.DescriptionManageUsersPage;
      ToolTip = ViewViewModelResources.ToolTipManageUsersPage;
    }
  }
}

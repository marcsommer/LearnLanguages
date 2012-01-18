using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.Common;

namespace LearnLanguages.DataAccess.Ef
{
  public class CustomIdentityDal : CustomIdentityDalBase
  {
    protected override bool? VerifyUserImpl(string username, string password)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from userData in ctx.ObjectContext.UserDatas
                      where userData.Username == username
                      select userData;

        if (results.Count() == 1)
        {
          var user = results.First();
          var authenticated = (SaltedHashedPassword.GetHashedPasswordValue(password, user.Salt) == user.SaltedHashedPasswordValue);
          return authenticated;
          //RETURNS SUCCESS IF VALIDATION IS AUTHENTICATED OR NOT.  DOES *NOT* THROW EXCEPTION
          //IF CREDENTIALS ARE INVALID.
        }
        else
        {
          if (results.Count() == 0)
            return false;  //FALSE BECAUSE USER NOT FOUND
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple users with the same username.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException, 
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }

    protected override UserDto GetUserImpl(string username)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = from userData in ctx.ObjectContext.UserDatas
                      where userData.Username == username
                      select userData;

        if (results.Count() == 1)
        {
          var userDto = EfHelper.ToDto(results.First());
          return userDto;
        }
        else if (results.Count() == 0)
        {
          return null;
        }
        else
        {
          //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
          //which means that we have multiple users with the same username.  this is very bad.
          var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                       DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
          throw new Exceptions.VeryBadException(errorMsg);
        }
      }
    }

    protected override ICollection<RoleDto> GetRolesImpl(string username)
    {
      //GET USER
      var userDto = GetUserImpl(username);
      if (username == null)
        throw new Exceptions.UsernameNotFoundException(username);
      ICollection<RoleDto> retRoles = null;
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //GET ROLES FOR THAT USER
        retRoles = (from roleData in ctx.ObjectContext.RoleDatas
                    where userDto.RoleIds.Contains(roleData.Id)
                    select EfHelper.ToDto(roleData)).ToList();
      }

      return retRoles;
    }
  }
}

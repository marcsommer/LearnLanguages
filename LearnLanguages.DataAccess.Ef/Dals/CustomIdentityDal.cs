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
      int maxTries = int.Parse(EfResources.MaxDeadlockAttempts);
      for (int i = 0; i < maxTries; i++)
      {
//#if DEBUG
//        //debug: I want to break it here if this is a retry, in which case i > 0
//        if (i > 0)
//          System.Diagnostics.Debugger.Break();
//#endif

        try
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
#if DEBUG
              System.Diagnostics.Debugger.Break();
#endif

              //RESULTS.COUNT IS NOT ONE OR ZERO.  EITHER IT'S NEGATIVE, WHICH WOULD BE FRAMEWORK ABSURD, OR ITS MORE THAN ONE,
              //WHICH MEANS THAT WE HAVE MULTIPLE USERS WITH THE SAME USERNAME.  THIS IS VERY BAD.
              var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                           DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
              throw new Exceptions.VeryBadException(errorMsg);
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is System.Data.EntityCommandExecutionException && 
              ex.InnerException is System.Data.SqlClient.SqlException &&
              ex.InnerException.Message.Contains("Rerun the transaction"))
          {
            //"Transaction (Process ID 55) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction."
            //DO NOTHING IF THE ERROR IS A DEADLOCK. WE HAVE THIS IN A FOR LOOP THAT WILL RETRY UP TO A MAX NUMBER OF ATTEMPTS
          }
          else
          {
            System.Diagnostics.Debugger.Break();
            //RETHROW THIS EXCEPTION
            throw;
          }
        }
      }

      //IF WE REACH THIS POINT, THEN WE HAVE TRIED OUR MAX TRIES AT BREAKING A SQL DEADLOCK.
#if DEBUG
      //if (retRoles == null)
      System.Diagnostics.Debugger.Break();
#endif

      //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
      //which means that we have multiple users with the same username.  this is very bad.
      var errorMsg2 = string.Format(DalResources.ErrorMsgVeryBadException,
                                   DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
      throw new Exceptions.VeryBadException(errorMsg2);


    }

    protected override ICollection<RoleDto> GetRolesImpl(string username)
    {
      try
      {

      //GET USER
      var userDto = GetUserImpl(username);
      if (username == null)
        throw new Exceptions.UsernameNotFoundException(username);
      ICollection<RoleDto> retRoles = new List<RoleDto>();
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        //GET ROLES FOR THAT USER
        var results = from roleData in ctx.ObjectContext.RoleDatas
                      where userDto.RoleIds.Contains(roleData.Id)
                      select roleData;

        foreach (var roleData in results)
        {
          var dto = EfHelper.ToDto(roleData);
          retRoles.Add(dto);
        }
      }

#if DEBUG
      if (retRoles == null)
        System.Diagnostics.Debugger.Break();
#endif
       
        return retRoles;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debugger.Break();
        
        throw;
      }
    }

    protected override UserDto AddUserImpl(string username, string password)
    {
      throw new NotImplementedException();
    }

    protected override bool? DeleteUserImpl(string username)
    {
      throw new NotImplementedException();
    }
  }
}

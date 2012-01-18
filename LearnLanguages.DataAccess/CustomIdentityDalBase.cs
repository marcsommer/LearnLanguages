using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every ICustomIdentityDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class CustomIdentityDalBase : ICustomIdentityDal
  {
    public Result<bool?> VerifyUser(string username, string password)
    {
      Result<bool?> retResult = Result<bool?>.Undefined(null);
      try
      {
        var verified = VerifyUserImpl(username, password);
        retResult = Result<bool?>.Success(verified);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.VerifyUserFailedException(ex, username);
        retResult = Result<bool?>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<UserDto> GetUser(string username)
    {
      Result<UserDto> retResult = Result<UserDto>.Undefined(null);
      try
      {
        var userDto = GetUserImpl(username);
        if (userDto == null)
          throw new Exceptions.UsernameNotFoundException(username);
        retResult = Result<UserDto>.Success(userDto);
      }
      catch (Exception ex)
      {
        var wrapperEx = new Exceptions.GetUserFailedException(ex, username);
        retResult = Result<UserDto>.FailureWithInfo(null, wrapperEx);
      }

      return retResult;
    }
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        var roles = GetRolesImpl(username);
        retResult = Result<ICollection<RoleDto>>.Success(roles);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetRolesFailedException(ex, username);
        retResult = Result<ICollection<RoleDto>>.FailureWithInfo(null, wrappedEx);
      }

      return retResult;
    }

    protected abstract bool? VerifyUserImpl(string username, string password);
    protected abstract UserDto GetUserImpl(string username);
    protected abstract ICollection<RoleDto> GetRolesImpl(string username);
  }
}

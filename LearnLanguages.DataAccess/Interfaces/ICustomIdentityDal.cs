using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ICustomIdentityDal
  {
    Result<bool?> VerifyUser(string username, string password);
    Result<UserDto> GetUser(string username);
    Result<ICollection<RoleDto>> GetRoles(string username);
    //TODO: ADD/DELETE USER
    //Result<UserDto> AddUser(string newUserName);
    //Result<bool?> DeleteUser(string username); 
  }
}

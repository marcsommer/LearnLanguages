using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Interfaces
{
  public interface ICustomIdentityDal
  {
    Result<bool?> VerifyUser(string username, string password);
    Result<UserDto> GetUser(string username);
    Result<ICollection<RoleDto>> GetRoles(string username);
  }
}

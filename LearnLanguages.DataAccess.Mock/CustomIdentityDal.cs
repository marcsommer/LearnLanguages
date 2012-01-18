using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.DataAccess.Mock
{
  public class CustomIdentityDal : ICustomIdentityDal
  {
    private Guid _TestValidUserId = new Guid("89991D3B-0435-4167-8691-455D3D5000BC");
    private string _TestValidUsername = "user";
    private static Guid _TestRoleId = new Guid("4E7DACEC-2EE7-4201-8657-694D51AA0487");
    private RoleDto _TestRole = new RoleDto()
    {
      Id = _TestRoleId,
      Text = DalResources.RoleAdmin
    };
    private string _TestValidPassword = "password";
    private string _TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    private int _TestSalt = -54623530;
    private string _TestInvalidUsername = "ImNotAValidUser";
    private string _TestInvalidPassword = "ImNotAValidPassword";

    public Result<bool?> VerifyUser(string username, string password)
    {
      return Result<bool?>.Success(username == _TestValidUsername && password == _TestValidPassword);
    }
    public Result<UserDto> GetUser(string username)
    {
      if (username != _TestValidUsername)
        throw new GeneralDataAccessException("GetUser(username) not found");
      UserDto dto = new UserDto()
      {
        Id = _TestValidUserId,
        Salt = _TestSalt,
        SaltedHashedPasswordValue = _TestSaltedHashedPassword,
        Username = _TestValidUsername
      };
      return Result<UserDto>.Success(dto);
    }
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        if (username != _TestValidUsername)
          throw new GeneralDataAccessException("GetUser(username) not found");

        var role = new RoleDto()
        {
          Id = SeedData.Instance.TestRoleId,
          Text = SeedData.Instance.TestRoleText
        };
        retResult = Result<ICollection<RoleDto>>.Success(new List<RoleDto>() { role });
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<RoleDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}

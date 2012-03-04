using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.DataAccess.Mock
{
  public class CustomIdentityDal : ICustomIdentityDal
  {
    //private Guid _TestValidUserId = new Guid("89991D3B-0435-4167-8691-455D3D5000BC");
    private Guid _TestValidUserId = SeedData.Ton.DefaultTestValidUserId;
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
      //if (username != _TestValidUsername)
      //  throw new GeneralDataAccessException("GetUser(username) not found");
      var results = from u in SeedData.Ton.Users
                    where u.Username == username
                    select u;

      if (results.Count() == 1)
        return Result<UserDto>.Success(results.First());
      else if (results.Count() == 0)
        throw new Exceptions.UsernameNotFoundException(username);
      else
        throw new Exceptions.VeryBadException();

      //UserDto dto = new UserDto()
      //{
      //  Id = SeedData.Ton.DefaultTestValidUserId,//_TestValidUserId,
      //  Salt = SeedData.Ton.TestSalt, //_TestSalt
      //  SaltedHashedPasswordValue = SeedData.Ton.TestSaltedHashedPassword,// _TestSaltedHashedPassword,
      //  Username = SeedData.Ton.TestValidUsername//_TestValidUsername
      //};
      //return Result<UserDto>.Success(dto);
    }
    public Result<ICollection<RoleDto>> GetRoles(string username)
    {
      Result<ICollection<RoleDto>> retResult = Result<ICollection<RoleDto>>.Undefined(null);
      try
      {
        var roleDtos = new List<RoleDto>();

        var userResults = (from user in SeedData.Ton.Users
                           where user.Username == username
                           select user);

        if (userResults.Count() == 1)
        {
          var roleIds = userResults.First().RoleIds;
          foreach (var roleId in roleIds)
          {
            var roleResults = (from role in SeedData.Ton.Roles
                               where role.Id == roleId
                               select role);
            if (roleResults.Count() == 1)
            {
              roleDtos.Add(roleResults.First());
            }
            else if (roleResults.Count() == 0)
              throw new Exceptions.IdNotFoundException(roleId);
            else
              throw new Exceptions.VeryBadException();
          }
        }
        else if (userResults.Count() == 0)
          throw new Exceptions.UsernameNotFoundException(username);
        else
          throw new Exceptions.VeryBadException();

        retResult = Result<ICollection<RoleDto>>.Success(roleDtos);
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<RoleDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}

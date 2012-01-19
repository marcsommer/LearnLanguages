using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LearnLanguages.DataAccess.Ef
{
  public static class EfHelper
  {
    public static RoleData ToData(RoleDto dto)
    {
      return new RoleData()
      {
        Id = dto.Id,
        Text = dto.Text,
      };
    }
    public static RoleDto ToDto(RoleData data)
    {
      return new RoleDto()
      {
        Id = data.Id,
        Text = data.Text
      };
    }

    //public static UserData ToData(UserDto dto, bool includeForeignEntities = true)
    //{
    //  var retUserData = new UserData()
    //  {
    //    Id = dto.Id,
    //    Username = dto.Username,
    //    Salt = dto.Salt,
    //    SaltedHashedPasswordValue = dto.SaltedHashedPasswordValue
    //  };

    //  if (includeForeignEntities)
    //  {
    //    using (var dalManager = DalFactory.GetDalManager())
    //    {
    //      //USER PHRASES
    //      var phraseDal = dalManager.GetProvider<IPhraseDal>();
    //      foreach (var phraseId in dto.PhraseIds)
    //      {
    //        var result = phraseDal.Fetch(phraseId);
    //        if (!result.IsSuccess || result.IsError)
    //          throw new Exceptions.FetchFailedException(result.Msg);

    //        //var phraseData = ToData(result.Obj);
    //        //retUserData.PhraseDatas.Add(phraseData);
    //        var phraseData = EfHelper.AddToContext(result.Obj, retUserData.
    //      }

    //      //USER ROLES
    //      var customIdentityDal = dalManager.GetProvider<ICustomIdentityDal>();
    //      foreach (var roleId in dto.RoleIds)
    //      {
    //        var result = customIdentityDal.GetRoles(dto.Username);
    //        if (!result.IsSuccess || result.IsError)
    //          throw new Exceptions.FetchFailedException(result.Msg);

    //        var roleDtos = result.Obj;
    //        foreach (var roleDto in roleDtos)
    //        {
    //          RoleData roleData = new RoleData()
    //          {
    //            Id = roleDto.Id,
    //            Text = roleDto.Text
    //          };
    //          retUserData.RoleDatas.Add(roleData);
    //        }
    //      }
    //    }
    //  }

    //  return retUserData;
    //}
    public static UserDto ToDto(UserData data)
    {
      //SCALARS
      var retUserDto = new UserDto()
      {
        Id = data.Id,
        Username = data.Username,
        Salt = data.Salt,
        SaltedHashedPasswordValue = data.SaltedHashedPasswordValue
      };

      //PHRASES
      retUserDto.PhraseIds = new List<Guid>();
      foreach (var phraseData in data.PhraseDatas)
      {
        retUserDto.PhraseIds.Add(phraseData.Id);
      }

      //ROLES
      retUserDto.RoleIds = new List<Guid>();
      foreach (var roleData in data.RoleDatas)
      {
        retUserDto.RoleIds.Add(roleData.Id);
      }

      return retUserDto;
    }

    public static PhraseDto ToDto(PhraseData data)
    {
      return new PhraseDto()
      {
        Id = data.Id,
        Text = data.Text,
        LanguageId = data.LanguageDataId,
        UserId = data.UserDataId,
        Username = data.UserDataReference.Value.Username
      };
    }
    /// <summary>
    /// Adds the phraseDto to the context, loading UserData and LanguageData into the newly
    /// created PhraseData.  Does NOT save changes to the context.
    /// </summary>
    public static PhraseData AddToContext(PhraseDto dto, LearnLanguagesContext context)
    {
      //only creates, does not add to phrasedatas
      var beforeCount = context.PhraseDatas.Count();
      var newPhraseData = context.PhraseDatas.CreateObject();
      var afterCount = context.PhraseDatas.Count();

      //assign properties
      newPhraseData.Text = dto.Text;
      newPhraseData.LanguageDataId = dto.LanguageId;
      newPhraseData.UserDataId = dto.UserId;

      context.PhraseDatas.AddObject(newPhraseData);

      return newPhraseData;
    }

    public static LanguageDto ToDto(LanguageData data)
    {
      return new LanguageDto()
      {
        Id = data.Id,
        Text = data.Text
      };
    }
    public static LanguageData ToData(LanguageDto dto)
    {
      return new LanguageData()
      {
        Id = dto.Id,
        Text = dto.Text
      };
    }

    public static string GetConnectionString()
    {
      return ConfigurationManager.ConnectionStrings[EfResources.LearnLanguagesConnectionStringKey].ConnectionString;
    }
  }
}

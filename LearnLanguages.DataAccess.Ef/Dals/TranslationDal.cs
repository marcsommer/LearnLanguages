using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using LearnLanguages.Business.Security;

namespace LearnLanguages.DataAccess.Ef
{
  public class TranslationDal : TranslationDalBase
  {
    protected override TranslationDto NewImpl(object criteria)
    {
      var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      string currentUsername = identity.Name;
      Guid currentUserId = identity.UserId;

      TranslationDto newTranslationDto = new TranslationDto()
      {
        Id = Guid.NewGuid(),
        UserId = currentUserId,
        Username = currentUsername,
      };

      return newTranslationDto;
    }

    protected override TranslationDto FetchImpl(Guid id)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var currentUserId = ((CustomIdentity)(Csla.ApplicationContext.User.Identity)).UserId;
        var results = (from translationData in ctx.ObjectContext.TranslationDatas
                       where translationData.Id == id &&
                             translationData.UserDataId == currentUserId
                       select translationData);

        if (results.Count() == 1)
        {
          var fetchedTranslationData = results.First();

          var translationDto = EfHelper.ToDto(fetchedTranslationData);
          return translationDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phrases with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }

    protected override ICollection<TranslationDto> FetchImpl(ICollection<Guid> ids)
    {
      throw new NotImplementedException();
    }

    protected override TranslationDto UpdateImpl(TranslationDto dto)
    {
      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        var results = (from translationData in ctx.ObjectContext.TranslationDatas
                       where translationData.Id == dto.Id &&
                             translationData.UserDataId == dto.UserId
                       select translationData);

        if (results.Count() == 1)
        {
          //UPDATE THE TRANSLATIONDATA IN THE CONTEXT
          var translationData = results.First();
          EfHelper.LoadFrom(ref translationData, dto, ctx.ObjectContext);

          //SAVE TO MAKE SURE AFFECTED USERS ARE UPDATED TO REMOVE THIS PHRASE
          ctx.ObjectContext.SaveChanges();

          var updatedDto = EfHelper.ToDto(translationData);
          return updatedDto;
        }
        else
        {
          if (results.Count() == 0)
            throw new Exceptions.IdNotFoundException(dto.Id);
          else
          {
            //results.count is not one or zero.  either it's negative, which would be framework absurd, or its more than one,
            //which means that we have multiple phrases with the same id.  this is very bad.
            var errorMsg = string.Format(DalResources.ErrorMsgVeryBadException,
                                         DalResources.ErrorMsgVeryBadExceptionDetail_ResultCountNotOneOrZero);
            throw new Exceptions.VeryBadException(errorMsg);
          }
        }
      }
    }

    protected override TranslationDto InsertImpl(TranslationDto dto)
    {
      //left off here 01/29/2012
      throw new NotImplementedException();
    }

    protected override TranslationDto DeleteImpl(Guid id)
    {
      throw new NotImplementedException();
    }

    protected override ICollection<TranslationDto> GetAllImpl()
    {
      throw new NotImplementedException();
    }
  }
}

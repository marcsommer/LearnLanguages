using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace LearnLanguages.DataAccess.Ef
{
  public class LanguageDal : ILanguageDal
  {
    public Result<LanguageDto> New(object criteria)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        LanguageDto newLanguageDto = new LanguageDto()
        {
          Id = Guid.NewGuid(),
          Text = DalResources.DefaultNewLanguageText
        };
        retResult = Result<LanguageDto>.Success(newLanguageDto);
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
    public Result<LanguageDto> Fetch(Guid id)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from languageData in ctx.ObjectContext.LanguageDatas
                        where languageData.Id == id
                        select languageData;

          if (results.Count() == 1)
            retResult = Result<LanguageDto>.Success(EfHelper.ToDto(results.First()));
          else
          {
            if (results.Count() == 0)
              retResult = Result<LanguageDto>.FailureWithInfo(null,
                new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
      }
      return retResult;
    }
    public Result<LanguageDto> Update(LanguageDto dto)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from languageData in ctx.ObjectContext.LanguageDatas
                        where languageData.Id == dto.Id
                        select languageData;

          if (results.Count() == 1)
          {
            var languageDataToUpdate = results.First();
            ctx.ObjectContext.LanguageDatas.DeleteObject(languageDataToUpdate);
            dto.Id = Guid.NewGuid();
            ctx.ObjectContext.LanguageDatas.AddObject(EfHelper.ToData(dto));
            ctx.ObjectContext.SaveChanges();
            retResult = Result<LanguageDto>.Success(dto);
          }
          else
          {
            if (results.Count() == 0)
              retResult = Result<LanguageDto>.FailureWithInfo(null,
                new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
      }
      return retResult;
    }
    public Result<LanguageDto> Insert(LanguageDto dto)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from languageData in ctx.ObjectContext.LanguageDatas
                        where languageData.Id == dto.Id
                        select languageData;

          //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
          if (results.Count() == 0)
          {
            var data = EfHelper.ToData(dto);
            ctx.ObjectContext.LanguageDatas.AddObject(data);
            ctx.ObjectContext.SaveChanges();
            dto.Id = data.Id;
            retResult = Result<LanguageDto>.Success(dto);
          }
          else
          {
            if (results.Count() == 1) //ID ALREADY EXISTS
              retResult = Result<LanguageDto>.FailureWithInfo(dto,
                new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
            else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
              retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
      }
      return retResult;
    }
    public Result<LanguageDto> Delete(Guid id)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from languageData in ctx.ObjectContext.LanguageDatas
                        where languageData.Id == id
                        select languageData;

          if (results.Count() == 1)
          {
            var languageDataToRemove = results.First();
            ctx.ObjectContext.LanguageDatas.DeleteObject((languageDataToRemove));
            ctx.ObjectContext.SaveChanges();
            retResult = Result<LanguageDto>.Success(EfHelper.ToDto(languageDataToRemove));
          }
          else
          {
            if (results.Count() == 0)
              retResult = Result<LanguageDto>.FailureWithInfo(null,
                new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
      }
      return retResult;
    }

    public Result<ICollection<LanguageDto>> GetAll()
    {
      Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var allDtos = (

                         from languageData in ctx.ObjectContext.LanguageDatas
                         select new LanguageDto()
                         {
                           Id = languageData.Id,
                           Text = languageData.Text
                         }

                        ).ToList();
          //var allDtos = new List<LanguageDto>(ctx.ObjectContext.LanguageDatas);
          retResult = Result<ICollection<LanguageDto>>.Success(allDtos);
        }
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, new Exceptions.GetAllFailedException(ex));
      }
      return retResult;
    }
  }
}

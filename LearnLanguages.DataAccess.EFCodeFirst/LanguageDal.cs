using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnLanguages.DataAccess.EFCodeFirst
{
  public class LanguageDal : ILanguageDal
  {
    public Result<LanguageDto> New(object criteria)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        LanguageDto newLanguage = new LanguageDto()
        {
          Id = Guid.NewGuid(),
          Text = DalResources.DefaultNewLanguageText
        };
        retResult = Result<LanguageDto>.Success(newLanguage);
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<LanguageDto> Fetch(Guid id)
    {
      LanguageDto dto = null;
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from language in ctx.Context.Languages
                        where language.Id == id
                        select language;

          if (results.Count() == 1)
            retResult = Result<LanguageDto>.Success(results.First());
          else
          {
            if (results.Count() == 0)
              retResult = Result<LanguageDto>.FailureWithInfo(null,
                new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<LanguageDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
          }
        }

        retResult = Result<LanguageDto>.Success(dto);
      }
      catch (Exception ex)
      {
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<LanguageDto> Update(LanguageDto newDto)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from languageDto in ctx.Context.Languages
                        where languageDto.Id == newDto.Id
                        select languageDto;

          if (results.Count() == 1)
          {
            var languageToUpdate = results.First();
            ctx.Context.Languages.Remove(languageToUpdate);
            newDto.Id = Guid.NewGuid();
            ctx.Context.Languages.Add(newDto);
            retResult = Result<LanguageDto>.Success(newDto);
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
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<LanguageDto> Insert(LanguageDto dto)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from languageDto in ctx.Context.Languages
                        where languageDto.Id == dto.Id
                        select languageDto;

          //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
          if (results.Count() == 0)
          {
            dto.Id = Guid.NewGuid();
            ctx.Context.Languages.Add(dto);
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
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<LanguageDto> Delete(Guid id)
    {
      Result<LanguageDto> retResult = Result<LanguageDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from languageDto in ctx.Context.Languages
                        where languageDto.Id == id
                        select languageDto;

          if (results.Count() == 1)
          {
            var languageToRemove = results.First();
            ctx.Context.Languages.Remove(languageToRemove);
            retResult = Result<LanguageDto>.Success(languageToRemove);
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
        retResult = Result<LanguageDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<ICollection<LanguageDto>> GetAll()
    {
      Result<ICollection<LanguageDto>> retResult = Result<ICollection<LanguageDto>>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var allDtos = new List<LanguageDto>(ctx.Context.Languages);
          retResult = Result<ICollection<LanguageDto>>.Success(allDtos);
        }
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<LanguageDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Csla.Data;

using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace LearnLanguages.DataAccess.Ef
{
  public class PhraseDal : IPhraseDal
  {
    public Result<PhraseDto> New(object criteria)
    {
      //throw new NotImplementedException("Ef.PhraseDal.New(object)");
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto newPhraseDto = new PhraseDto()
        {
          Id = Guid.NewGuid(),
          Text = DalResources.DefaultNewPhraseText,
          LanguageId = GetDefaultLanguageId()
        };
        retResult = Result<PhraseDto>.Success(newPhraseDto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.CreateFailedException(ex));
      }
      return retResult;
    }
    public Result<PhraseDto> Fetch(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from phraseData in ctx.ObjectContext.PhraseDatas
                        where phraseData.Id == id
                        select phraseData;

          if (results.Count() == 1)
            retResult = Result<PhraseDto>.Success(EfHelper.ToDto(results.First()));
          else
          {
            if (results.Count() == 0)
              retResult = Result<PhraseDto>.FailureWithInfo(null,
                new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException(ex));
      }
      return retResult;
    }
    public Result<PhraseDto> Update(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from phraseData in ctx.ObjectContext.PhraseDatas
                        where phraseData.Id == dto.Id
                        select phraseData;

          if (results.Count() == 1)
          {
            var phraseDataToUpdate = results.First();
            ctx.ObjectContext.PhraseDatas.DeleteObject(phraseDataToUpdate);
            var newPhraseData = EfHelper.ToData(dto);
            ctx.ObjectContext.PhraseDatas.AddObject(newPhraseData);
            ctx.ObjectContext.SaveChanges();
            dto.Id = newPhraseData.Id;
            retResult = Result<PhraseDto>.Success(dto);
          }
          else
          {
            if (results.Count() == 0)
              retResult = Result<PhraseDto>.FailureWithInfo(null,
                new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.UpdateFailedException(ex));
      }
      return retResult;
    }
    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from phraseData in ctx.ObjectContext.PhraseDatas
                        where phraseData.Id == dto.Id
                        select phraseData;

          //SHOULD FIND ZERO LANGUAGEDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
          if (results.Count() == 0)
          {
            var data = EfHelper.ToData(dto);
            ctx.ObjectContext.PhraseDatas.AddObject(data);
            ctx.ObjectContext.SaveChanges();
            dto.Id = data.Id;
            retResult = Result<PhraseDto>.Success(dto);
          }
          else
          {
            if (results.Count() == 1) //ID ALREADY EXISTS
              retResult = Result<PhraseDto>.FailureWithInfo(dto,
                new Exceptions.InsertFailedException(DalResources.ErrorMsgIdNotFound));
            else                      //MULTIPLE IDS ALREADY EXIST?? SHOULD NOT BE POSSIBLE
              retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.InsertFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.InsertFailedException(ex));
      }
      return retResult;
    }
    public Result<PhraseDto> Delete(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var results = from phraseData in ctx.ObjectContext.PhraseDatas
                        where phraseData.Id == id
                        select phraseData;

          if (results.Count() == 1)
          {
            var phraseDataToRemove = results.First();
            ctx.ObjectContext.PhraseDatas.DeleteObject((phraseDataToRemove));
            ctx.ObjectContext.SaveChanges();
            retResult = Result<PhraseDto>.Success(EfHelper.ToDto(phraseDataToRemove));
          }
          else
          {
            if (results.Count() == 0)
              retResult = Result<PhraseDto>.FailureWithInfo(null,
                new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
          }
        }
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException(ex));
      }
      return retResult;
    }
    public Result<ICollection<PhraseDto>> GetAll()
    {
      Result<ICollection<PhraseDto>> retResult = Result<ICollection<PhraseDto>>.Undefined(null);
      try
      {
        using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
        {
          var allDtos = (

                         from phraseData in ctx.ObjectContext.PhraseDatas
                         select new PhraseDto()
                         {
                           Id = phraseData.Id,
                           Text = phraseData.Text
                         }

                        ).ToList();
          //var allDtos = new List<PhraseDto>(ctx.ObjectContext.PhraseDatas);
          retResult = Result<ICollection<PhraseDto>>.Success(allDtos);
        }
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    private Guid GetDefaultLanguageId()
    {
      Guid retDefaultLanguageId;

      using (var ctx = LearnLanguagesContextManager.Instance.GetManager())
      {
        try
        {

        retDefaultLanguageId = (from defaultLanguage in ctx.ObjectContext.LanguageDatas
                                where defaultLanguage.Text == EfResources.DefaultLanguageText
                                select defaultLanguage).First().Id;
        }
        catch (Exception ex)
        {
          throw new Exceptions.GeneralDataAccessException(ex);
        }
      }

      return retDefaultLanguageId;
    }
  }
}

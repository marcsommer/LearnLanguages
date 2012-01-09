using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnLanguages.DataAccess.EFCodeFirst
{
  public class PhraseDal : IPhraseDal
  {
    public Result<PhraseDto> New(object criteria)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto newPhrase = new PhraseDto()
        {
          Id = Guid.NewGuid(),
          Text = DalResources.DefaultNewPhraseText,
          LanguageId = Guid.Parse(DalResources.DefaultLanguageId)
        };
        retResult = Result<PhraseDto>.Success(newPhrase);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Fetch(Guid id)
    {
      PhraseDto dto = null;
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from Phrase in ctx.Context.Phrases
                        where Phrase.Id == id
                        select Phrase;

          if (results.Count() == 1)
            retResult = Result<PhraseDto>.Success(results.First());
          else
          {
            if (results.Count() == 0)
              retResult = Result<PhraseDto>.FailureWithInfo(null,
                new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFound));
            else
              retResult = Result<PhraseDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
          }
        }

        retResult = Result<PhraseDto>.Success(dto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Update(PhraseDto newDto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from PhraseDto in ctx.Context.Phrases
                        where PhraseDto.Id == newDto.Id
                        select PhraseDto;

          if (results.Count() == 1)
          {
            var PhraseToUpdate = results.First();
            ctx.Context.Phrases.Remove(PhraseToUpdate);
            newDto.Id = Guid.NewGuid();
            ctx.Context.Phrases.Add(newDto);
            retResult = Result<PhraseDto>.Success(newDto);
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
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from PhraseDto in ctx.Context.Phrases
                        where PhraseDto.Id == dto.Id
                        select PhraseDto;

          //SHOULD FIND ZERO PhraseDTOS (NO DUPLICATE IDS, NO DUPLICATE DTOS)
          if (results.Count() == 0)
          {
            dto.Id = Guid.NewGuid();
            ctx.Context.Phrases.Add(dto);
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
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Delete(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var results = from PhraseDto in ctx.Context.Phrases
                        where PhraseDto.Id == id
                        select PhraseDto;

          if (results.Count() == 1)
          {
            var PhraseToRemove = results.First();
            ctx.Context.Phrases.Remove(PhraseToRemove);
            retResult = Result<PhraseDto>.Success(PhraseToRemove);
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
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<ICollection<PhraseDto>> GetAll()
    {
      Result<ICollection<PhraseDto>> retResult = Result<ICollection<PhraseDto>>.Undefined(null);
      try
      {
        using (var ctx = DbContextManager<LearnLanguagesContext>.GetManager(EFCodeFirstResources.DatabaseName))
        {
          var allDtos = new List<PhraseDto>(ctx.Context.Phrases);
          retResult = Result<ICollection<PhraseDto>>.Success(allDtos);
        }
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}

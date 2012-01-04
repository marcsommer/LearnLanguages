using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess;
using LearnLanguages;
using System.ComponentModel.Composition;

namespace LearnPhrases.DataAccess.MockProvider
{
  [Export(typeof(IPhraseDalAsync))]
  public class PhraseAdapter : IPhraseDalAsync
  {
    public Result<PhraseDto> Delete(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto retDto = null;
        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<PhraseDto>.Success(retDto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Fetch(Guid id)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto retDto = null;
        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<PhraseDto>.Success(retDto);
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
        var retDtos = new List<PhraseDto>();
        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<ICollection<PhraseDto>>.Success(retDtos);
      }
      catch (Exception ex)
      {
        retResult = Result<ICollection<PhraseDto>>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto retDto = null;

        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<PhraseDto>.Success(retDto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> New(object criteria)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto retDto = null;
        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<PhraseDto>.Success(retDto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }

    public Result<PhraseDto> Update(PhraseDto dto)
    {
      Result<PhraseDto> retResult = Result<PhraseDto>.Undefined(null);
      try
      {
        PhraseDto retDto = null;

        //TODO: Do your work here, e.g.
        //dto.Id = Guid.NewGuid();
        //PhraseData data = new PhraseData() { Id = dto.Id, Phrase = dto.Phrase, Text = dto.Text };
        retResult = Result<PhraseDto>.Success(retDto);
      }
      catch (Exception ex)
      {
        retResult = Result<PhraseDto>.FailureWithInfo(null, ex);
      }
      return retResult;
    }
  }
}

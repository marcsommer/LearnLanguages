using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Service
{
  public class PhraseService : IPhraseDalAsync
  {
    public LearnLanguages.Result<PhraseDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<PhraseDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<PhraseDto> Update(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<PhraseDto> Insert(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<PhraseDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<PhraseDto> Delete(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<ICollection<PhraseDto>> GetAll()
    {
      throw new NotImplementedException();
    }
  }
}

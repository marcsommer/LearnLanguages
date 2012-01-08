using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.EFCodeFirst
{
  public class PhraseDal : IPhraseDal
  {
    public Result<PhraseDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Update(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public Result<ICollection<PhraseDto>> GetAll()
    {
      throw new NotImplementedException();
    }
  }
}

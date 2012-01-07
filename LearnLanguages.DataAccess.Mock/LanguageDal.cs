using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class LanguageDal : ILanguageDal
  {
    public Result<LanguageDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Update(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Insert(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<ICollection<LanguageDto>> GetAll()
    {
      throw new NotImplementedException();
    }
  }
}

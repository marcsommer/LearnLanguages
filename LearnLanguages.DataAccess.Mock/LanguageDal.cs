using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Mock
{
  public class LanguageDal : ILanguageDal
  {
    public LearnLanguages.Result<LanguageDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<LanguageDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<LanguageDto> Update(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<LanguageDto> Insert(LanguageDto dto)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<LanguageDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

    public LearnLanguages.Result<ICollection<LanguageDto>> GetAll()
    {
      throw new NotImplementedException();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface IPhraseDal
  {
    Result<PhraseDto> New(object criteria);
    Result<PhraseDto> Fetch(Guid id);
    Result<PhraseDto> Update(PhraseDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<PhraseDto> Insert(PhraseDto dto);
    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    Result<PhraseDto> Delete(Guid id);
    Result<ICollection<PhraseDto>> GetAll();
  }
}

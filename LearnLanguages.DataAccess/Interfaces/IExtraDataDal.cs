using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  public interface ICycleStudyDataDal
  {
    Result<CycleStudyDataDto> New(object criteria);
    Result<CycleStudyDataDto> Fetch(Guid id);
    Result<ICollection<CycleStudyDataDto>> Fetch(ICollection<Guid> ids);
    Result<CycleStudyDataDto> Update(CycleStudyDataDto dto);
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    Result<CycleStudyDataDto> Insert(CycleStudyDataDto dto);
    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    Result<CycleStudyDataDto> Delete(Guid id);
    Result<ICollection<CycleStudyDataDto>> GetAll();
  }
}

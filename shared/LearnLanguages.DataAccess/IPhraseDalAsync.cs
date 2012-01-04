using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LearnLanguages.DataAccess
{
  [ServiceContract]
  public interface IPhraseDalAsync
  {
    [OperationContract]
    Result<PhraseDto> New(object criteria);

    [OperationContract]
    Result<PhraseDto> Fetch(Guid id);
    
    [OperationContract]
    Result<PhraseDto> Update(PhraseDto dto);
    
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    [OperationContract]
    Result<PhraseDto> Insert(PhraseDto dto);

    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    [OperationContract]
    Result<PhraseDto> Delete(Guid id);

    [OperationContract]
    Result<ICollection<PhraseDto>> GetAll();

  }
}

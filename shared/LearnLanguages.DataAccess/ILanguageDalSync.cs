using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LearnLanguages.DataAccess
{
  [ServiceContract]
  public interface ILanguageDalSync
  {
    [OperationContract]
    Result<LanguageDto> New(object criteria);

    [OperationContract]
    Result<LanguageDto> Fetch(Guid id);
    
    [OperationContract]
    Result<LanguageDto> Update(LanguageDto dto);
    
    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    [OperationContract]
    Result<LanguageDto> Insert(LanguageDto dto);

    /// <summary>
    /// Apparently cannot overload when using WCF services.
    /// </summary>
    [OperationContract]
    Result<LanguageDto> Delete(Guid id);

    [OperationContract]
    Result<ICollection<LanguageDto>> GetAll();

  }
}

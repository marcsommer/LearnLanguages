using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LearnLanguages.DataAccess
{
  [ServiceContract]
  public interface ILanguageDalAsync
  {
    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginNew(object criteria, AsyncCallback callback, object state);
    Result<LanguageDto> EndNew(IAsyncResult result);

    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginFetch(Guid id, AsyncCallback callback, object state);
    Result<LanguageDto> EndFetch(IAsyncResult result);

    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginUpdate(LanguageDto dto, AsyncCallback callback, object state);
    Result<LanguageDto> EndUpdate(IAsyncResult result);

    /// <summary>
    /// Dal implementation is responsible for assigning new Id to dto
    /// </summary>
    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginInsert(LanguageDto dto, AsyncCallback callback, object state);
    Result<LanguageDto> EndInsert(IAsyncResult result);

    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginDelete(Guid id, AsyncCallback callback, object state);
    Result<LanguageDto> EndDelete(IAsyncResult result);
    
    [OperationContract(AsyncPattern = true)]
    IAsyncResult BeginGetAll(AsyncCallback callback, object state);
    Result<ICollection<LanguageDto>> EndGetAll(IAsyncResult result);
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnLanguages.DataAccess;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace LearnLanguages.Service
{
  public class LanguageService : ILanguageDalAsync
  {
    #region Fetch
    public Result<LanguageDto> Fetch(Guid id)
    {
      Result<LanguageDto> result = null;
      System.Diagnostics.Debug.WriteLine("fetch method called");
      //talk to db, get result
      return result;
    }
    public delegate Result<LanguageDto> FetchDelegate(Guid id);
    public IAsyncResult BeginFetch(Guid id, AsyncCallback callback, object state)
    {
      var fetchMethod = new FetchDelegate(Fetch);

      IAsyncResult iar = fetchMethod.BeginInvoke(id, callback, null);
      return iar;
    }
    public Result<LanguageDto> EndFetch(IAsyncResult iar)
    {
      AsyncResult result = (AsyncResult)iar;
      FetchDelegate caller = (FetchDelegate)result.AsyncDelegate;
      Result<LanguageDto> retResult = caller.EndInvoke(iar);
      return retResult;
    }

    ///// <summary>
    ///// To be used by client service.  Change beginFet
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //public Task<Result<LanguageDto>> FetchAsync(Guid id)
    //{
    //  var tcs = new TaskCompletionSource<Result<LanguageDto>>();
    //  _Client.BeginFetch(id,
    //             iar =>
    //             {
    //               try 
    //               {
    //                 var result = _Client.EndFetch(iar);
    //                 tcs.SetResult(result);
    //               }
    //               catch (Exception ex) 
    //               { 
    //                 tcs.SetException(ex); 
    //               }
    //             }, 
    //             null);
    //  return tcs.Task;
    //}
    #endregion //Fetch

    public IAsyncResult BeginNew(object criteria, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }
    public Result<LanguageDto> EndNew(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public IAsyncResult BeginUpdate(LanguageDto dto, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }
    public Result<LanguageDto> EndUpdate(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public IAsyncResult BeginInsert(LanguageDto dto, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }
    public Result<LanguageDto> EndInsert(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public IAsyncResult BeginDelete(Guid id, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }
    public Result<LanguageDto> EndDelete(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public IAsyncResult BeginGetAll(AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }
    public Result<ICollection<LanguageDto>> EndGetAll(IAsyncResult result)
    {
      throw new NotImplementedException();
    }
  }
}

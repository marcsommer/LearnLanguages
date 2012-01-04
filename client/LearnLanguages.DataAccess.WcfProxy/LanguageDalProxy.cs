using System;
using System.ComponentModel.Composition;
using LearnLanguages.DataAccess.WcfProxy.WcfServices.Language;

namespace LearnLanguages.DataAccess.WcfProxy
{
  [Export(typeof(ILanguageDalAsync))]
  public class LanguageDalProxy : ILanguageDalAsync
  {

    public IAsyncResult BeginNew(object criteria, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> EndNew(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public IAsyncResult BeginFetch(Guid id, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> EndFetch(IAsyncResult result)
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

    public Result<System.Collections.Generic.ICollection<LanguageDto>> EndGetAll(IAsyncResult result)
    {
      throw new NotImplementedException();
    }
  }
}

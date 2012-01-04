using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace LearnLanguages.DataAccess.MockProvider
{
  [Export(typeof(ILanguageDalAsync))]
  public class LanguageAdapter : ILanguageDalAsync, ILanguageDalSync
  {
    
    public IAsyncResult BeginNew(object criteria, AsyncCallback callback, object state)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> EndNew(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> Fetch(Guid id)
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

    public Result<ICollection<LanguageDto>> EndGetAll(IAsyncResult result)
    {
      throw new NotImplementedException();
    }

    public Result<LanguageDto> New(object criteria)
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

    public Result<ICollection<LanguageDto>> GetAll()
    {
      throw new NotImplementedException();
    }
  }
}

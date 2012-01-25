using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess
{
  /// <summary>
  /// This class wraps every ITranslationDal with a try..catch wrapper
  /// block that does the wrapping for each call.  The descending classes only need
  /// to implement the 
  /// </summary>
  public abstract class TranslationDalBase : ITranslationDal
  {
    public Result<TranslationDto> New(object criteria)
    {
      Result<TranslationDto> retResult = Result<TranslationDto>.Undefined(null);
      try
      {
        CheckAuthentication();

        var dto = NewImpl(criteria);
        retResult = Result<TranslationDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.CreateFailedException(ex);
        retResult = Result<TranslationDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<TranslationDto> Fetch(Guid id)
    {
      Result<TranslationDto> retResult = Result<TranslationDto>.Undefined(null);
      try
      {
        CheckAuthentication();

        var dto = FetchImpl(id);
        retResult = Result<TranslationDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<TranslationDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<TranslationDto>> Fetch(ICollection<Guid> ids)
    {
      Result<ICollection<TranslationDto>> retResult = Result<ICollection<TranslationDto>>.Undefined(null);
      try
      {
        CheckAuthentication();

        var dtos = FetchImpl(ids);
        retResult = Result<ICollection<TranslationDto>>.Success(dtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.FetchFailedException(ex);
        retResult = Result<ICollection<TranslationDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<TranslationDto> Update(TranslationDto dtoToUpdate)
    {
      Result<TranslationDto> retResult = Result<TranslationDto>.Undefined(null);
      try
      {
        CheckAuthentication();

        var updatedDto = UpdateImpl(dtoToUpdate);
        retResult = Result<TranslationDto>.Success(updatedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.UpdateFailedException(ex);
        retResult = Result<TranslationDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<TranslationDto> Insert(TranslationDto dtoToInsert)
    {
      Result<TranslationDto> retResult = Result<TranslationDto>.Undefined(null);
      try
      {
        CheckAuthentication();
        
        var insertedDto = InsertImpl(dtoToInsert);
        retResult = Result<TranslationDto>.Success(insertedDto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.InsertFailedException(ex);
        retResult = Result<TranslationDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<TranslationDto> Delete(Guid id)
    {
      Result<TranslationDto> retResult = Result<TranslationDto>.Undefined(null);
      try
      {
        CheckAuthentication();
        
        var dto = DeleteImpl(id);
        retResult = Result<TranslationDto>.Success(dto);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.DeleteFailedException(ex);
        retResult = Result<TranslationDto>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }
    public Result<ICollection<TranslationDto>> GetAll()
    {
      Result<ICollection<TranslationDto>> retResult = Result<ICollection<TranslationDto>>.Undefined(null);
      try
      {
        CheckAuthentication();
        
        var allDtos = GetAllImpl();
        retResult = Result<ICollection<TranslationDto>>.Success(allDtos);
      }
      catch (Exception ex)
      {
        var wrappedEx = new Exceptions.GetAllFailedException(ex);
        retResult = Result<ICollection<TranslationDto>>.FailureWithInfo(null, wrappedEx);
      }
      return retResult;
    }

    protected abstract TranslationDto NewImpl(object criteria);
    protected abstract TranslationDto FetchImpl(Guid id);
    protected abstract ICollection<TranslationDto> FetchImpl(ICollection<Guid> ids);
    protected abstract TranslationDto UpdateImpl(TranslationDto dto);
    protected abstract TranslationDto InsertImpl(TranslationDto dto);
    protected abstract TranslationDto DeleteImpl(Guid id);
    protected abstract ICollection<TranslationDto> GetAllImpl();

    protected void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Exceptions.UserNotAuthenticatedException();
    }


  }
}

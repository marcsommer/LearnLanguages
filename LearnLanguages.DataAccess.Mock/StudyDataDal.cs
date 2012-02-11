using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LearnLanguages.DataAccess.Mock
{
  public class StudyDataDal : StudyDataDalBase
  {
    //public Result<StudyDataDto> New(object criteria)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    var dto = new StudyDataDto() 
    //    { 
    //      Id = Guid.NewGuid(),
    //      LanguageId = SeedData.Instance.DefaultLanguageId
    //    };
    //    retResult = Result<StudyDataDto>.Success(dto);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<StudyDataDto> Fetch(Guid id)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Instance.StudyDatas
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //      retResult = Result<StudyDataDto>.Success(results.First());
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null,
    //          new Exceptions.FetchFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<StudyDataDto> Update(StudyDataDto dto)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Instance.StudyDatas
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var StudyDataToUpdate = results.First();
    //      SeedData.Instance.StudyDatas.Remove(StudyDataToUpdate);
    //      dto.Id = Guid.NewGuid();
    //      SeedData.Instance.StudyDatas.Add(dto);
    //      retResult = Result<StudyDataDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<StudyDataDto> Insert(StudyDataDto dto)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Instance.StudyDatas
    //                  where item.Id == dto.Id
    //                  select item;

    //    if (results.Count() == 0)
    //    {
    //      dto.Id = Guid.NewGuid();
    //      //MIMIC LANGUAGEID REQUIRED CONSTRAINT IN DB
    //      if (dto.LanguageId == Guid.Empty || !SeedData.Instance.ContainsLanguageId(dto.LanguageId))
    //      {
    //        //I'VE RESTRUCTURED HOW TO DO EXCEPTIONHANDLING, SO THIS IS NOT QUITE HOW IT SHOULD BE DONE.
    //        //THIS SHOULD BE AN INSERTIMPL METHOD, AND IT SHOULD THROW ITS OWN EXCEPTION THAT IS WRAPPED IN THE 
    //        //PHRASEDALBASE CLASS IN AN INSERTFAILEDEXCEPTION.
    //        throw new Exceptions.InsertFailedException(string.Format(DalResources.ErrorMsgIdNotFoundException, dto.LanguageId));
    //      }
    //      SeedData.Instance.StudyDatas.Add(dto);
    //      retResult = Result<StudyDataDto>.Success(dto);
    //    }
    //    else
    //    {
    //      if (results.Count() == 1) //ID ALREADY EXISTS
    //        retResult = Result<StudyDataDto>.FailureWithInfo(dto,
    //          new Exceptions.UpdateFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else                      //MULTIPLE IDS ALREADY EXIST??
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null, new Exceptions.FetchFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public Result<StudyDataDto> Delete(Guid id)
    //{
    //  Result<StudyDataDto> retResult = Result<StudyDataDto>.Undefined(null);
    //  try
    //  {
    //    var results = from item in SeedData.Instance.StudyDatas
    //                  where item.Id == id
    //                  select item;

    //    if (results.Count() == 1)
    //    {
    //      var StudyDataToRemove = results.First();
    //      SeedData.Instance.StudyDatas.Remove(StudyDataToRemove);
    //      retResult = Result<StudyDataDto>.Success(StudyDataToRemove);
    //    }
    //    else
    //    {
    //      if (results.Count() == 0)
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null,
    //          new Exceptions.DeleteFailedException(DalResources.ErrorMsgIdNotFoundException));
    //      else
    //        retResult = Result<StudyDataDto>.FailureWithInfo(null, new Exceptions.DeleteFailedException());
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<StudyDataDto>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}
    //public LearnLanguages.Result<ICollection<StudyDataDto>> GetAll()
    //{
    //  Result<ICollection<StudyDataDto>> retResult = Result<ICollection<StudyDataDto>>.Undefined(null);
    //  try
    //  {
    //    var allDtos = new List<StudyDataDto>(SeedData.Instance.StudyDatas);
    //    retResult = Result<ICollection<StudyDataDto>>.Success(allDtos);
    //  }
    //  catch (Exception ex)
    //  {
    //    retResult = Result<ICollection<StudyDataDto>>.FailureWithInfo(null, ex);
    //  }
    //  return retResult;
    //}

    protected override StudyDataDto NewImpl(object criteria)
    {
      //DALBASE CHECKS AUTHENTICATION
      //to get to this point, we must have already been authenticated
      //Debug.Assert(Csla.ApplicationContext.User.Identity.IsAuthenticated);
      //if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
      //  throw new Exceptions.UserNotAuthenticatedException();

      var username = Csla.ApplicationContext.User.Identity.Name;

      var dto = new StudyDataDto()
      {
        Id = Guid.NewGuid(),
        NativeLanguageText = DalResources.DefaultNativeLanguageText,
        Username = username
      };

      return dto;
    }
    protected override StudyDataDto FetchForCurrentUserImpl()
    {
      var currentUsername = Csla.ApplicationContext.User.Identity.Name;

      var results = from studyData in SeedData.Instance.StudyDatas
                    where studyData.Username == currentUsername
                    select studyData;

      StudyDataDto dto = null;

      if (results.Count() == 1)
      {
        dto = results.First();
        return dto;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.StudyDataNotFoundForUserException(currentUsername);
        else
          throw new Exceptions.VeryBadException();
      }

    }
    protected override bool StudyDataExistsForCurrentUserImpl()
    {
      var currentUsername = Csla.ApplicationContext.User.Identity.Name;

      var results = from studyData in SeedData.Instance.StudyDatas
                    where studyData.Username == currentUsername
                    select studyData;
      
      if (results.Count() == 1)
      {
        return true;
      }
      else
      {
        if (results.Count() == 0)
          return false;
        else
          throw new Exceptions.VeryBadException();
      }

    }
    //protected override StudyDataDto FetchImpl(Guid id)
    //{
    //  var results = from item in SeedData.Instance.StudyDatas
    //                where item.Id == id
    //                select item;

    //  if (results.Count() == 1)
    //    return results.First();
    //  else
    //  {
    //    if (results.Count() == 0)
    //      throw new Exceptions.IdNotFoundException(id);
    //    else
    //      throw new Exceptions.VeryBadException();
    //  }
    //}
    //protected override ICollection<StudyDataDto> FetchImpl(ICollection<Guid> ids)
    //{
    //  if (ids == null)
    //    throw new ArgumentNullException("ids");
    //  else if (ids.Count == 0)
    //    throw new ArgumentOutOfRangeException("ids", "ids cannot be empty.");

    //  var retStudyDatas = new List<StudyDataDto>();

    //  foreach (var id in ids)
    //  {
    //    var dto = FetchImpl(id);
    //    retStudyDatas.Add(dto);
    //  }

    //  return retStudyDatas;
    //}
    protected override StudyDataDto UpdateImpl(StudyDataDto dto)
    {
      var results = from item in SeedData.Instance.StudyDatas
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 1)
      {
        CheckContraints(dto);

        var studyDataToUpdate = results.First();
        SeedData.Instance.StudyDatas.Remove(studyDataToUpdate);
        dto.Id = Guid.NewGuid();
        SeedData.Instance.StudyDatas.Add(dto);
        return dto;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(dto.Id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override StudyDataDto InsertImpl(StudyDataDto dto)
    {
      var results = from item in SeedData.Instance.StudyDatas
                    where item.Id == dto.Id
                    select item;

      if (results.Count() == 0)
      {
        CheckContraints(dto);

        dto.Id = Guid.NewGuid();
        SeedData.Instance.StudyDatas.Add(dto);
        return dto;
      }
      else
      {
        if (results.Count() == 1) //ID ALREADY EXISTS
          throw new Exceptions.IdAlreadyExistsException(dto.Id);
        else                      //MULTIPLE IDS ALREADY EXIST??
          throw new Exceptions.VeryBadException();
      }
    }
    protected override StudyDataDto DeleteImpl(Guid id)
    {
      var results = from item in SeedData.Instance.StudyDatas
                    where item.Id == id
                    select item;

      if (results.Count() == 1)
      {
        var studyDataToRemove = results.First();
        SeedData.Instance.StudyDatas.Remove(studyDataToRemove);
        return studyDataToRemove;
      }
      else
      {
        if (results.Count() == 0)
          throw new Exceptions.IdNotFoundException(id);
        else
          throw new Exceptions.VeryBadException();
      }
    }
    protected override ICollection<StudyDataDto> GetAllImpl()
    {
      var allDtos = new List<StudyDataDto>(SeedData.Instance.StudyDatas);
      return allDtos;
    }

    private void CheckContraints(StudyDataDto dto)
    {
      if (string.IsNullOrEmpty(dto.NativeLanguageText))
        throw new ArgumentException("dto.NativeLanguageText");
      if (string.IsNullOrEmpty(dto.Username))
        throw new ArgumentException("dto.Username");
    }
  }
}

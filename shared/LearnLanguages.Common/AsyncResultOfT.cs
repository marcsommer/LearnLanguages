using LearnLanguages.Common.Interfaces;
using LearnLanguages.Statuses;
using System;
using Csla.Serialization;

namespace LearnLanguages
{
  [Serializable]
  public class AsyncResult<T> : Result<T>, IAsyncStub
  {
    public AsyncResult(T resultObj,  
                       bool isSuccess = true, 
                       string msg = "", 
                       params Tuple<string, object>[] infoParams)
      : base(resultObj, isSuccess, msg, infoParams)
    {
      
    }

    private object _StateLock = new object();
    private object _State;
    public object State
    {
      get
      {
        lock (_StateLock)
        {
          return _State; 
        }
      }
      set
      {
        lock (_StateLock)
        {
          if (_State != value)
            _State = value;
        }
      }
    }

    private object _IsCompletedLock = new object();
    private bool _IsCompleted = false;
    public bool IsCompleted
    {
      get
      {
        lock (_IsCompletedLock)
        {
          return _IsCompleted;
        }
      }
      set
      {
        lock (_IsCompletedLock)
        {
          //can only set IsCompleted to true once
          if (!_IsCompleted && _IsCompleted != value)
            _IsCompleted = value;
        }
      }
    }

    private object _StatusLock = new object();
    private Status _Status = Status.Common[CommonResources.StatusInitialized];
    public Status Status
    {
      get
      {
        lock (_StatusLock)
        {
          return _Status;
        }
      }
      set
      {
        lock (_StatusLock)
        {
          if (_Status != value)
            _Status = value;
        }
      }
    }

    #region Common Results

    new public static AsyncResult<T> Undefined(T resultObj)
    {
      return new AsyncResult<T>(resultObj, false, CommonResources.ResultUndefined);
    }

    new public static AsyncResult<T> UndefinedWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new AsyncResult<T>(resultObj, false, CommonResources.ResultUndefinedWithInfo, infos);
    }

    new public static AsyncResult<T> Success(T resultObj)
    {
      return new AsyncResult<T>(resultObj, true, CommonResources.ResultSuccess);
    }

    new public static AsyncResult<T> SuccessWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new AsyncResult<T>(resultObj, false, CommonResources.ResultSuccessWithInfo, infos);
    }

    new public static AsyncResult<T> Failure(T resultObj)
    {
      return new AsyncResult<T>(resultObj, false, CommonResources.ResultFailure);
    }

    new public static AsyncResult<T> FailureWithInfo(T resultObj, params Tuple<string, object>[] infos)
    {
      return new AsyncResult<T>(resultObj, false, CommonResources.ResultFailureWithInfo, infos);
    }

    new public static AsyncResult<T> FailureWithInfo(T resultObj, Exception ex)
    {
      Tuple<string, object> infoExObject =
            new Tuple<string, object>() { Item1 = CommonResources.InfoKeyExceptionObject, Item2 = ex };
      AsyncResult<T> retResult = new AsyncResult<T>(resultObj, false, ex.Message, infoExObject);
      return retResult;
    }

    #endregion
  }
}

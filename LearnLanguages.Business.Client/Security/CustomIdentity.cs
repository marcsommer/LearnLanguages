using System;
using Csla.Security;
using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
#if !SILVERLIGHT
#endif

namespace LearnLanguages.Business.Security
{
  [Serializable]
  public class CustomIdentity : CslaIdentityBase<CustomIdentity>
  {
    #region Factory Methods

#if !SILVERLIGHT
    public static CustomIdentity GetCustomIdentity(string username, string clearUnsaltedPassword)
    {
      var criteria = new UsernameCriteria(username, clearUnsaltedPassword);
      return DataPortal.Fetch<CustomIdentity>(criteria);
    }

    internal static CustomIdentity GetCustomIdentity(string username)
    {
      return DataPortal.Fetch<CustomIdentity>(username);
    }

#endif

    public static void GetCustomIdentity(string username,
                                         string clearUnsaltedPassword,
                                         EventHandler<DataPortalResult<CustomIdentity>> callback)
    {
      var criteria = new UsernameCriteria(username, clearUnsaltedPassword);
      DataPortal.BeginFetch<CustomIdentity>(criteria, callback);
    }

    #endregion

    #region public int Salt
    public static readonly PropertyInfo<int> SaltProperty = RegisterProperty<int>(c => c.Salt);
    public int Salt
    {
      get { return ReadProperty(SaltProperty); }
      private set { LoadProperty(SaltProperty, value); }
    }
    #endregion

#if !SILVERLIGHT
    /// <summary>
    /// This should run ONLY on the server.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="dal"></param>
    private void LoadUserData(string username, ICustomIdentityDal dal)
    {
      var result = dal.GetUser(username);
      if (!result.IsSuccess || result.IsError)
        throw new GeneralDataAccessException(result.Msg);

      var userData = result.Obj;

      IsAuthenticated = (userData != null);
      if (IsAuthenticated)
      {
        Name = userData.Username;
        Salt = userData.Salt;

        var resultRoles = dal.GetRoles(Name);
        if (!resultRoles.IsSuccess || resultRoles.IsError || resultRoles.Obj.Count == 0)
          throw new GeneralDataAccessException(resultRoles.Msg);

        Roles = new Csla.Core.MobileList<string>();
        foreach (var roleDto in resultRoles.Obj)
        {
          Roles.Add(roleDto.Text);
        }
      }
    }

    private void DataPortal_Fetch(UsernameCriteria criteria)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<ICustomIdentityDal>();

        var verifyResult = dal.VerifyUser(criteria.Username, criteria.Password);
        if (!verifyResult.IsSuccess || verifyResult.IsError)
          throw new FetchFailedException(verifyResult.Msg);
        bool? userIsVerified = verifyResult.Obj;
        if (userIsVerified == null)
          userIsVerified = false;

        if (userIsVerified == true)
          LoadUserData(criteria.Username, dal);
      }
    }
    private void DataPortal_Fetch(string username)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<ICustomIdentityDal>();
        LoadUserData(username, dal);
      }
    }

    private void Child_Fetch(string username)
    {
      AuthenticationType = DalResources.AuthenticationTypeString;
      using (var dalManager = DataAccess.DalFactory.GetDalManager())
      {
        var dal = dalManager.GetProvider<ICustomIdentityDal>();
        LoadUserData(username, dal);
      }
    }
#endif
  }
}

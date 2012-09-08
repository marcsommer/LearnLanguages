using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business.Security;
using Microsoft.Silverlight.Testing;

namespace LearnLanguages.Silverlight.Tests
{
  [TestClass]
  [Tag("sec")]
  public class SecurityTests : Microsoft.Silverlight.Testing.SilverlightTest
  {

    //private void OutputExecutionLocations()
    //{
    //  string msg = "ExecutionLocation: " + Csla.ApplicationContext.ExecutionLocation.ToString();
    //  msg = msg + "\r\nLogicalExecutionLocation: " + Csla.ApplicationContext.LogicalExecutionLocation.ToString();
    //  MessageBox.Show(msg);
    //}


    //[ClassInitialize]
    //public void Initialize()
    //{
    //  _TestUsername = "user";
    //  _TestRole = "Users";
    //  _TestPassword = "password";
    //  _TestSaltedHashedPassword = new SaltedHashedPassword(_TestPassword);
    //  _TestSalt = _TestSaltedHashedPassword.Salt;

    //  MockDb.Users.Add(new User()
    //    {
    //      Username = _TestUsername,
    //      SaltedHashedPasswordValue = _TestSaltedHashedPassword.Value,
    //      Salt = _TestSalt
    //    });

    //  MockDb.UserRoles.Add(new UserRole() 
    //    { 
    //      Username = _TestUsername, 
    //      Role = _TestRole 
    //    });


    //}

    private string _TestValidUsername = "user";
    private string _TestRole = "Admin";
    private string _TestValidPassword = "password";
    //private SaltedHashedPassword _TestSaltedHashedPassword;
    private string _TestSaltedHashedPassword = @"瞌訖ꎚ壿喐ຯ缟㕧";
    private int _TestSalt = -54623530;

    private string _TestInvalidUsername = "ImNotAValidUser";
    private string _TestInvalidPassword = "ImNotAValidPassword";

    [TestMethod]
    [Asynchronous]
    [Tag("valid")]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_SUCCESS_VALID_USERNAME_VALID_PASSWORD()
    {
      var isInitialized = false;

      CustomPrincipal.BeginLogin(_TestValidUsername, _TestValidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsTrue(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_INVALID_USERNAME_INVALID_PASSWORD()
    {
      var isInitialized = false;

      CustomPrincipal.BeginLogin(_TestInvalidUsername, _TestInvalidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_VALID_USERNAME_INVALID_PASSWORD()
    {
      var isInitialized = false;

      CustomPrincipal.BeginLogin(_TestValidUsername, _TestInvalidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    public void TEST_CUSTOM_PRINCIPAL_BEGIN_LOGIN_FAIL_INVALID_USERNAME_VALID_PASSWORD()
    {
      var isInitialized = false;

      CustomPrincipal.BeginLogin(_TestInvalidUsername, _TestValidPassword, (e) =>
      {
        if (e != null)
          throw e;
        isInitialized = true;
      });

      EnqueueConditional(() => isInitialized);

      EnqueueCallback(() => { Assert.IsFalse(Csla.ApplicationContext.User.IsInRole(_TestRole)); },
                      () => { Assert.AreNotEqual(_TestValidUsername, Csla.ApplicationContext.User.Identity.Name); },
                      () => { Assert.IsFalse(Csla.ApplicationContext.User.Identity.IsAuthenticated); });

      EnqueueTestComplete();
    }

    [TestMethod]
    [Asynchronous]
    [Tag("current")]
    public void TEST_ADD_USER()
    {
      var isCreated = false;

      var testUsername = "BobValidUsername";
      var testPassword = "password1";
      var criteria = new Business.Criteria.UserInfoCriteria(testUsername, testPassword);
      Business.NewUserCreator.CreateNew(criteria, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          isCreated = true;
        });


      EnqueueConditional(() => isCreated);

      EnqueueCallback(() => { Assert.IsTrue(Csla.ApplicationContext.User.IsInRole(DataAccess.SeedData.Ton.AdminRoleText)); });

      EnqueueTestComplete();
    }
  }
}

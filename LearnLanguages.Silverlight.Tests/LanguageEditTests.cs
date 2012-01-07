using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;
using Microsoft.Silverlight.Testing;

namespace LearnLanguages.Client.Tests
{
  [TestClass]
  public class LanguageEditTests : Microsoft.Silverlight.Testing.SilverlightTest
  {
    [TestMethod]
    public void DP_CREATE_NEW()
    {
      LanguageEdit.NewLanguageEdit( (s,r) =>
        {
          if (r.Error != null)
            throw new Exception();
          if (r.Object == null)
            throw new Exception();
        });
    }

    [TestMethod]
    [Asynchronous]
    public void DP_FETCH()
    {
      Guid testId = Guid.Parse(@"BFA56C9F-31C3-495D-973E-5A5FF66983A2");
      var isLoaded = false;
      Exception error = null;
      LanguageEdit person = null;

      LanguageEdit.GetLanguageEdit(testId, (s, r) =>
      {
        //OutputExecutionLocations();
        error = r.Error;
        person = r.Object;
        //MessageBox.Show(r.Object.FirstName);
        isLoaded = true;
      });

      EnqueueConditional(() => isLoaded);
      EnqueueCallback(
        () =>
        {
          if (error != null)
            throw error;
        },
        () => Assert.IsNotNull(person)
        );

      EnqueueTestComplete();

    }
  }
}
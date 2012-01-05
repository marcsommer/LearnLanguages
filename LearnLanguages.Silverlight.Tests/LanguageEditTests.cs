using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;

namespace LearnLanguages.Client.Tests
{
  [TestClass]
  public class LanguageEditTests
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
    public void DP_FETCH()
    {
      Guid testId = Guid.Parse(@"BFA56C9F-31C3-495D-973E-5A5FF66983A2");
      LanguageEdit.GetLanguageEdit(testId, (s, r) =>
      {
        if (r.Error != null)
          throw new Exception();
        if (r.Object == null)
          throw new Exception();
      });
    }
  }
}
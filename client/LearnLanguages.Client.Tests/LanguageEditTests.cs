using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnLanguages.Business;

namespace LearnLanguages.Client.Tests
{
  [TestClass]
  public class LanguageEditTests
  {
    [TestMethod]
    public void TEST_DP_CREATE_NEW()
    {
      
      PhraseEdit.NewPhraseEdit( (s,r) =>
        {
          if (r.Error != null)
            throw new Exception();
          if (r.Object == null)
            throw new Exception();
        });
    }

    //public void TEST_DP_CREATE_NEW_CALLBACK(
  }
}
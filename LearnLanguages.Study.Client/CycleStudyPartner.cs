using System;
using LearnLanguages.Study.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The CycleStudyPartner is a simple study partner that asks what the user's native language is.
  /// It then cycles through phrases in foreign languages asking if the user knows it.  It has no memory.
  /// This is my most basic partner, as I am just forming how they work.  This isn't really a practical study
  /// partner.
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class CycleStudyPartner : StudyPartnerBase
  {

    protected override void StudyImpl()
    {
      AskUserExtraData();
      System.Windows.MessageBox.Show("study impl()");
    }

    protected override void AskUserExtraDataImpl()
    {
      System.Windows.MessageBox.Show("ask extra impl()");
    }
  }
}

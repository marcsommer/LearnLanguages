using System;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IHaveModelList<TList>
    where TList : class
  {
    TList ModelList { get; set; }
  }
}

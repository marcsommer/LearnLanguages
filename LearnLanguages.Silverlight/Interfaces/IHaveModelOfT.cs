using System;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IHaveModel<T>
    where T : class
  {
    T Model { get; set; }
  }
}

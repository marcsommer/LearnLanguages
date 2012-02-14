using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  /// <summary>
  /// Currently a marker interface, conveying that the context of this studier
  /// entails meaning.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IMeaningStudier<T> : IStudier<T>
  {
  }
}

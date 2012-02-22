using System;
using System.Net;

using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  public class StudyItemCreator
  {
    #region Singleton Pattern Members
    private static volatile StudyItemCreator _Ton;
    private static object _Lock = new object();
    public static StudyItemCreator Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new StudyItemCreator();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public IStudyItemViewModelBase TranslationQA(PhraseEdit phrase, 
                                                 string nativeLanguageText, 
                                                 AsyncCallback<IStudyItemViewModelBase> callback)
    {
      //FOR NOW, THIS WILL JUST CREATE ONE TYPE OF STUDY ITEM VIEW MODEL: STUDY QUESTION ANSWER VIEWMODEL.
      //IT WILL SHOW THE QUESTION AND HIDE THE ANSWER FOR VARIOUS AMOUNTS OF TIME, DEPENDING ON QUESTION
      //LENGTH, AND OTHER FACTORS.
      //IN THE FUTURE, THIS WILL BE EXTENSIBILITY POINT WHERE WE CAN SELECT DIFFERENT VARIETIES OF Q AND A 
      //TRANSLATION VIEW MODELS BASED ON HOW SUCCESSFUL THEY ARE.  WE CAN ALSO HAVE DIFFERENT CONFIGURATIONS
      //OF VARIETIES BE SELECTED HERE.

      ViewModels.StudyQuestionAnswerViewModel viewModel = new ViewModels.StudyQuestionAnswerViewModel();
      var words = phrase.Text.ParseIntoWords();
      var duration = words.Count * (int.Parse(StudyResources.DefaultMillisecondsTimePerWordInQuestion));
    }
  }
}

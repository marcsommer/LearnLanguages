using System;
using System.Net;
using LearnLanguages.Business;

namespace LearnLanguages.Delegates
{
  public delegate void QuestionAnswerCallback(PhraseEdit question, PhraseEdit answer, Exception exception);
}

using System;
using System.Net;
//using Caliburn.Micro;
using LearnLanguages.Business;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyItemViewModelBase : IViewModelBase, IStudyReviewMethod
  {
    void Show(ExceptionCheckCallback callback);
    void Abort();
  }
}

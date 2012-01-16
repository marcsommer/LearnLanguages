using System;
using System.Collections.Generic;
using LearnLanguages.Silverlight.Interfaces;

namespace LearnLanguages.Silverlight.Events
{
  public class NavigatedEventMessage : NavigationEventMessage,
                                                   INavigatedEventMessage
  {
    public NavigatedEventMessage(NavigationInfo navigationInfo)
      : base(navigationInfo)
    {

    }
    ///// <summary>
    ///// Navigation requested event message, for navigating to view models using convention over configuration.
    ///// EXPECTED FORMAT: [CoreText]ViewModel
    ///// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    ///// </summary>
    ///// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    ///// <param name="queryString"></param>
    //public NavigatedEventMessage(string requestedViewModelCoreText, Guid navigationId)
    //  : base(requestedViewModelCoreText, navigationId)
    //{
    //}

    //public NavigatedEventMessage(string requestedViewModelCoreText, string query, Guid navigationId)
    //  : base(requestedViewModelCoreText, query, navigationId)
    //{
    //}

    //public NavigatedEventMessage(string requestedViewModelCoreText, 
    //                             IDictionary<string, string> queryEntries,
    //                             Guid navigationId)
    //  : base(requestedViewModelCoreText, queryEntries, navigationId)
    //{
    //}

  }
}

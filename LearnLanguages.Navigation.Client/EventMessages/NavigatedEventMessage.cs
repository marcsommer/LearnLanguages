using System;
using System.Collections.Generic;
using LearnLanguages.Navigation.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Navigation.EventMessages
{
  public class NavigatedEventMessage : NavigationEventMessage, INavigatedEventMessage
  {
    public NavigatedEventMessage(NavigationInfo navigationInfo, IViewModelBase viewModel)
      : base(navigationInfo)
    {
      ViewModel = viewModel;
    }

    public IViewModelBase ViewModel { get; private set; }
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

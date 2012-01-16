using System;
using LearnLanguages.Silverlight.Interfaces;
using System.Collections.Generic;

namespace LearnLanguages.Silverlight
{
  /// <summary>
  /// NOT SERIALIZABLE!!!  No public default constructor.  Not marked as serializable.  
  /// I note this because I foresee the possibility of having to change this if I 
  /// ever need to Csla-ize this class.
  /// </summary>
  public class NavigationInfo : IHaveUri 
  {
    /// <summary>
    /// Navigation requested event message, for navigating to view models using convention over configuration.
    /// EXPECTED FORMAT: [CoreText]ViewModel
    /// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    /// </summary>
    /// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    /// <param name="queryString"></param>
    public NavigationInfo(Guid navigationId, string viewModelCoreNoSpaces)
    {
      if (navigationId == Guid.Empty)
        throw new ArgumentException("navigationId is Guid.Empty");

      NavigationId = navigationId;
      ViewModelCoreNoSpaces = viewModelCoreNoSpaces;
      Uri = new Uri(AppResources.BaseAddress + "/" + ViewModelCoreNoSpaces, UriKind.Absolute);
    }
    /// <summary>
    /// Navigation requested event message, for navigating to view models using convention over configuration.
    /// EXPECTED FORMAT: [CoreText]ViewModel
    /// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    /// </summary>
    /// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    /// <param name="queryString"></param>
    public NavigationInfo(Guid navigationId, string viewModelCoreNoSpaces, string query)
    {
      if (navigationId == Guid.Empty)
        throw new ArgumentException("navigationId is Guid.Empty");

      NavigationId = navigationId;
      ViewModelCoreNoSpaces = viewModelCoreNoSpaces;
      var address = AppResources.BaseAddress + "/" + ViewModelCoreNoSpaces;
      if (!string.IsNullOrEmpty(query))
        address += "?" + query;
      Uri = new Uri(address, UriKind.Absolute);
    }
    /// <summary>
    /// Navigation requested event message, for navigating to view models using convention over configuration.
    /// EXPECTED FORMAT: [CoreText]ViewModel
    /// EXPECTED FORMAT: QueryString[key=PropertyName, value=PropertyValueAsString] (not implemented yet)
    /// </summary>
    /// <param name="requestedViewModelCoreText">e.g. core = Login for LoginViewModel, core=AddNew for AddNewViewModel</param>
    /// <param name="queryString"></param>
    public NavigationInfo(Guid navigationId, 
                          string viewModelCoreNoSpaces, 
                          IDictionary<string, string> queryEntries)
    {
      if (queryEntries == null || queryEntries.Count == 0)
        throw new ArgumentNullException(
          "queryEntries == null || count == 0.  There does exist another overload without queryEntries.");

      NavigationId = navigationId;
      ViewModelCoreNoSpaces = viewModelCoreNoSpaces;

      var address = AppResources.BaseAddress + "/" + ViewModelCoreNoSpaces;
      string queryStr = "";
      bool firstEntry = true;
      foreach (var entry in queryEntries)
      {
        if (firstEntry)
          queryStr += entry.Key + "=" + entry.Value;
        else
          queryStr += "&" + entry.Key + "=" + entry.Value;
      }
      Uri = new Uri(address + "?" + queryStr, UriKind.Absolute);
    }

    public Guid NavigationId { get; private set; }
    public string ViewModelCoreNoSpaces { get; private set; }
    public Uri Uri { get; private set; }
  }
}

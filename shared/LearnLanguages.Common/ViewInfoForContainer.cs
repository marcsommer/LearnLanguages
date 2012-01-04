using System;

namespace LearnLanguages
{
  public class ViewInfoForContainer
  {
    public ViewInfoForContainer(Type serviceType, string key)
    {
      this.ServiceType = serviceType;
      this.Key = key;
    }

    public Type ServiceType { get; private set; }
    public string Key { get; private set; }
  }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LearnLanguages.Common.EventArgs
{
  public class ModelEventArgs<TModel> : System.EventArgs
  {
    public ModelEventArgs(TModel model)
    {
      Model = model;
    }

    public TModel Model { get; private set; }
  }
}

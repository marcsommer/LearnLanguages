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
using System.ComponentModel.Composition;

namespace LearnLanguages.DataAccess.WcfProxy
{
  [Export(typeof(IPhraseDalAsync))]
  public class PhraseDalProxy : IPhraseDalAsync
  {

    public Result<PhraseDto> New(object criteria)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Fetch(Guid id)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Update(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Insert(PhraseDto dto)
    {
      throw new NotImplementedException();
    }

    public Result<System.Collections.Generic.ICollection<PhraseDto>> GetAll()
    {
      throw new NotImplementedException();
    }

    public Result<PhraseDto> Delete(Guid id)
    {
      throw new NotImplementedException();
    }

  }
}

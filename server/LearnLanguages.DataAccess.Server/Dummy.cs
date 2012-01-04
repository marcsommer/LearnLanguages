using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.DataAccess.Server
{
  /// <summary>
  /// It wasn't working without an actual class (not link) in the library, but it compiles properly with this.
  /// Specifically, I had another project in this solution that would not for the life of it see the public 
  /// interface IPhraseDal, as it was not getting compiled.  I tried building, rebuilding, running.  The usual 
  /// just wasn't working, but adding a dummy class and then compiling worked, so I'm leaving it in.
  /// </summary>
  class Dummy
  {
  }
}

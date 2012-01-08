using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LearnLanguages.DataAccess.EFCodeFirst
{
  public class PhraseContext : DbContext
  {
    public DbSet<PhraseDto> Phrases { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
      //base.OnModelCreating(modelBuilder);
    }
  }
}

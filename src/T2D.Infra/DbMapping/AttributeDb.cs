using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
  public static class AttributeDb
  {
    public static void SetDbMapping(ModelBuilder modelBuilder)
    {
      var tbl = modelBuilder.Entity<T2D.Entities.Attribute>();

      tbl.Property(e => e.Name)
        .HasMaxLength(256)
        ;


    }
  }
}

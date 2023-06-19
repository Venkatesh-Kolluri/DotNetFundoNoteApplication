using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Db
{
    public class FundoContext : DbContext
    {
            public FundoContext(DbContextOptions options)
                : base(options)
            {
            }
            public DbSet<UserEntity> UserTable { get; set; }
            public DbSet<NotesEntity>NotesTable { get; set; }
            public DbSet<CollaboratorEntity> CollaboratorTable { get; set; }
    }
}

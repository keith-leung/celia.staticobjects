using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.StaticObjects.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Celia.io.Core.StaticObjects.DataAccess
{
    public class StaticObjectsDbContext : DbContext
    {
        public StaticObjectsDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<MigrationVersion> MigrationVersions { get; set; }

        public DbSet<ServiceApp> ServiceApps { get; set; }

        public DbSet<Storage> Storages { get; set; }

        public DbSet<ImageElement> ImageElements { get; set; }

        public DbSet<ImageElementTranItem> ImageElementTranItems { get; set; }

        public DbSet<ServiceAppStorageRelation> ServiceAppStorageRelations { get; set; }
    }
}

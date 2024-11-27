using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Datamodels.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Datamodels
{
    public class Context : PruebaContext
    {
        private readonly string connection;

        public Context(string connection, bool readOnly = false)
        {
            this.connection = connection;
            if (readOnly)
            {
                ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
#if DEBUG
            optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
#endif
        }
    }
}
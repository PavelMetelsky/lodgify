using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Database;

namespace VacationRental.Api.Tests
{
    public class FakeVRContext : VRContext
    {
        public FakeVRContext(DbContextOptions<VRContext> options)
            : base(options)
        {
        }

        public void UnloadDataFromMemory()
        {
            ChangeTracker.Entries()
                .Where(e => e.Entity != null)
                .ToList()
                .ForEach(e => e.State = EntityState.Detached);
        }
    }
}

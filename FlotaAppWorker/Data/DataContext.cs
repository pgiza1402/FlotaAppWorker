using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> AspNetUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarInsurance> CarInsurances { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<TechnicalExamination> TechnicalExaminations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }
    }
}

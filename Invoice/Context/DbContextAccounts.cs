using Invoice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Context
{
    public partial class DbContextAccounts : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=IEENN-MARS\\SAGE200;Initial Catalog=******;User ID=*****;Password=*****;Encrypt=False");
        }

        public DbSet<UpsRateCard> UpsRateCard { get; set; }

        public DbSet<TharsternDeliveries> TharstenDeliveries { get; set; }
    }
}

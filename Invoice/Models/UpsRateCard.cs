using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Models
{
    [Keyless]
    [Table("cpUPSRates")]
    public class UpsRateCard
    {

        public string? CC { get; set; }

        public double oneParcel { get; set; }

        public double twoParcels { get; set; }

        public double threeParcels { get; set; }

        public double fourParcels { get; set; }

        public double fiveParcels { get; set; }

        public double sixParcels { get; set; }

        public double sevenParcels { get; set; }

        public double eightParcels { get; set; }

        public double nineParcels { get; set; }

        public double tenParcels { get; set; }

    }
}

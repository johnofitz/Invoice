using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Models
{
    public class UpsInvoice
    {
        public string? InvoiceDownLoadDate { get; set; }

        public string? DeliveryDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? JobNumber { get; set; }
        public string? Parcels { get; set; }
        public string? BaseRate { get; set; }

    }
}

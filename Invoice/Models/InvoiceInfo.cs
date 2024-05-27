using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Invoice.Models
{
    public class InvoiceInfo
    {
        public string InvoiceType { get; set; }

        public string SelectedPath { get; set; }

        public string JobNumber { get; set; }


        public string BaseRate { get; set; }


        public string Quantity { get; set; }


        public string InvoiceNumber { get; set; }


        public string DeliveryDate { get; set; }


        public string DownloadDate { get; set; }

        public string SelectedOption { get; set; }



    }
}

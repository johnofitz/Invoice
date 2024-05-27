using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Models
{
    public class InvoiceReconcile
    {
        public string? TharDelNoteNumber { get; set; }
        public string? TharDeliveryName { get; set; }
        public string? TharDeliveryContact { get; set; }
        public DateTime TharDelivereyDate { get; set; }
        public DateTime TharReqDate { get; set; }
        public string? TharJob { get; set; }
        public string? TharCountry { get; set; }
        public string? TharDelConsignType { get; set; }
        public string? TharConsignType { get; set; }
        public string? TharQuantity { get; set; }
        public string? TharLocation { get; set; }
        public string? UpsInvoiceDownLoadDate { get; set; }
        public string? UpsDeliveryDate { get; set; }
        public string? UpsInvoiceNumber { get; set; }
        public string? UpsJobNumber { get; set; }
        public string? UpsParcels { get; set; }
        public string? UpsBaseRate { get; set; }
        public string? RateCard { get; set; }
    }
}

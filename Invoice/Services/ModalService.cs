using Invoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Services
{
    public class ModalService
    {
        public Guid Guid = Guid.NewGuid();
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        private FileService load = new();

        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "Show";
            ShowBackdrop = true;
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
        }


        public async void GetDefaults(InvoiceInfo invoiceInfo)
        {

            var columns = await load.GetColumns();

            if (invoiceInfo.SelectedOption == "UPS")
            {

                var tempJob = columns.ElementAt(15);
                invoiceInfo.JobNumber = tempJob.Column;

                var tempParc = columns.ElementAt(18);
                invoiceInfo.Quantity = tempParc.Column;

                var tempBase = columns.ElementAt(52);
                invoiceInfo.BaseRate = tempBase.Column;

                var tempInvoiceNum = columns.ElementAt(5);
                invoiceInfo.InvoiceNumber = tempInvoiceNum.Column;

                var tempDelDate = columns.ElementAt(11);
                invoiceInfo.DeliveryDate = tempDelDate.Column;

                var tempDownDate = columns.ElementAt(4);
                invoiceInfo.DownloadDate = tempDownDate.Column;

            }
            else
            {
                var tempJob = columns.ElementAt(18);
                invoiceInfo.JobNumber = tempJob.Column;
                var tempParc = columns.ElementAt(18);
                invoiceInfo.Quantity = tempParc.Column;
                var tempBase = columns.ElementAt(52);
                invoiceInfo.BaseRate = tempBase.Column;
                var tempInvoiceNum = columns.ElementAt(5);
                invoiceInfo.InvoiceNumber = tempInvoiceNum.Column;
                var tempDelDate = columns.ElementAt(11);
                invoiceInfo.DeliveryDate = tempDelDate.Column;
                var tempDownDate = columns.ElementAt(4);
                invoiceInfo.DownloadDate = tempDownDate.Column;
            }
        }
    }
}

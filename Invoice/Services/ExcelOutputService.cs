using Invoice.Context;
using Invoice.Models;


namespace Invoice.Services
{
    public class ExcelOutputService
    {
        public InvoiceInfo? invoiceInfo =new();

        private List<InvoiceReconcile> Item = new();

        private FileService load = new();
        public void Reconcile(List<int> excelPositions, InvoiceInfo invoice)
        {
            var _joinList = new List<InvoiceReconcile>();
            DbContextAccounts contextFinance = new DbContextAccounts();
            string tempCountry = "";
            string tempJob = "";
            string tempQuantity = "";
            string? fileDate = "";

            if (invoice.InvoiceType == "UPS" && invoice.SelectedPath is not null)
            {
                ExcelProcessingService excelProcessor = new();
                var _upsRates = contextFinance.UpsRateCard.ToList();
                var _tharstenDeliveries = contextFinance.TharstenDeliveries.ToList();
                var _invoices = excelProcessor.ProcessUpsExcel(invoice.SelectedPath, excelPositions, invoice.InvoiceType);

                foreach (var x in _invoices)
                {
                    bool foundMatch = false;

                    foreach (var y in _tharstenDeliveries)
                    {
                        foreach (var z in _upsRates)
                        {
                            if (x.JobNumber == y.OrderNo)
                            {
                                if (y.DeliveryCountry.ToUpper() == z.CC.ToUpper())
                                {
                                    var parcelRate = GetUpsRates(z.CC, x.Parcels);

                                    var newJoin = new InvoiceReconcile
                                    {
                                        TharCountry = y.DeliveryCountry,
                                        TharJob = y.OrderNo,
                                        UpsJobNumber = x.JobNumber,
                                        TharQuantity = y.NumberOfItems,
                                        UpsParcels = x.Parcels,
                                        UpsBaseRate = x.BaseRate,
                                        RateCard = parcelRate


                                    };
                                    fileDate = x.InvoiceDownLoadDate;
                                    Item.Add(newJoin);
                                    foundMatch = true;
                                    break; // Once a match is found, no need to continue looping
                                }
                                else
                                {
                                    tempCountry = y.DeliveryCountry.ToUpper();
                                    tempJob = y.OrderNo;
                                    tempQuantity = y.NumberOfItems;
                                }

                            }
                        }

                    }

                    if (!foundMatch)
                    {
                        var noJoin = new InvoiceReconcile
                        {
                            TharCountry = "Not Found",
                            TharJob = "Not Found",
                            UpsJobNumber = x.JobNumber,
                            TharQuantity = "Mot Found",
                            UpsParcels = x.Parcels,
                            UpsBaseRate = x.BaseRate,
                            RateCard = "Not Found"
                        };
                        Item.Add(noJoin);
                    }


                }
            }
        }

        private string GetUpsRates(string country, string parcel)
        {
            DbContextAccounts contextFinance = new();
            var _upsRates = contextFinance.UpsRateCard.ToList();
            string parc = "";
            foreach (var item in _upsRates)
            {
                if (parcel == "1" && country == item.CC)
                {
                    parc = item.oneParcel.ToString();
                }
                if (parcel == "2" && country == item.CC)
                {
                    parc = item.twoParcels.ToString();
                }
                if (parcel == "3" && country == item.CC)
                {
                    parc = item.threeParcels.ToString();

                }
                if (parcel == "4" && country == item.CC)
                {
                    parc = item.fourParcels.ToString();
                }
                if (parcel == "5" && country == item.CC)
                {
                    parc = item.fiveParcels.ToString();
                }
                if (parcel == "6" && country == item.CC)
                {
                    parc = item.sixParcels.ToString();
                }
                if (parcel == "7" && country == item.CC)
                {
                    parc = item.sevenParcels.ToString();
                }
                if (parcel == "8" && country == item.CC)
                {
                    parc = item.eightParcels.ToString();
                }
                if (parcel == "9" && country == item.CC)
                {
                    parc = item.nineParcels.ToString();
                }
                if (parcel == "10" && country == item.CC)
                {
                    parc = item.eightParcels.ToString();
                }

            }
            return parc;
        }

        private async Task<List<int>> GetFields()
        {
            var excelPositions = new Dictionary<string, int>();
            var jsonExcelMap = await load.GetColumns();

            foreach (var item in jsonExcelMap)
            {
                if (item.Column == invoiceInfo?.JobNumber.ToUpper() || item.Column == invoiceInfo?.BaseRate.ToUpper() || item.Column == invoiceInfo?.Quantity.ToUpper() || item.Column == invoiceInfo?.InvoiceNumber.ToUpper() || item.Column == invoiceInfo?.DownloadDate.ToUpper() || item.Column == invoiceInfo?.DeliveryDate.ToUpper())
                {
                    excelPositions[item.Column] = item.Value;
                }
            }

            // Extract values in the order of the input parameters
            var result = new List<int>();
            result.Add(excelPositions.ContainsKey(invoiceInfo.JobNumber) ? excelPositions[invoiceInfo.JobNumber] : -1); // If not found, add -1 or handle it accordingly
            result.Add(excelPositions.ContainsKey(invoiceInfo.BaseRate) ? excelPositions[invoiceInfo.BaseRate] : -1);
            result.Add(excelPositions.ContainsKey(invoiceInfo.Quantity) ? excelPositions[invoiceInfo.Quantity] : -1);
            result.Add(excelPositions.ContainsKey(invoiceInfo.InvoiceNumber) ? excelPositions[invoiceInfo.InvoiceNumber] : -1);
            result.Add(excelPositions.ContainsKey(invoiceInfo.DownloadDate) ? excelPositions[invoiceInfo.DownloadDate] : -1);
            result.Add(excelPositions.ContainsKey(invoiceInfo.DeliveryDate) ? excelPositions[invoiceInfo.DeliveryDate] : -1);
            result.Sort();
            return result;
        }

    }
}

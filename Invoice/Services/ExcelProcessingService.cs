using ClosedXML.Excel;
using Invoice.Models;
using System.Text.RegularExpressions;


namespace Invoice.Services
{
    public partial class ExcelProcessingService
    {
        protected int JobIndex;
        protected int BaseRateIndex;
        protected int QuantiityIndex;
        protected int InvoiceNoIndex;
        protected int DownloadDateIndex;
        protected int DeliveryDateIndex;

        public List<UpsInvoice> ProcessUpsExcel(string filePath, List<int> cellNumbers, string invoiceType)
        {
            List<UpsInvoice> dataList = new();
            // Load Excel file using ClosedXML
            using (XLWorkbook workbook = new(filePath))
            {
                // Assuming its the first worksheet
                IXLWorksheet worksheet = workbook.Worksheet(1);

                // Determine the column index for each required column with ups
                // if UPS ever changes these will need to be altered
                if (invoiceType == "UPS")
                {
                    DownloadDateIndex = cellNumbers[0];
                    InvoiceNoIndex = cellNumbers[1];
                    DeliveryDateIndex = cellNumbers[2];
                    JobIndex = cellNumbers[3];
                    QuantiityIndex = cellNumbers[4];
                    BaseRateIndex = cellNumbers[5];
                }

                // Get the count of used rows
                int rowCount = worksheet.RowsUsed().Count();
                // Excel is not 0th index 
                // Dictionary to store job number as key and corresponding UpsInvoice object as value
                Dictionary<int, UpsInvoice> jobDictionary = new();

                // Excel is not 0th index 
                for (int row = 1; row <= rowCount; row++)
                {
                    string jobNumberCell = worksheet.Cell(row, JobIndex).Value.ToString();

                    if (invoiceType == "UPS")
                    {
                        // Check if the job number cell contains a number
                        if (MyRegex().IsMatch(jobNumberCell))
                        {
                            // Extract only the numeric part of the job number
                            int jobNumber = int.Parse(MyRegex().Match(jobNumberCell).Value);

                            // Create or update UpsInvoice object in the dictionary based on parcel count
                            UpsInvoice invoice = new()
                            {
                                InvoiceDownLoadDate = worksheet.Cell(row, DownloadDateIndex).Value.ToString(),
                                InvoiceNumber = worksheet.Cell(row, InvoiceNoIndex).Value.ToString(),
                                DeliveryDate = worksheet.Cell(row, DeliveryDateIndex).Value.ToString(),
                                JobNumber = jobNumber.ToString(),
                                Parcels = worksheet.Cell(row, QuantiityIndex).Value.ToString(),
                                BaseRate = worksheet.Cell(row, BaseRateIndex).Value.ToString()
                            };

                            if (!jobDictionary.ContainsKey(jobNumber))
                            {
                                jobDictionary.Add(jobNumber, invoice);
                            }
                            else
                            {
                                // If job number already exists, compare parcel counts and update if current parcel count is higher
                                if (int.Parse(invoice.Parcels) > int.Parse(jobDictionary[jobNumber].Parcels))
                                {
                                    jobDictionary[jobNumber] = invoice;
                                }
                            }
                        }
                    }
                }

                // Add all UpsInvoice objects from the dictionary to dataList
                dataList.AddRange(jobDictionary.Values);

            }
            return dataList;
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex MyRegex();





        public void OutputToExcel(List<InvoiceReconcile> dataList, string filePath)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("UpsInvoices");

            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 20;
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;
            worksheet.Column(7).Width = 20;

            // Add headers
            worksheet.Cell(1, 1).Value = "Delivery Country";
            worksheet.Cell(1, 2).Value = "Thar JobNo";
            worksheet.Cell(1, 3).Value = "UPS JobNo";
            worksheet.Cell(1, 4).Value = "Thar Parcels";
            worksheet.Cell(1, 5).Value = "UPS Parcels";
            worksheet.Cell(1, 6).Value = "UPS Base Rate";
            worksheet.Cell(1, 7).Value = "Rate Card";

            // Populate data
            int row = 2;
            foreach (InvoiceReconcile invoice in dataList)
            {
                var dataRow = worksheet.Row(row);
                dataRow.Cell(1).Value = invoice.TharCountry;
                dataRow.Cell(2).Value = invoice.TharJob;
                dataRow.Cell(3).Value = invoice.UpsJobNumber;
                dataRow.Cell(4).Value = invoice.TharQuantity;
                dataRow.Cell(5).Value = invoice.UpsParcels;
                dataRow.Cell(6).Value = invoice.UpsBaseRate;
                dataRow.Cell(7).Value = invoice.RateCard;

                // Check if Thar JobNo and UPS JobNo are equal
                if (invoice.TharJob == invoice.UpsJobNumber)
                    dataRow.Cell(2).Style.Fill.BackgroundColor = XLColor.Green; // Add green tick
                else
                    dataRow.Cell(2).Style.Fill.BackgroundColor = XLColor.Red; // Add red tick

                row++;
            }

            // Apply filters and corrections
            FilterAndCorrect(worksheet);

            // Save the Excel file
            workbook.SaveAs(filePath);
        }

        public static void FilterAndCorrect(IXLWorksheet worksheet)
        {
            // Apply filters
            worksheet.RangeUsed().SetAutoFilter();

            // Check condition for each row
            foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
            {
                var tharJobNo = row.Cell(2).Value.ToString();
                var upsJobNo = row.Cell(3).Value.ToString();
                var checkCell = row.Cell(8); // Assuming you want to check row 8

                // Check if Thar JobNo and UPS JobNo are equal
                if (tharJobNo == upsJobNo)
                    checkCell.Value = "✓"; // Add a tick mark
                else
                    checkCell.Value = "✗"; // Add a cross mark
            }
        }
    }
}

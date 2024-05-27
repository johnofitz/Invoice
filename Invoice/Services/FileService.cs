using Invoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
#nullable disable

namespace Invoice.Services
{
    public class FileService
    {
        private string _file;
        public async Task<string> FilePick()
        {
            
            try
            {
                // Define allowed file types for Excel files
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "com.microsoft.excel.xls", "org.openxmlformats.spreadsheetml.sheet" } }, // iOS UTI
                    { DevicePlatform.Android, new[] { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" } }, // MIME types
                    { DevicePlatform.WinUI, new[] { ".xls", ".xlsx" } }, // File extensions
                    { DevicePlatform.MacCatalyst, new[] { "com.microsoft.excel.xls", "org.openxmlformats.spreadsheetml.sheet" } } // MacCatalyst UTI
                });

                var options = new PickOptions
                {
                    FileTypes = customFileType
                };

                // Show the file picker dialog
                var result = await FilePicker.PickAsync(options);

                if (result != null)
                {
                    // Get the full file path
                    _file = result.FullPath;

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }

            return _file;
        }

        public async Task GoToSelectionAsync(string selectedPath, string selectedOption)
        {
            try
            {
                if (!File.Exists(selectedPath))
                {
                    await Shell.Current.DisplayAlert("Error", "The selected file path is invalid.", "OK");
                    return;
                }
                else if (string.IsNullOrEmpty(selectedOption))
                {
                    await Shell.Current.DisplayAlert("Error", "Invoice company is blank.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        List<ColumnMap> columnMap = new();

        public async Task<List<ColumnMap>> GetColumns()
        {
            // Condition to check if menu is already loaded and not null
            if (columnMap?.Count > 0)
                return columnMap;

            // OpenAppPackageFileAsync as file is on application
            using var stream = await FileSystem.OpenAppPackageFileAsync("excelmap.json");
            using var reader = new StreamReader(stream);
            var items = await reader.ReadToEndAsync();
            columnMap = JsonSerializer.Deserialize<List<ColumnMap>>(items);

            return columnMap;
        }
    }
}

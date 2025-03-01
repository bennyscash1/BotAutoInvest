using OpenQA.Selenium.DevTools.V118.IndexedDB;

namespace InvesAuto.Infra.InfraCommonService
{
    public class InfraFileService
    {
        public async Task UpdateCSVFile(string filePath, params object[] values)
        {
            string newLine = string.Join(',', values.Select(x => x.ToString())) + Environment.NewLine;

            // Check if the file exists and read its content
            string existingContent = File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : string.Empty;

            // Prepend the new line to the existing content
            string updatedContent = newLine + existingContent;

            // Overwrite the file with the updated content
            await File.WriteAllTextAsync(filePath, updatedContent);
        }
        public bool IsDataExistInCsvFile(string valueToTest, string fileFullPath)
        {
            bool stringExists = false;
            try
            {
                // Open the CSV file and read it line by line
                using (StreamReader reader = new StreamReader(fileFullPath))
                {
                    string line;

                    // Read each line of the CSV file
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Check if the line contains the test value
                        if (line.Contains(valueToTest))
                        {
                            stringExists = true;
                            break; // Exit the loop as soon as we find the value
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return stringExists;
        }
        public string GetCompanyStockFilePath()
        {
            string currentPath = Directory.GetCurrentDirectory();
            string indexCompanyPath = Path.Combine(currentPath, "GeneralFiles", "CompanyNames.csv");
            return indexCompanyPath;
        }
        public string GetCsvValue(string filePath, string cellAddress)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }

            // Convert cellAddress (e.g., "A2") to row and column indexes
            int columnIndex = cellAddress[0] - 'A'; // 'A' → 0, 'B' → 1, 'C' → 2, etc.
            int rowIndex = int.Parse(cellAddress.Substring(1)) - 1; // Convert to zero-based index

            using (var reader = new StreamReader(filePath))
            {
                for (int i = 0; i <= rowIndex; i++)
                {
                    var line = reader.ReadLine();
                    if (line == null) return string.Empty; // Row out of bounds

                    if (i == rowIndex)
                    {
                        var values = line.Split(','); // Adjust delimiter if needed
                        return columnIndex < values.Length ? values[columnIndex].Trim() : string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        public static async Task<bool> ReadAndUpdateCSVFile(string filePath, Dictionary<string, string> values)
        {
            try
            {
                List<string[]> rows = new List<string[]>();
                bool fileExists = File.Exists(filePath);

                if (fileExists)
                {
                    // Read all lines from the existing file
                    string[] lines = await File.ReadAllLinesAsync(filePath);
                    foreach (var line in lines)
                    {
                        rows.Add(line.Split(',')); // Convert CSV lines to lists
                    }
                }

                // If file does not exist, create headers

                else 
                {
                    foreach (var key in values.Keys)
                    {
                        rows.Add(new[] { key });
                    }
                }

                bool updated = false;

                foreach (var entry in values)
                {
                    string key = entry.Key;
                    string newValue = entry.Value;

                    // Find the row where the first column matches the key
                    var row = rows.FirstOrDefault(r => r.Length > 0 && r[0] == key);

                    if (row != null)
                    {
                        // Find the first empty column in the row (excluding the key column)
                        int emptyIndex = Array.FindIndex(row, 1, col => string.IsNullOrWhiteSpace(col));

                        if (emptyIndex != -1)
                        {
                            row[emptyIndex] = newValue;
                        }
                        else
                        {
                            // If the row is full, add new data in a new column
                            List<string> newRow = row.ToList();
                            newRow.Add(newValue);
                            rows[rows.IndexOf(row)] = newRow.ToArray();
                        }
                    }
                    else
                    {
                        // If the key does not exist, add a new row
                        List<string> newRow = new List<string> { key, newValue };
                        while (newRow.Count < rows[0].Length) newRow.Add(""); // Ensure correct column count
                        rows.Add(newRow.ToArray());
                    }

                    updated = true;
                }

                // Convert list back to CSV format
                List<string> updatedLines = new List<string>();
                foreach (var row in rows)
                {
                    updatedLines.Add(string.Join(",", row));
                }

                // Write updated content back to file
                await File.WriteAllLinesAsync(filePath, updatedLines);

                return updated;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public int GetCsvRowsIntValue(string csvPath, string column = "A")
        {
            Console.WriteLine("GetCsvRowsIntValue!!!");
            if (!File.Exists(csvPath))
            {
                Console.WriteLine("CSV file not found.");
                throw new FileNotFoundException("CSV file not found.");
            }

            // Read all lines from the CSV file
            var lines = File.ReadAllLines(csvPath);

            // Convert column letter to index (A = 0, B = 1, etc.)
            int columnIndex = column.ToUpper()[0] - 'A';

            // Count non-empty lines in the specified column
            int nonEmptyCount = lines
                .Select(line => line.Split(',')) // Split by comma
                .Where(cells => cells.Length > columnIndex && !string.IsNullOrWhiteSpace(cells[columnIndex]))
                .Count();

            return nonEmptyCount;
        }
    }

}








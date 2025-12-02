
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Document = QuestPDF.Fluent.Document;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DummyConsoleApp.EpubProject
{
    internal class PdfCreator(string title, string imagesFolderDirectory)
    {
        public void CreatePdfFiles()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            if (string.IsNullOrEmpty(imagesFolderDirectory) || !Directory.Exists(imagesFolderDirectory))
            {
                throw new DirectoryNotFoundException("The specified images folder directory does not exist.");
            }

            var folders = Directory.GetDirectories(imagesFolderDirectory);

            foreach (var folder in folders)
            {
                CreatePdfFile(folder);
            }
        }

        private void CreatePdfFile(string imagesFolder)
        {
            var filePath = imagesFolder + ".pdf";
            var imageFiles = new SortedDictionary<string, string>(
                Directory.GetFiles(imagesFolder, "*.png").ToDictionary(GetFormattedFileNameForSort)
            );
            if (File.Exists(filePath))
                File.Delete(filePath);
            Document.Create(container =>
            {
                foreach (var imagePath in imageFiles.Values)
                {
                    container.Page(page =>
                    {
                        page.Margin(0);
                        page.Content().Element(container =>
                        {
                            var imageDescriptor = container.Image(imagePath);
                            imageDescriptor.FitArea();
                        });
                    });
                }
            }).GeneratePdf(filePath);

            Console.WriteLine($"PDF ${Path.GetFileName(filePath)} created successfully!");
        }
        private static string GetFormattedFileNameForSort(string fileNameRaw)
        {
            var fileName = Path.GetFileName(fileNameRaw);
            var match = Regex.Match(fileName, @"\((\d+)\)");
            if (match.Success)
            {
                var number= match.Groups[1].Value;
                if (number.Length == 1)
                    return "0" + number;
                return number;
            }

            if (Regex.IsMatch(fileName, @"^[0-9]\.png"))
                fileName = "0" + fileName;

            return fileName;
        }
    }
}

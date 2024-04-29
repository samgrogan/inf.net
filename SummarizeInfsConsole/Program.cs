using INF.Net;
using InfNet;
using InfNet.Models.Public;
using SummarizeInfsConsole.Models;
using System.Collections.Concurrent;
using System.Text;

namespace SummarizeInfsConsole {
    internal class Program {
        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                // Init the output
                StringBuilder output = new();

                SummarizeDirectory(arguments.SourceFolder, output);
                File.WriteAllText(arguments.OutputPath, output.ToString());
            }
        }

        static void SummarizeDirectory(string sourceFolder, StringBuilder output) {
            // Get the list of files in the source directory
            string[] fileNames = Directory.GetFiles(sourceFolder);

            foreach (string fileName in fileNames) {
                Console.WriteLine($"{fileName}...");
                // Parse the INF
                InfFile infFile = InfDeserializer.DeserializeFromFile(fileName);
                // Extract the summary
                List<InfOsDeviceDriver> summary = InfSummarizer.SummaryizeInfFile(infFile);

                // Print out the summary
                OutputSummary(summary, output);
            }

            // Recurse through the child folders
            string[] directories = Directory.GetDirectories(sourceFolder);
            foreach (string directory in directories) {
                SummarizeDirectory(directory, output);
            }
        }

        static void OutputSummary(List<InfOsDeviceDriver> summary, StringBuilder output) {
            foreach (InfOsDeviceDriver summaryItem in summary) {
                output.AppendLine($"\"{summaryItem.File.FileName}\",\"{summaryItem.Os}\",\"{summaryItem.DriverVersion}\",\"{summaryItem.DeviceName}\",\"{summaryItem.DeviceId}\"");
            }
        }
    }
}

using INF.Net;
using InfNet;
using InfNet.Models.Public;
using SummarizeInfsConsole.Models;
using System.Collections.Concurrent;

namespace SummarizeInfsConsole {
    internal class Program {
        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                // Init the output
                ConcurrentBag<SummaryRow> summaryRows = new();

                SummarizeDirectory(arguments.SourceFolder);
            }
        }

        static void SummarizeDirectory(string sourceFolder) {
            // Get the list of files in the source directory
            string[] fileNames = Directory.GetFiles(sourceFolder);

            foreach (string fileName in fileNames) {
                Console.WriteLine($"{fileName}...");
                // Parse the INF
                InfFile infFile = InfDeserializer.DeserializeFromFile(fileName);
                // Extract the summary
                List<InfOsDeviceDriver> summary = InfSummarizer.SummaryizeInfFile(infFile);
            }

            // Recurse through the child folders
            string[] directories = Directory.GetDirectories(sourceFolder);
            foreach (string directory in directories) {
                SummarizeDirectory(directory);
            }
        }
    }
}

using InfNet;
using InfNet.Models.Public;
using SummarizeInfsConsole.Models;
using SummarizeInfsConsole.Models.Enums;
using System;
using System.Net.Http;
using System.Text;

namespace SummarizeInfsConsole {
    internal class Program {

        public static StringBuilder Output = new();

        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                // Act on the given source type
                switch (arguments.SourceType) {
                    case SourceType.Directory:
                        SummarizeDirectory(arguments.Source);
                        break;
                    case SourceType.InfFile:
                        SummarizeInfFile(arguments.Source);
                        break;
                    case SourceType.ArchiveFile:
                        InfExtractor.ExtractInfFilesFromFileToDelegate(arguments.Source, SummarizeInfStream);
                        break;
                    case SourceType.InfUrl:
                    case SourceType.ArchiveUrl: {
                            // Try and download the file to a stream
                            using HttpClient httpClient = new();
                            using HttpRequestMessage requestMessage = new(HttpMethod.Get, arguments.Source);
                            using HttpResponseMessage response = httpClient.Send(requestMessage, HttpCompletionOption.ResponseHeadersRead);
                            using Stream rawResponseStream = response.Content.ReadAsStream();

                            if (arguments.SourceType == SourceType.InfUrl) {
                                SummarizeInfStream(arguments.Source, rawResponseStream);
                            } else {
                                InfExtractor.ExtractInfFilesFromStreamToDelegate(arguments.Source, rawResponseStream, SummarizeInfStream);
                            }
                        }
                        break;
                }

                File.WriteAllText(arguments.OutputPath, Output.ToString());
            }
        }

        static void SummarizeDirectory(string sourceFolder) {
            // Get the list of files in the source directory
            string[] fileNames = Directory.GetFiles(sourceFolder);

            foreach (string fileName in fileNames) {
                SummarizeInfFile(fileName);
            }

            // Recurse through the child folders
            string[] directories = Directory.GetDirectories(sourceFolder);
            foreach (string directory in directories) {
                SummarizeDirectory(directory);
            }
        }

        static void SummarizeInfFile(string fileName) {
            Console.WriteLine($"{fileName}...");
            // Parse the INF
            InfFile infFile = InfDeserializer.DeserializeFromFile(fileName);
            // Extract the summary
            List<InfOsDeviceDriver> summary = InfSummarizer.SummaryizeInfFile(infFile);

            // Print out the summary
            OutputSummary(summary);
        }

        static void SummarizeInfStream(string fileName, Stream stream) {
            Console.WriteLine($"{fileName}...");
            // Parse the INF
            InfFile infFile = InfDeserializer.DeserializeFromStream(fileName, stream);
            // Extract the summary
            List<InfOsDeviceDriver> summary = InfSummarizer.SummaryizeInfFile(infFile);

            // Print out the summary
            OutputSummary(summary);
        }

        static void OutputSummary(List<InfOsDeviceDriver> summary) {
            foreach (InfOsDeviceDriver summaryItem in summary) {
                Output.AppendLine($"\"{summaryItem.File.FileName}\",\"{summaryItem.Os}\",\"{summaryItem.DriverVersion}\",\"{summaryItem.DeviceName}\",\"{summaryItem.DeviceId}\"");
            }
        }
    }
}

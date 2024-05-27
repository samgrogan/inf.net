using System;
using System.IO;
using InfNet.Models.Internal;
using InfNet.Models.Public;

namespace InfNet {
    public class InfExtractor {
        #region Public Methods

        // Extract INF files from a local archive file and write them to a local directory
        public static bool ExtractInfFilesFromFileToDirectory(string fileName, string outputFolder) {
            // Make sure the output folder exists
            if (!Directory.Exists(outputFolder)) {
                Console.WriteLine($"'{outputFolder}' does not exist.");
                return false;
            }

            InfArchiveExtractor extractor = new();
            return extractor.ExtractInfFilesFromFileToDirectory(fileName, outputFolder);
        }

        // Extract INF files from a stream and write them to a local directory
        public static bool ExtractInfFilesFromStreamToDirectory(string fileName, Stream stream, string outputFolder) {
            // Make sure the output folder exists
            if (!Directory.Exists(outputFolder)) {
                Console.WriteLine($"'{outputFolder}' does not exist.");
                return false;
            }

            InfArchiveExtractor extractor = new();
            return extractor.ExtractInfFilesFromStreamToDirectory(fileName, outputFolder, stream);
        }

        // Extract INF files from a local archive file and pass them to a delegate for processing
        public static bool ExtractInfFilesFromFileToDelegate(string fileName, ProcessInfStream processInfStream) {
            InfArchiveExtractor extractor = new();
            return extractor.ExtractInfFilesFromFileToDelegate(fileName, processInfStream);
        }

        // Extract INF files from a stream and pass them to a delegate for processing
        public static bool ExtractInfFilesFromStreamToDelegate(string fileName, Stream stream, ProcessInfStream processInfStream) {
            InfArchiveExtractor extractor = new();
            return extractor.ExtractInfFilesFromStreamToDelegate(fileName, stream, processInfStream);
        }

        #endregion Public Methods
    }
}

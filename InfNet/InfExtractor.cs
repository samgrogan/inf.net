using System;
using System.IO;
using InfNet.Models;

namespace InfNet {
    public class InfExtractor {
        #region Public Methods

        public static bool ExtractInfFiles(string fileName, string outputFolder) {
            if (!IsFileSupported(fileName)) {
                Console.WriteLine($"'{Path.GetFileName(fileName)}' is not supported.");
                return false;
            }
            if (!Directory.Exists(outputFolder)) {
                Console.WriteLine($"'{outputFolder}' does not exist.");
                return false;
            }

            // Create the extractor for this file type
            InfFileExtractorBase fileExtractor = CreateFileExtractor(fileName, outputFolder);
            return fileExtractor.ExtractInfFiles();
        }

        #endregion Public Methods

        #region Internal Methods

        // Is the given file supported?
        private static bool IsFileSupported(string fileName) {
            // Does the file exist?
            if (!File.Exists(fileName)) {
                return false;
            }

            // Is this a supported file type
            string? extension = Path.GetExtension(fileName)?.ToLower();
            return extension switch {
                Constants.ExeFileExtension or
                Constants.ZipFileExtension or Constants.InfFileExtension
                  => true,
                _ => false,
            };
        }

        // Creates the file extractor for the given file
        private static InfFileExtractorBase CreateFileExtractor(string fileName, string outputFolder) {
            string? extension = Path.GetExtension(fileName)?.ToLower();

            return extension switch {
                Constants.ExeFileExtension => new InfExeExtractor(fileName, outputFolder),
                Constants.ZipFileExtension => new InfZipExtractor(fileName, outputFolder),
                _ => throw new($"'{Path.GetFileName(fileName)}' is not supported.")
            };
        }

        #endregion Internal Methods
    }
}

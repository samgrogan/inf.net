using InfNet.Helpers;
using InfNet.Models.Public;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;

namespace InfNet.Models.Internal {
    internal class InfArchiveExtractor {
        #region Properties

        #endregion Properties

        #region Public Methods

        public InfArchiveExtractor() {
        }

        public bool IsSupportedFileType(string fileName) {
            switch (PathHelpers.SafePathGetExtension(fileName)) {
                case Constants.ZipFileExtension:
                case Constants.ExeFileExtension:
                    return true;
            }
            return false;
        }

        // Extract INF files from the given local file path to the given local directory
        public bool ExtractInfFilesFromFileToDirectory(string fileName, string outputFolder) {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException(nameof(fileName));
            }
            using FileStream archiveStream = File.OpenRead(fileName);
            return ExtractInfFilesFromStreamToDirectory(fileName, outputFolder, archiveStream);
        }

        // Extract INF files from the given local file path and pass them to the given delegate for processing
        public bool ExtractInfFilesFromFileToDelegate(string fileName, ProcessInfStream processInfStream) {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException(nameof(fileName));
            }
            using FileStream archiveStream = File.OpenRead(fileName);
            return ExtractInfFilesFromStreamToDelegate(fileName, archiveStream, processInfStream);
        }

        // Extract INF files from the given stream and write them to a local directory
        public bool ExtractInfFilesFromStreamToDirectory(string fileName, string outputFolder, Stream archiveStream, string? parentFolder = null) {
            try {
                using ZipArchive zipArchive = new(archiveStream, ZipArchiveMode.Read);
                foreach (ZipArchiveEntry entry in zipArchive.Entries) {
                    // Is this an INF file
                    if (entry.FullName.EndsWith(Constants.InfFileExtension, StringComparison.OrdinalIgnoreCase)) {
                        string outputPath = PrepareOutputDirectoryForFile(entry.FullName, parentFolder, outputFolder);
                        Console.WriteLine($"\t\t{outputPath}: {entry.FullName}");
                        entry.ExtractToFile(outputPath, true);
                    }
                    // Process recursively, if supported
                    if (IsSupportedFileType(entry.FullName)) {
                        using Stream entryStream = entry.Open();
                        string childFolder = Path.Combine(Path.GetDirectoryName(entry.FullName) ?? string.Empty, Path.GetFileNameWithoutExtension(entry.FullName));
                        ExtractInfFilesFromStreamToDirectory(fileName, outputFolder, entryStream, childFolder);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        // Extract INF files from the given stream and pass them to the given delegate for processing
        public bool ExtractInfFilesFromStreamToDelegate(string fileName, Stream archiveStream, ProcessInfStream processInfStream) {
            try {
                using ZipArchive zipArchive = new(archiveStream, ZipArchiveMode.Read);
                foreach (ZipArchiveEntry entry in zipArchive.Entries) {
                    // Is this an INF file
                    if (entry.FullName.EndsWith(Constants.InfFileExtension, StringComparison.OrdinalIgnoreCase)) {
                        using Stream entryStream = entry.Open();
                        processInfStream(entry.FullName, entryStream);
                    }
                    // Process recursively, if supported
                    if (IsSupportedFileType(entry.FullName)) {
                        using Stream entryStream = entry.Open();
                        ExtractInfFilesFromStreamToDelegate(entry.FullName, entryStream, processInfStream);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        #endregion Public Methods

        #region Internal Methods

        // Prepares the given output directory
        protected static string PrepareOutputDirectoryForFile(string fileName, string? outputFolder) {
            return PrepareOutputDirectoryForFile(fileName, outputFolder, outputFolder);
        }

        protected static string PrepareOutputDirectoryForFile(string fileName, string? parentFolder, string? outputFolder) {
            if (string.IsNullOrEmpty(outputFolder)) {
                throw new ArgumentNullException(nameof(outputFolder));
            }
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException(nameof(fileName));
            }
            parentFolder ??= "/";

            string outputPath = Path.Combine(Path.Combine(outputFolder, parentFolder), fileName);
            string? directoryPath = Path.GetDirectoryName(outputPath);

            if (string.IsNullOrWhiteSpace(directoryPath)) {
                throw new("$Directory is blank.");
            }

            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            return outputPath;
        }

        #endregion Internal Methods
    }
}

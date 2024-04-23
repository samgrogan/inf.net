using InfNet.Models;
using System;
using System.IO;
using System.IO.Compression;


namespace InfNet {
    internal class InfZipExtractor : InfFileExtractorBase {

        #region Public Methods

        public InfZipExtractor(string fileName, string outputFolder) : base(fileName, outputFolder) {
        }

        public override bool ExtractInfFiles() {
            using FileStream zipFileStream = new FileStream(FileName, FileMode.Open);

            return ExtractInfFilesFromZipStream(zipFileStream);
        }

        #endregion Public Methods

        #region Internal Methods

        // Extract INF files from the given ZIP stream
        private bool ExtractInfFilesFromZipStream(Stream zipFileStream, string? parentFolder = null) {
            using ZipArchive zipArchive = new(zipFileStream, ZipArchiveMode.Read);

            foreach (ZipArchiveEntry entry in zipArchive.Entries) {

                // Is this an INF file
                if (entry.FullName.EndsWith(Constants.InfFileExtension, StringComparison.OrdinalIgnoreCase)) {
                    string outputPath = PrepareOutputDirectoryForFile(entry.FullName, parentFolder);
                    Console.WriteLine($"\t\t{outputPath}: {entry.FullName}");
                    entry.ExtractToFile(outputPath, true);
                }
                // Is this an embedded zip file?
                if (entry.FullName.EndsWith(Constants.ZipFileExtension, StringComparison.OrdinalIgnoreCase)) {
                    // Process recursively
                    string childFolder = Path.Combine(Path.GetDirectoryName(entry.FullName) ?? string.Empty, Path.GetFileNameWithoutExtension(entry.FullName));

                    using Stream entryStream = entry.Open();
                    ExtractInfFilesFromZipStream(entryStream, childFolder);
                }
            }
            return false;
        }

        #endregion Internal Methods
    }
}

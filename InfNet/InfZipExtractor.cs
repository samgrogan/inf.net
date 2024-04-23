using InfNet.Models;
using System;
using System.IO;
using System.IO.Compression;


namespace InfNet {
    internal class InfZipExtractor : InfFileExtractorBase {
        public InfZipExtractor(string fileName, string outputFolder) : base(fileName, outputFolder) {
        }

        public override bool ExtractInfFiles() {
            using ZipArchive zipArchive = ZipFile.OpenRead(FileName);

            foreach (ZipArchiveEntry entry in zipArchive.Entries) {
                if (entry.FullName.EndsWith(Constants.InfFileExtension, StringComparison.OrdinalIgnoreCase)) {
                    Console.WriteLine($"\t\t{entry.FullName}");
                    string outputPath = PrepareOutputDirectoryForFile(entry.FullName);
                    entry.ExtractToFile(outputPath, true);
                }
            }

            return false;
        }
    }
}

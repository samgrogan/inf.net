using System;
using System.IO;

namespace InfNet {
    internal abstract class InfFileExtractorBase {
        #region Properties

        protected string FileName { get; }

        protected string OutputFolder { get; }

        #endregion Properties

        #region Public Methods

        public InfFileExtractorBase(string fileName, string outputFolder) {
            FileName = fileName;
            OutputFolder = outputFolder;
        }

        public abstract bool ExtractInfFiles();

        #endregion Public Methods

        #region Internal Methods

        // Prepares the given output directory
        protected string PrepareOutputDirectoryForFile(string fileName) {
            string outputPath = Path.Combine(Path.Combine(OutputFolder, Path.GetFileNameWithoutExtension(FileName), fileName));
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

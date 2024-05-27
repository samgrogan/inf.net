using InfNet.Helpers;
using InfNet.Models;
using SummarizeInfsConsole.Models.Enums;

namespace SummarizeInfsConsole.Models {
    internal class Arguments {
        #region Properties

        public string Source { get; init; }

        public SourceType SourceType { get; init; }

        public string OutputPath { get; init; }

        #endregion Properties

        #region Public Methods

        // Initialze the arguments based on what was passed in from the command line
        public static Arguments? Create(string[] args) {
            // Expect 2 arguments
            if (args.Length != 2) {
                Console.WriteLine($"Usage: SummarizeInfsConsole.exe <source> <output file path>");
                Console.WriteLine($"\t<source> can be a folder containing INF files, a file (zip or exe), or a URL to a file (zip or exe).");
                return null;
            }

            // What is the source type (folder, file, or url)?
            string source = args[0];
            SourceType? sourceType;
            if (Directory.Exists(source)) {
                sourceType = SourceType.Directory;
            }
            else if (File.Exists(source)) {
                if (PathHelpers.SafePathGetExtension(source) == Constants.InfFileExtension) {
                    sourceType = SourceType.InfFile;
                } else {
                    // Default to archive
                    sourceType = SourceType.ArchiveFile;
                }
            }
            else if (Uri.TryCreate(source, UriKind.Absolute, out _)) {
                if (PathHelpers.SafePathGetExtension(source) == Constants.InfFileExtension) {
                    sourceType = SourceType.InfUrl;
                }
                else {
                    // Default to archive
                    sourceType = SourceType.ArchiveUrl;
                }
            }
            else {
                Console.WriteLine($"Source '{source}' does not exist.");
                return null;
            }

            // Where are we writing the output file?
            string output = args[1];

            return new Arguments(source, sourceType.Value, output);
        }

        #endregion Public Methods

        #region Internal Methods

        private Arguments(string source, SourceType sourceType, string outputPath) {
            Source = source;
            SourceType = sourceType;
            OutputPath = outputPath;
        }

        #endregion Internal Methods
    }
}

namespace SummarizeInfsConsole.Models {
    internal class Arguments {
        #region Properties

        public string SourceFolder { get; init; }

        public string OutputPath { get; init; }

        #endregion Properties

        #region Public Methods

        // Initialze the arguments based on what was passed in from the command line
        public static Arguments? Create(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine($"Usage: SummarizeInfsConsole.exe <source folder> <output file path>");
                return null;
            }

            string source = args[0];
            if (!Directory.Exists(source)) {
                Console.WriteLine($"Source folder '{source}' does not exist.");
                return null;
            }

            string output = args[1];

            return new Arguments(source, output);
        }

        #endregion Public Methods

        #region Internal Methods

        private Arguments(string source, string output) {
            SourceFolder = source;
            OutputPath = output;
        }

        #endregion Internal Methods
    }
}

namespace ExtractInfsConsole.Models {
    internal class Arguments {
        #region Properties

        public string SourceFolder { get; init; }

        public string OutputFolder { get; init; }

        #endregion Properties

        #region Public Methods

        // Initialze the arguments based on what was passed in from the command line
        public static Arguments? Create(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine($"Usage: ExtractInfsConsole.exe <source folder> <output folder>");
                return null;
            }

            string source = args[0];
            if (!Directory.Exists(source)) {
                Console.WriteLine($"Source folder '{source}' does not exist.");
                return null;
            }

            string output = args[1];
            if (!Directory.Exists(output)) {
                Console.WriteLine($"Output folder '{output}' does not exist.");
                return null;
            }

            return new Arguments(source, output);
        }


        #endregion Public Methods

        #region Internal Methods

        private Arguments(string source, string output) {
            SourceFolder = source;
            OutputFolder = output;
        }

        #endregion Internal Methods
    }
}

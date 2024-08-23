using ExtractInfsConsole.Models;
using InfNet;

namespace ExtractInfsConsole {
    internal class Program {
        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                ProcessDirectory(arguments.SourceFolder, arguments.OutputFolder);
            }
        }

        // Try to process any files in the source directory and recurse through child directories
        static void ProcessDirectory(string source, string output) {
            // Get the list of files in the source directory
            string[] fileNames = Directory.GetFiles(source);
            foreach (string fileName in fileNames) {
                Console.WriteLine($"{fileName}...");
                // Is the file supported
                bool result = InfExtractor.ExtractInfFilesFromFileToDirectory(fileName, output);
                Console.WriteLine($"\t{result}");
            }

            // Recurse through any sub directories
            string[] directories = Directory.GetDirectories(source);
            foreach (string directory in directories) {
                ProcessDirectory(directory, output);
            }
        }
    }
}

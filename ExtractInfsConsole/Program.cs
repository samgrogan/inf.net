using ExtractInfsConsole.Models;
using InfNet;

namespace ExtractInfsConsole {
    internal class Program {
        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                // Get the list of files in the source directory
                string[] fileNames = Directory.GetFiles(arguments.SourceFolder);

                foreach (string fileName in fileNames) {
                    Console.WriteLine($"{fileName}...");
                    // Is the file supported
                    bool result = InfExtractor.ExtractInfFiles(fileName, arguments.OutputFolder);
                    Console.WriteLine($"\t{result}");
                }
            }
        }
    }
}

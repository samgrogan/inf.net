using ExtractInfsConsole.Models;

namespace ExtractInfsConsole {
    internal class Program {
        static void Main(string[] args) {
            Arguments? arguments = Arguments.Create(args);

            if (arguments != null) {
                // Get the list of files in the source directory
                string[] fileNames = Directory.GetFiles(arguments.SourceFolder);

                foreach (string fileName in fileNames) {



                }
            }
        }
    }
}

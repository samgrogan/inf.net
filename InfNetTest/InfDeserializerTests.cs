using InfNet.Models.Public;
using System.Diagnostics;

namespace InfNet.Test {
    [TestClass]
    public class InfDeserializerTests {
        [TestMethod]
        public void DeserializeWindowsInfsTest() {
            // Get a list of the INF files in te Windows\INF folder
            string windowsInfFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"INF");
            List<string> infFileNames = Directory.EnumerateFiles(windowsInfFolder, "*.inf").ToList<string>();

            // Try to deserialize each file
            foreach (string infFileName in infFileNames) {
                string infFilePath = Path.Combine(windowsInfFolder, infFileName);
                Debug.WriteLine(infFilePath);

                try {
                    InfFile infFile = InfDeserializer.DeserializeFromFile(infFilePath);

                    // Write out the lines
                    foreach (InfLine line in infFile.Lines) {
                        Debug.WriteLine($"\t{line}");
                    }
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}

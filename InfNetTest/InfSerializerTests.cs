using InfNet.Models;
using System.Diagnostics;

namespace INF.Net.Test {
    [TestClass]
    public class InfSerializerTests {
        [TestMethod]
        public void DeserializeWindowsOemInfs() {
            // Get a list of the INF files in te Windows\INF folder
            string windowsInfFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"INF");
            List<string> infFileNames = Directory.EnumerateFiles(windowsInfFolder, "oem*.inf").ToList<string>();

            // Try to deserialize each file
            foreach (string infFileName in infFileNames) {
                string infFilePath = Path.Combine(windowsInfFolder, infFileName);

                Debug.WriteLine(infFilePath);
                InfFile infFile = InfSerializer.DeserializeFromFile(infFilePath);
            }
        }
    }
}

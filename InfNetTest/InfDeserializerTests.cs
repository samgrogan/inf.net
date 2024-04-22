using InfNet.Models.Public;
using System.Diagnostics;

namespace INF.Net.Test
{
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
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        //[TestMethod]
        //[DataRow("HKR, Settings, Compression_On,, \"%%C1\"\"H3-J1\"")]
        //[DataRow("%PCI\\CC_0C0010.DeviceDesc%=Generic.Install,PCI\\CC_0C0010")]
        //public void DeserializeInfLineTests(string line) {
        //    InfSection? currentSection = null;
        //    InfDeserializer.ParseLine(line, ref currentSection);
        //}
    }
}

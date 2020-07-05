using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using INF.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace INF.Net.Test {
    [TestClass]
    public class InfSerializerTests {
        [TestMethod]
        public void DeserializeTest() {
            // Get a list of the INF files in te Windows\INF folder
            string windowsInfFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"\INF");
            List<string> infFileNames = Directory.EnumerateFiles(windowsInfFolder, "*.inf").ToList<string>();

            // Try to deserialize each file
            foreach (string infFileName in infFileNames) {

            }

        }
    }
}

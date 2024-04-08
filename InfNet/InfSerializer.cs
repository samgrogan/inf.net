using System.Diagnostics;
using System.IO;
using System.Text;
using InfNet.Models;

namespace INF.Net {
    public class InfSerializer {

        // Deserialize an INF file from the given file path
        public static InfFile DeserializeFromFile(string filePath) {
            using FileStream infFileStream = File.OpenRead(filePath);
            return DeserializeFromStream(infFileStream);
        }

        // Deserialize an INF file from the given stream
        public static InfFile DeserializeFromStream(Stream inputStream) {
            using StreamReader streamReader = new(inputStream, true);
            return DeserializeFromString(streamReader.ReadToEnd());
        }

        // Deserialize an ING file from 

        public static InfFile DeserializeFromString(string infContents) {
            InfFile infFile = new();

            using StringReader stringReader = new(infContents);
            string? infLine;
            while ((infLine = stringReader.ReadLine()) != null) {
                Debug.WriteLine(infLine);
            }

            return infFile;
        }

        public static void Serialize(InfFile inputFile) {
            return;
        }
    }
}

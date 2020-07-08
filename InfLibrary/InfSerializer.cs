using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF.Net {
    public class InfSerializer {

        // Deserialize an INF file from the given file path
        InfFile DeserializeFromFile(string FilePath) {
            using (FileStream infFileStream = File.OpenRead(FilePath)) {
                return DeserializeFromStream(infFileStream);
            }
        }

        // Deserialize an INF file from the given stream
        InfFile DeserializeFromStream(Stream InputStream) {
            StringBuilder infStringBuilder = new StringBuilder();
            byte[] buffer = new byte[1024];
            UTF8Encoding utf8Encoding = new UTF8Encoding(true);

            while (InputStream.Read(buffer, 0, buffer.Length) > 0) {
                infStringBuilder.Append(utf8Encoding.GetString(buffer));
            }

            return DeserializeFromString(infStringBuilder.ToString());
        }

        // Deserialize an ING file from 

        InfFile DeserializeFromString(string InfContents) {
            InfFile infFile = new InfFile();




            return infFile;
        }

        void Serialize(InfFile InputFile) {
            return;
        }
    }
}

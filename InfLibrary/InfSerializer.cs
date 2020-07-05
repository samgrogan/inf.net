using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF.Net {
    public class InfSerializer {

        async Task<InfFile> DeserializeFromFileAsync(string FilePath) {

            using (FileStream infFileStream = File.OpenRead(FilePath)) {
                return await DeserializeFromStreamAsync(infFileStream);
            }
        }

        async Task<InfFile> DeserializeFromStreamAsync(Stream InputStream) {
            StringBuilder infStringBuilder = new StringBuilder();
            byte[] buffer = new byte[1024];
            UTF8Encoding utf8Encoding = new UTF8Encoding(true);

            while (InputStream.Read(buffer, 0, buffer.Length) > 0) {
                infStringBuilder.Append(utf8Encoding.GetString(buffer));
            }

            return await DeserializeFromStringAsync(infStringBuilder.ToString());
        }

        async Task<InfFile> DeserializeFromStringAsync(string InfContents) {
            InfFile infFile = new InfFile();



            return infFile;
        }

        Task SerializeAsync(InfFile InputFile) {
            return null;
        }
    }
}

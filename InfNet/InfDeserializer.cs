using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using InfNet.Models;

namespace INF.Net {
    public class InfDeserializer {

        #region Public Methods

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

        // Deserialize an INF file from a string 

        public static InfFile DeserializeFromString(string infContents) {
            InfFile infFile = new();
            InfSection? currentSection = null;

            using StringReader stringReader = new(infContents);
            string? infLine;
            while ((infLine = stringReader.ReadLine()) != null) {
                // Parse the line
                ParseLine(infLine, ref currentSection);
            }

            return infFile;
        }

        #endregion Public Methods

        #region Internal Methods

        // Parse the given line in the INF
        public static void ParseLine(string line, ref InfSection? currentSection) {
            bool inSectionName = false;
            bool inComment = false;
            bool inQuotes = false;
            bool inQuotedString = false;

            int length = line.Length;
            for (int index = 0; index < length; index++) {
                char c = line[index];
                switch (c) {
                    case '[':
                        if (!inComment) {
                            if (inSectionName && !inQuotes) {
                                throw new($"Found '[' in section name.");
                            }
                            else if (!inQuotes) {
                                inSectionName = true;
                            }
                        }
                        break;
                    case ']':
                        if (!inComment) {
                            if (inSectionName && !inQuotes) {
                                inSectionName = false;
                            }
                            else if (!inQuotes) {
                                throw new($"Found ']' not in section name.");
                            }
                        }
                        break;
                    case ';':
                        if (!inQuotes) {
                            inComment = true;
                        }
                        break;
                    case '"':
                        // Is this a double quote
                        if (!inComment) {
                            if (!inQuotes) {
                                inQuotes = true;
                            }
                            else {
                                if (index < (length - 1) && line[index + 1] == '"') {
                                    index++;
                                }
                                else {
                                    inQuotes = false;
                                }
                            }
                        }
                        break;
                    case ',':
                        break;
                    case '=':
                        break;
                    case '%':
                        if (!inComment) {
                            if (!inQuotedString) {
                                inQuotedString = true;
                            } else {
                                if (index < (length - 1) && line[index + 1] == '%') {
                                    index++;
                                }
                                else {
                                    inQuotedString = false;
                                }
                            }
                        }
                        break;
                    case '\\':
                        if (!inComment && !inQuotes && !inQuotedString) {
                            if (index < (length - 1) && line[index + 1] == '\\') {
                                index++;
                            }
                            else {
                                throw new($"Continues to the next line.");
                            }
                        }
                        break;
                }
            }
            if (inQuotes) {
                throw new($"Error finding matching quotes.");
            }
            if (inQuotedString) {
                throw new($"Error finding end of quoted string.");
            }
            if (inSectionName) {
                throw new($"Error finding end of section name.");
            }
        }


        #endregion Internal Methods
    }
}

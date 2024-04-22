using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using InfNet.Collections;
using InfNet.Enums;
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
            TokenList tokens = new();
            InfToken? currentToken = tokens.Add(new(InfNet.Enums.TokenType.StartOfFile));

            // Convert the file to a stream of tokens
            int length = infContents.Length;
            for (int index = 0; index < length; index++) {
                char c = infContents[index];
                char? next = (index < (length - 1)) ? infContents[index + 1] : null;

                ParseNextChar(c, next, ref index, ref currentToken, tokens);
            }

            currentToken = tokens.Add(new(InfNet.Enums.TokenType.EndOfFile));

            InfFile infFile = new();

            return infFile;
        }

        #endregion Public Methods

        #region Internal Methods

        // Parse the next character in the INF file
        private static void ParseNextChar(char c, char? next, ref int index, ref InfToken? currentToken, TokenList tokens) {
            if (c == '\n') { // Is this a newline? (\n)
                currentToken = tokens.Add(new(InfNet.Enums.TokenType.NewLine, c));
            }
            else if ($"{c}{next}" == $"\r\n") { // Is this a newline? (\r\n)
                index++;
                tokens.Add(new(TokenType.NewLine, $"{c}{next}"));
            }
            else {
                switch (c) {
                    case ';': // Start Comment
                        currentToken = tokens.Add(new(TokenType.StartComment, c));
                        break;
                    case '=': // Equals
                        currentToken = tokens.Add(new(TokenType.EqualsSymbol, c));
                        break;
                    case ',': // Comma
                        currentToken = tokens.Add(new(TokenType.CommaSeparator, c));
                        break;
                    case '\\': // Continue line
                        // A line continuation can only happen before whitespace
                        if (next == null || char.IsWhiteSpace(next.Value) || next == '\n' || next == '\r' || next == ';') {
                            currentToken = tokens.Add(new(TokenType.LineContinuation, c));
                        }
                        else {
                            currentToken = tokens.AddToLiteral(c);
                        }
                        break;
                    case '[': // Start section name
                        currentToken = tokens.Add(new(TokenType.StartSectionName, c));
                        break;
                    case ']': // End section name
                        currentToken = tokens.Add(new(TokenType.EndSectionName, c));
                        break;
                    case '%': // Start or end string literal
                        currentToken = tokens.Add(new(TokenType.StringToken, c));
                        break;
                    case '"': // Quote      
                        if (next == '"') { // Is this a quote literal?
                            index++;
                            currentToken = tokens.AddToLiteral(c);
                        }
                        else {
                            currentToken = tokens.Add(new(TokenType.QuotedString, c));
                        }
                        break;
                    default:
                        if (currentToken == null || currentToken.Type != TokenType.Literal) {
                            currentToken = tokens.Add(new(TokenType.Literal, c));
                        }
                        else {
                            currentToken.Data += c;
                        }
                        break;
                }
            }
        }

        // Parse the given line in the INF
        //public static void ParseLine(string line, ref InfSection? currentSection) {
        //    bool inSectionName = false;
        //    bool inComment = false;
        //    bool inQuotes = false;
        //    bool inQuotedString = false;

        //    int length = line.Length;
        //    for (int index = 0; index < length; index++) {
        //        char c = line[index];
        //        switch (c) {
        //            case '[':
        //                if (!inComment) {
        //                    if (inSectionName && !inQuotes) {
        //                        throw new($"Found '[' in section name.");
        //                    }
        //                    else if (!inQuotes) {
        //                        inSectionName = true;
        //                    }
        //                }
        //                break;
        //            case ']':
        //                if (!inComment) {
        //                    if (inSectionName && !inQuotes) {
        //                        inSectionName = false;
        //                    }
        //                    else if (!inQuotes) {
        //                        throw new($"Found ']' not in section name.");
        //                    }
        //                }
        //                break;
        //            case ';':
        //                if (!inQuotes) {
        //                    inComment = true;
        //                }
        //                break;
        //            case '"':
        //                // Is this a double quote
        //                if (!inComment) {
        //                    if (!inQuotes) {
        //                        inQuotes = true;
        //                    }
        //                    else {
        //                        if (index < (length - 1) && line[index + 1] == '"') {
        //                            index++;
        //                        }
        //                        else {
        //                            inQuotes = false;
        //                        }
        //                    }
        //                }
        //                break;
        //            case ',':
        //                break;
        //            case '=':
        //                break;
        //            case '%':
        //                if (!inComment) {
        //                    if (!inQuotedString) {
        //                        inQuotedString = true;
        //                    } else {
        //                        if (index < (length - 1) && line[index + 1] == '%') {
        //                            index++;
        //                        }
        //                        else {
        //                            inQuotedString = false;
        //                        }
        //                    }
        //                }
        //                break;
        //            case '\\':
        //                if (!inComment && !inQuotes && !inQuotedString) {
        //                    if (index < (length - 1) && line[index + 1] == '\\') {
        //                        index++;
        //                    }
        //                    else {
        //                        throw new($"Continues to the next line.");
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //    if (inQuotes) {
        //        throw new($"Error finding matching quotes.");
        //    }
        //    if (inQuotedString) {
        //        throw new($"Error finding end of quoted string.");
        //    }
        //    if (inSectionName) {
        //        throw new($"Error finding end of section name.");
        //    }
        //}


        #endregion Internal Methods
    }
}

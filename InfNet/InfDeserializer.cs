using System.Collections.Generic;
using System.IO;
using System.Text;
using InfNet.Enums;
using InfNet.Models.Internal;
using InfNet.Models.Public;

namespace INF.Net {
    public class InfDeserializer {

        // Keep track of the current status while parsing the list of tokens


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
            InfRawLineTokens rawTokens = new();

            // Convert the file to a stream of tokens
            int length = infContents.Length;
            bool continueLine = false;
            for (int index = 0; index < length; index++) {
                char c = infContents[index];
                char? next = (index < (length - 1)) ? infContents[index + 1] : null;

                ParseNextChar(c, next, ref index, ref continueLine, rawTokens);
            }

            // Initialize the INF file object from the list of tokens
            InfFile infFile = CreateInfFileFromRawTokens(rawTokens);

            return infFile;
        }

        #endregion Public Methods

        #region Internal Methods

        // Parse the next character in the INF file
        private static void ParseNextChar(char c, char? next, ref int index, ref bool continueLine, InfRawLineTokens rawTokens) {
            if (c == '\n' || $"{c}{next}" == $"\r\n") { // Is this a newline? (\n or \r\n)
                if (continueLine) {
                    continueLine = false;
                }
                else {
                    rawTokens.AddLine();
                    if ($"{c}{next}" == $"\r\n") { // If 2 character new line, then skip the second character
                        index++;
                    }
                }
            }
            else {
                InfRawToken? currentToken = rawTokens.CurrentToken();

                switch (c) {
                    case ';': // Start Comment
                        rawTokens.AddToken(new(RawTokenType.StartComment, c));
                        break;
                    case '=': // Equals
                        rawTokens.AddToken(new(RawTokenType.EqualsSymbol, c));
                        break;
                    case ',': // Comma
                        rawTokens.AddToken(new(RawTokenType.CommaSeparator, c));
                        break;
                    case '\\': // Continue line
                        // A line continuation can only happen before whitespace
                        if (next == null || char.IsWhiteSpace(next.Value) || next == '\n' || next == '\r' || next == ';') {
                            continueLine = true;
                        }
                        else {
                            rawTokens.AddToLiteral(c);
                        }
                        break;
                    case '[': // Start section name
                        rawTokens.AddToken(new(RawTokenType.StartSectionName, c));
                        break;
                    case ']': // End section name
                        rawTokens.AddToken(new(RawTokenType.EndSectionName, c));
                        break;
                    case '%': // Start or end string literal
                        rawTokens.AddToken(new(RawTokenType.StringToken, c));
                        break;
                    case '"': // Quote      
                        if (next == '"') { // Is this a quote literal?
                            index++;
                            rawTokens.AddToLiteral(c);
                        }
                        else {
                            rawTokens.AddToken(new(RawTokenType.QuotedString, c));
                        }
                        break;
                    default:
                        if (currentToken == null || currentToken.Type != RawTokenType.Literal) {
                            rawTokens.AddToken(new(RawTokenType.Literal, c));
                        }
                        else {
                            currentToken.Data += c;
                        }
                        break;
                }
            }
        }

        // Creates and INF File from a series of tokens
        private static InfFile CreateInfFileFromRawTokens(InfRawLineTokens rawTokens) {
            InfFile infFile = new();

            // Parse each line in the input
            foreach (List<InfRawToken> rawLineTokens in rawTokens) {
                InfLine line = ParseNextLine(infFile, rawLineTokens);
                infFile.Lines.Add(line);
            }

            return infFile;
        }

        // Parse a line of tokens
        private static InfLine ParseNextLine(InfFile infFile, List<InfRawToken> rawLineTokens) {
            // Is this a blank line?
            if (rawLineTokens.Count == 0) {
                return new InfLine();
            }
            else {
                // Extract any comments on the line
                InfLine line = new() {
                    Comment = ExtractComment(rawLineTokens)
                };

                // What is the first token in the line?
                InfRawToken firstToken = rawLineTokens[0];
                switch (firstToken.Type) {
                    case RawTokenType.StartComment:
                        break;
                    case RawTokenType.StartSectionName:
                        break;
                    default:
                        // Optional key and values
                        break;
                }

                return line;
            }
        }

        //// Parse the next token in a list of tokens
        //private static void ParseNextToken(InfFile infFile, InfRawToken currentToken, ref InfLine? currentLine, InfLineFlags lineFlags) {
        //    switch (currentToken.Type) {
        //        case RawTokenType.StartComment:
        //            if (!lineFlags.InComment) {
        //                lineFlags.InComment = true;
        //            }
        //            break;
        //        case RawTokenType.Literal:
        //            break;
        //        case RawTokenType.EqualsSymbol:
        //            break;
        //        case RawTokenType.CommaSeparator:
        //            break;
        //        case RawTokenType.StartSectionName:
        //            if (currentLine == null) {
        //                currentLine = new InfSection("");
        //            }
        //            break;
        //        case RawTokenType.EndSectionName:
        //            break;
        //        case RawTokenType.StringToken:
        //            break;
        //        case RawTokenType.QuotedString:
        //            break;

        //        // Fall through in case we missed something (should not happen)
        //        default:
        //            throw new($"Unknown raw token type '{currentToken.Type}'.");
        //    }
        //}

        private static string? ExtractComment(List<InfRawToken> rawLineTokens) {
            bool inComment = false;
            StringBuilder comment = new();

            foreach (InfRawToken token in rawLineTokens) {
                if (token.Type == RawTokenType.StartComment) {
                    inComment = true;
                }
                else if (inComment) {
                    comment.Append(token.Data);
                }
            }

            return (inComment ? comment.ToString() : null);
        }

        #endregion Internal Methods
    }
}

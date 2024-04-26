using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using InfNet.Enums;
using InfNet.Helpers;
using InfNet.Models.Internal;
using InfNet.Models.Public;

namespace INF.Net {
    public class InfDeserializer {

        // Keep track of the current status while parsing the list of tokens


        #region Public Methods

        // Deserialize an INF file from the given file path
        public static InfFile DeserializeFromFile(string filePath) {
            using FileStream infFileStream = File.OpenRead(filePath);
            return DeserializeFromStream(Path.GetFileName(filePath), infFileStream);
        }

        // Deserialize an INF file from the given stream
        public static InfFile DeserializeFromStream(string fileName, Stream inputStream) {
            using StreamReader streamReader = new(inputStream, true);
            return DeserializeFromString(fileName, streamReader.ReadToEnd());
        }

        // Deserialize an INF file from a string 

        public static InfFile DeserializeFromString(string fileName, string infContents) {
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
            InfFile infFile = CreateInfFileFromRawTokens(fileName, rawTokens);

            return infFile;
        }

        #endregion Public Methods

        #region Internal Methods

        // Parse the next character in the INF file
        private static void ParseNextChar(char c, char? next, ref int index, ref bool continueLine, InfRawLineTokens rawTokens) {
            if (c == '\n' || $"{c}{next}" == $"\r\n") { // Is this a newline? (\n or \r\n)
                rawTokens.AddToken(new(RawTokenType.NewLine));
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
                    case CharHelpers.SemicolonChar: // Start Comment
                        rawTokens.AddToken(new(RawTokenType.StartComment, c));
                        break;
                    case CharHelpers.EqualsChar: // Equals
                        rawTokens.AddToken(new(RawTokenType.EqualsSymbol, c));
                        break;
                    case CharHelpers.CommaChar: // Comma
                        rawTokens.AddToken(new(RawTokenType.CommaSeparator, c));
                        break;
                    case CharHelpers.BackslashChar: // Continue line
                        // A line continuation can only happen before whitespace
                        if (next == null || char.IsWhiteSpace(next.Value) || next == '\n' || next == '\r' || next == CharHelpers.SemicolonChar) {
                            continueLine = true;
                        }
                        else {
                            rawTokens.AddToLiteral(c);
                        }
                        break;
                    case CharHelpers.LeftSquareBracketChar: // Start section name
                        rawTokens.AddToken(new(RawTokenType.StartSectionName, c));
                        break;
                    case CharHelpers.RightSquareBracketChar: // End section name
                        rawTokens.AddToken(new(RawTokenType.EndSectionName, c));
                        break;
                    case CharHelpers.PercentChar: // Start or end string literal
                        if (next == CharHelpers.PercentChar) { // Is this a percent literal?
                            index++;
                            rawTokens.AddToLiteral(c);
                        }
                        else {
                            rawTokens.AddToken(new(RawTokenType.StringToken, c));
                        }
                        break;
                    case CharHelpers.DoubleQuoteChar: // Quote      
                        if (next == CharHelpers.DoubleQuoteChar) { // Is this a quote literal?
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
        private static InfFile CreateInfFileFromRawTokens(string fileName, InfRawLineTokens rawTokens) {
            InfFile infFile = new(fileName);

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
                        // Comments have already been extracted
                        break;
                    case RawTokenType.StartSectionName:
                        line.SectionName = ExtractSectionName(rawLineTokens);
                        break;
                    default:
                        // Extract optional key and values
                        ExtractKeyAndValues(line, rawLineTokens);
                        break;
                }

                return line;
            }
        }

        private static string? ExtractComment(List<InfRawToken> rawLineTokens) {
            bool inComment = false;
            StringBuilder comment = new();

            foreach (InfRawToken token in rawLineTokens) {
                if (token.Type == RawTokenType.StartComment) {
                    inComment = true;
                }
                else if (token.Type == RawTokenType.NewLine) {
                    inComment = false;
                }
                else if (inComment) {
                    comment.Append(token.Data);
                }
            }

            return comment.ToString();
        }

        private static string? ExtractSectionName(List<InfRawToken> rawLineTokens) {
            bool inSectionName = false;
            StringBuilder sectionName = new();

            foreach (InfRawToken token in rawLineTokens) {
                if (token.Type == RawTokenType.StartSectionName) {
                    inSectionName = true;
                }
                else if (inSectionName) {
                    if (token.Type == RawTokenType.EndSectionName) {
                        inSectionName = false;
                    }
                    else {
                        sectionName.Append(token.Data);
                    }
                }
            }

            return sectionName.ToString();
        }

        private static void ExtractKeyAndValues(InfLine line, List<InfRawToken> rawLineTokens) {
            InfLineFlags flags = new();
            InfValue currentValue = new();

            // Parse the tokens in the line
            foreach (InfRawToken token in rawLineTokens) {
                ParseNextToken(token, line, flags, ref currentValue);
            }
        }

        // Parse the next token in a list of tokens
        private static void ParseNextToken(InfRawToken token, InfLine line, InfLineFlags flags, ref InfValue currentValue) {
            if (!flags.InComment) {
                switch (token.Type) {
                    case RawTokenType.StartComment: // Comments run to end of line
                        flags.InComment = true;
                        break;
                    case RawTokenType.Literal:
                        currentValue.Value += token.Data;
                        break;
                    case RawTokenType.EqualsSymbol: // Left of the equals is the key
                        if (!flags.InQuotedString && !flags.InStringToken) {
                            if (line.Key == null) {
                                line.AddKey(currentValue);
                                currentValue = new();
                            }
                            else {
                                // Treat any additional = as text
                                currentValue.Value += token.Data;
                            }
                        }
                        else {
                            currentValue.Value += token.Data;
                        }
                        break;
                    case RawTokenType.CommaSeparator:
                        if (!flags.InQuotedString && !flags.InStringToken) {
                            line.AddValue(currentValue);
                            currentValue = new();
                        }
                        else {
                            currentValue.Value += token.Data;
                        }
                        break;
                    case RawTokenType.StartSectionName:
                    case RawTokenType.EndSectionName:
                        if (!flags.InQuotedString && !flags.InStringToken) {
                            throw new($"Found unexpected token '{token.Type}'.");
                        }
                        break;
                    case RawTokenType.StringToken:
                        if (!flags.InStringToken) {
                            flags.InStringToken = true;
                            currentValue.IsStringToken = true;
                        }
                        else {
                            flags.InStringToken = false;
                        }
                        break;
                    case RawTokenType.QuotedString:
                        if (!flags.InQuotedString) {
                            flags.InQuotedString = true;
                            currentValue.IsQuotedString = true;
                        }
                        else {
                            flags.InQuotedString = false;
                        }
                        break;
                    case RawTokenType.NewLine:
                        line.AddValue(currentValue);
                        break;
                    // Fall through in case we missed something (should not happen)
                    default:
                        throw new($"Unknown raw token type '{token.Type}'.");
                }
            }
            else {
                if (token.Type == RawTokenType.NewLine) {
                    flags.InComment = false;
                }
            }
        }

        #endregion Internal Methods
    }
}

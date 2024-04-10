﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using InfNet.Enums;
using InfNet.Extensions;
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
            Stack<InfToken> tokens = new();
            InfToken? currentToken = tokens.PushAndReturn(new(InfNet.Enums.TokenType.StartOfFile));

            // Convert the file to a stream of tokens
            int length = infContents.Length;
            for (int index = 0; index < length; index++) {
                char c = infContents[index];
                char? next = (index < (length - 1)) ? infContents[index + 1] : null;

                ParseNextChar(c, next, ref index, ref currentToken, tokens);
            }

            currentToken = tokens.PushAndReturn(new(InfNet.Enums.TokenType.EndOfFile));

            InfFile infFile = new();

            return infFile;
        }

        #endregion Public Methods

        #region Internal Methods

        // Parse the next character in the INF file
        private static void ParseNextChar(char c, char? next, ref int index, ref InfToken? currentToken, Stack<InfToken> tokens) {
            // Is this a newline? (\n)
            if (c == '\n') {
                currentToken = tokens.PushAndReturn(new(InfNet.Enums.TokenType.NewLine, c));
            }
            // Is this a newline? (
            else if ($"{c}{next}" == $"\r\n") {
                index++;
                tokens.PushAndReturn(new(InfNet.Enums.TokenType.NewLine, $"{c}{next}"));
            }
            else {
                switch (c) {
                    case ';':
                        currentToken = tokens.PushAndReturn(new(TokenType.StartComment, c));
                        break;
                    case '=':
                        currentToken = tokens.PushAndReturn(new(TokenType.EqualsSymbol, c));
                        break;
                    case ',':
                        currentToken = tokens.PushAndReturn(new(TokenType.CommaSeparator, c));
                        break;
                    case '\\':
                        // A line continuation can only happen before whites
                        if (next == null || char.IsWhiteSpace(next.Value) || next == '\n' || next == '\r' || next == ';') {
                            currentToken = tokens.PushAndReturn(new(TokenType.LineContinuation, c));
                        }
                        else {
                            if (currentToken == null || currentToken.Type != TokenType.Literal) {
                                currentToken = tokens.PushAndReturn(new(TokenType.Literal, c));
                            } else {
                                currentToken.Data += c;
                            }
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

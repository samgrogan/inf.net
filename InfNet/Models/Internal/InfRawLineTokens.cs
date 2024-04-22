using InfNet.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfNet.Models.Internal {
    internal class InfRawLineTokens : IEnumerable<List<InfRawToken>> {
        #region Fields

        // Store the raw list of tokens by line
        protected List<List<InfRawToken>> _tokens;

        #endregion Fields

        #region Properties

        public List<InfRawToken> this[int index] { get => _tokens[index]; }

        public int LineCount => _tokens.Count;

        #endregion Properties

        #region Public Methods

        public InfRawLineTokens() {
            _tokens = new();
        }

        // Adds a new line
        public List<InfRawToken> AddLine() {
            List<InfRawToken> newLine = new();
            _tokens.Add(newLine);
            return newLine;
        }

        // Gets the current (last) line of tokens
        public List<InfRawToken> CurrentLine() {
            if (_tokens.Count == 0) {
                return AddLine();
            }
            else {
                return _tokens.Last();
            }
        }

        // Adds a new token to the list of tokens, and returns the token
        public InfRawToken AddToken(InfRawToken token) {
            // Get the current line of tokens
            List<InfRawToken> currentLine = CurrentLine();
            currentLine.Add(token);
            token.Index = currentLine.IndexOf(token);
            return token;
        }

        // Gets the current token in the current line
        public InfRawToken? CurrentToken() {
            List<InfRawToken> currentLine = CurrentLine();
            if (currentLine.Count == 0) {
                return null;
            }
            return currentLine.Last();
        }

        // Add to the current literal token (or create a new one)
        public InfRawToken AddToLiteral(char c) {
            InfRawToken? currentToken = CurrentToken();

            if (currentToken == null || currentToken.Type != RawTokenType.Literal) {
                currentToken = AddToken(new(RawTokenType.Literal, c));
            }
            else {
                currentToken.Data += c;
            }

            return currentToken;
        }

        public IEnumerator<List<InfRawToken>> GetEnumerator() {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _tokens.GetEnumerator();
        }

        #endregion Public Methods
    }
}

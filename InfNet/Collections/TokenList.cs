using InfNet.Enums;
using InfNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfNet.Collections {
    public class TokenList {
        #region Constants

        // Lists the allowed transitions (what token can follow a given token)
        private static readonly Dictionary<TokenType, List<TokenType>> _allowedTokenTransitions = new() {
            {
                TokenType.StartOfFile, new() { TokenType.StartComment, TokenType.StartSectionName, TokenType.NewLine, TokenType.EndOfFile }
            }
        };

        #endregion Constants

        #region Fields

        // Store the raw list of tokens
        protected List<InfToken> _tokens;

        #endregion Fields

        #region Public Methods

        public TokenList() {
            _tokens = new();
        }

        // Adds a new token to the list of tokens, and returns the token
        public InfToken Add(InfToken token) {
            _tokens.Add(token);
            token.Index = _tokens.IndexOf(token);
            return token;
        }

        // Add to the current literal token (or create a new one)
        public InfToken AddToLiteral(char c) {
            InfToken? currentToken = _tokens.Last();

            if (currentToken == null || currentToken.Type != TokenType.Literal) {
                currentToken = Add(new(TokenType.Literal, c));
            }
            else {
                currentToken.Data += c;
            }

            return currentToken;
        }

        // Finds the first token of the given type from the end of the list
        public InfToken? MostRecentOfType(TokenType tokenType) {
            return _tokens.LastOrDefault(o => o.Type == tokenType);
        }

        //// Is the current state inside of a comment?
        //public bool IsInComment() {
        //    // Comments end at either end of file or end of line
        //    InfToken? startCommentToken = MostRecentOfType(TokenType.StartComment);
        //    InfToken? endCommentToken = MostRecentOfType(TokenType.NewLine);

        //    // Find the last index
        //    if (startCommentToken != null) {
        //        if (endCommentToken != null) {
        //            if (endCommentToken.Index > startCommentToken.Index) {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }

        //    return false;
        //}

        //// Are we inside a string token value?
        //public bool IsInStringToken() {
        //    InfToken? startStringTokenToken = MostRecentOfType(TokenType.StartStringToken);
        //    InfToken? endStringTokenToken = MostRecentOfType(TokenType.EndStringToken);

        //    if (startStringTokenToken != null) {
        //        if (endStringTokenToken != null) {
        //            if (endStringTokenToken.Index < startStringTokenToken.Index) {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }

        //    return false;
        //}


        #endregion Public Methods
    }
}

using InfNet.Models;
using System.Collections.Generic;

namespace InfNet.Collections {
    internal class TokenList {
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
            return token;
        }

        #endregion Public Methods
    }
}

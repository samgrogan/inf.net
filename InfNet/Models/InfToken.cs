using InfNet.Enums;

namespace InfNet.Models {
    public class InfToken {
        #region Properties

        public TokenType Type { get; set; }

        public string? Data { get; set; }

        #endregion Properties

        #region Public Methods

        public InfToken(TokenType type) {
            Type = type;
        }

        public InfToken(TokenType type, string? data) : this(type) {
            Data = data;
        }

        public InfToken(TokenType type, char c) : this(type) {
            Data = c.ToString();
        }

        #endregion Public Methods
    }
}

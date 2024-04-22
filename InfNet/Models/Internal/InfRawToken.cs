using InfNet.Enums;

namespace InfNet.Models.Internal {
    internal class InfRawToken {
        #region Properties

        public RawTokenType Type { get; set; }

        public int? Index { get; set; }

        public string? Data { get; set; }

        #endregion Properties

        #region Public Methods

        public InfRawToken(RawTokenType type) {
            Type = type;
        }

        public InfRawToken(RawTokenType type, string? data) : this(type) {
            Data = data;
        }

        public InfRawToken(RawTokenType type, char c) : this(type) {
            Data = c.ToString();
        }

        #endregion Public Methods
    }
}

namespace InfNet.Models.Public {
    public class InfValue {
        #region Properties

        public bool IsStringToken { get; set; }

        public bool IsQuotedString { get; set; }

        public string? Value { get; set; }

        #endregion Properties

        #region Public Methods

        public override string ToString() {
            if (IsQuotedString) {
                return $"\"{Value}\"";
            }
            if (IsStringToken) {
                return $"%{Value}%";
            }
            return Value ?? string.Empty;
        }

        #endregion Public Methods
    }
}

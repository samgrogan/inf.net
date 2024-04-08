

namespace InfNet.Models {
    public class InfComment {
        #region Properties

        public string? Comment { get; set; }

        #endregion Properties

        #region Public Methods

        public static InfComment DeserializeFromString(string rawComment) {
            InfComment infComment = new();

            if (rawComment.StartsWith(";")) {
                infComment.Comment = rawComment.Substring(1).Trim();
            }

            return infComment;
        }

        #endregion Public Methods
    }
}



namespace InfNet.Models {
    public class InfComment : InfEntry {
        #region Properties

        public string? Comment { get; set; }

        #endregion Properties

        #region Public Methods

        // Does the given line comtain a comment
        public static bool ContainsComment(string line) {
            return false;
        }

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

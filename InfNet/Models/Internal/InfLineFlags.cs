using System.Collections.Generic;

namespace InfNet.Models.Internal {
    internal class InfLineFlags {
        #region Properties

        // Is the stream of tokens in a comment
        public bool InComment { get; set; }

        // In a section title
        public bool InSectionName { get; set; }

        // In a quoted string ("value")
        public bool InQuotedString { get; set; }

        // In a string token (%name%)
        public bool InStringToken { get; set; }

        #endregion Properties

        #region Public Methods

        // Clears all the flags
        public void Clear() {
            InComment = false;
            InSectionName = false;
            InQuotedString = false;
            InStringToken = false;
        }

        // Are any of the flags set?
        public bool Any() {
            return InComment || InSectionName || InQuotedString || InStringToken;
        }

        public override string ToString() {
            List<string> flags = new();

            if (InComment) {
                flags.Add(nameof(InComment));
            }
            if (InSectionName) {
                flags.Add(nameof(InSectionName));
            }
            if (InQuotedString) {
                flags.Add(nameof(InQuotedString));
            }
            if (InStringToken) {
                flags.Add(nameof(InStringToken));
            }
            return string.Join(", ", flags);
        }

        #endregion Public Methods
    }
}

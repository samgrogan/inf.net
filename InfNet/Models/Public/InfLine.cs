using InfNet.Helpers;
using System.Collections.Generic;
using System.Text;

namespace InfNet.Models.Public {
    public class InfLine {
        #region Properties

        // The name of the section if this is a section break
        public string? SectionName { get; set; }

        // Any comment associated with this line
        public string? Comment { get; set; }

        public InfValue? Key { get; set; }

        public List<InfValue>? Values { get; private set; }

        #endregion Properties

        #region Public Methods

        // Adds a value to the list of values
        public void AddValue(InfValue value) {
            Values ??= new();
            value.Value = value.Value?.Trim();
            Values.Add(value);
        }

        public override string ToString() {
            StringBuilder builder = new();

            if (!string.IsNullOrEmpty(SectionName)) {
                builder.Append($"{CharHelpers.LeftSquareBracketChar}{SectionName}{CharHelpers.RightSquareBracketChar}");
            }
            else {
                if (Key != null) {
                    builder.Append($"{Key}{CharHelpers.EqualsChar}");
                }
                if (Values != null) {
                    builder.Append(string.Join($"{CharHelpers.CommaChar}", Values));
                }
            }
            if (!string.IsNullOrEmpty(Comment)) {
                builder.Append($"{CharHelpers.SemicolonChar}{Comment}");
            }

            return builder.ToString();
        }

        #endregion Public Methods
    }
}

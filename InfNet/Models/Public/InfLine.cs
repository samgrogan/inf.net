using InfNet.Helpers;
using System.Collections.Generic;
using System.Text;

namespace InfNet.Models.Public {
    public class InfLine {
        #region Properties

        // The name of the section if this is a section break
        private string? _sectionName;
        public string? SectionName {
            get => _sectionName;
            set {
                _sectionName = value?.Trim();
            }
        }

        // Any comment associated with this line
        private string? _comment;
        public string? Comment {
            get => _comment;
            set {
                _comment = value?.Trim();
            }
        }

        public InfValue? Key { get; private set; }

        public List<InfValue>? Values { get; private set; }

        #endregion Properties

        #region Public Methods

        public void AddKey(InfValue value) {
            if (Key == null) {
                value.Value = value.Value?.Trim();
                Key = value;
            }
            else {
                throw new($"Key is already set.");
            }
        }

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

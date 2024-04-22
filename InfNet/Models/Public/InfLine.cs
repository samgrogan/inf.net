using System.Collections.Generic;

namespace InfNet.Models.Public {
    public class InfLine {
        #region Properties

        // The name of the section if this is a section break
        public string? SectionName { get; set; }

        // Any comment associated with this line
        public string? Comment { get; set; }

        public string? Key { get; set; }

        public List<string>? Values { get; }

        #endregion Properties
    }
}

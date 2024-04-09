using System.Collections.Generic;

namespace InfNet.Models {
    public class InfEntry {
        #region Properties

        public string? Key { get; set; }

        public List<string> Values { get; } = new();

        #endregion Properties
    }
}

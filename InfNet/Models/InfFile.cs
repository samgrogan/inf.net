using System.Collections.Generic;

namespace InfNet.Models {
    public class InfFile {

        #region Properties

        // A dictionary of the sections in the INF File
        public Dictionary<string, InfSection> Sections { get; } = new();

        #endregion Properties

        //
    }
}

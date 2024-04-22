using System.Collections.Generic;

namespace InfNet.Models.Public {
    public class InfFile {

        #region Properties

        // List of comments

        // A dictionary of the sections in the INF File
        public List<InfLine> Lines { get; } = new();

        #endregion Properties

        #region Public Methods

        // Create a blank INF file
        public InfFile() {
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods
    }
}

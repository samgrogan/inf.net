using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF.Net {
    class InfFile {

        #region Properties

        // A dictionary of the sections in the INF File
        public Dictionary<string, InfFile> Sections { get; } = new Dictionary<string, InfFile>();

        #endregion Properties

        //
    }
}


using System;

namespace InfNet.Models.Public {
    public class InfDriverVersion {
        #region Properties

        public DateTime Date { get; private set; }

        public Version Version { get; private set; }

        #endregion Properties

        #region Public Methods

        public InfDriverVersion(DateTime date, Version version) {
            Date = date;
            Version = version;
        }

        #endregion Public Methods
    }
}

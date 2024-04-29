namespace InfNet.Models.Public {
    public class InfOsDeviceDriver {

        #region Properties

        public InfFile File { get; private set; }

        public InfOs Os { get; private set; }  

        public InfDriverVersion DriverVersion { get; private set; }

        public string DeviceName { get; private set; }

        public string DeviceId { get; private set; }

        #endregion Properties

        #region Public Methods

        public InfOsDeviceDriver(InfFile file, InfOs os, InfDriverVersion driverVersion, string deviceName, string deviceId) {
            File = file;
            Os = os;
            DriverVersion = driverVersion;
            DeviceName = deviceName;
            DeviceId = deviceId;
        }

        #endregion Public Methods
    }
}

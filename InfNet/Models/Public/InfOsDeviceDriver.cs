namespace InfNet.Models.Public {
    public class InfOsDeviceDriver {

        #region Properties

        public InfDriverVersion DriverVersion { get; private set; }

        public string DeviceId { get; private set; }

        #endregion Properties

        #region Public Methods

        public InfOsDeviceDriver(InfDriverVersion driverVersion, string deviceId) {
            DriverVersion = driverVersion;
            DeviceId = deviceId;
        }

        #endregion Public Methods
    }
}

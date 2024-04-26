using InfNet.Models;
using InfNet.Models.Public;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace InfNet {
    public class InfSummarizer {
        #region Public Methods

        // Returns a list of the OS/Device/Driver combinations found in the given INF file
        public static List<InfOsDeviceDriver> SummaryizeInfFile(InfFile infFile) {
            List<InfOsDeviceDriver> summary = new();

            // Read the version information
            try {
                InfDriverVersion driverVersion = ExtractDriverVersion(infFile);

                Console.WriteLine($"Driver version: {driverVersion.Date.ToShortDateString()}, {driverVersion.Version}");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return summary;
        }

        // Extract the driver version information from the INF file
        public static InfDriverVersion ExtractDriverVersion(InfFile infFile) {
            try {
                // Find the version section 
                List<InfLine> infVersionLines = infFile.SectionLines(Constants.VersionSectionName);
                // Find the DriverVer property
                InfLine driverVerLine = infVersionLines.Single(o => o.Key?.Value == Constants.DriverVerKey);
                if (driverVerLine.Values?.Count != 2) {
                    throw new($"Expected 2 values (date, version) for {Constants.DriverVerKey} but found {driverVerLine.Values?.Count ?? 0} instead.");
                }

                // Format is date
                string? rawDateValue = driverVerLine.Values[0].Value;
                if (string.IsNullOrWhiteSpace(rawDateValue)) {
                    throw new($"Date value in {Constants.DriverVerKey} is blank.");
                }
                DateTime date = DateTime.ParseExact(rawDateValue, "d", CultureInfo.InvariantCulture);

                // Format the version
                string? rawVersionValue = driverVerLine.Values[1].Value;
                if (string.IsNullOrWhiteSpace(rawVersionValue)) {
                    throw new($"Version value in {Constants.DriverVerKey} is blank.");
                }
                Version version = Version.Parse(rawVersionValue);

                return new(date, version);
            }
            catch (Exception ex) {
                throw new($"Failed to read driver version information from INF file.", ex);
            }
        }

        #endregion Public Methods


    }
}

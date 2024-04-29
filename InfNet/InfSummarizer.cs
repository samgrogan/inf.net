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

            try {
                // Read the version information
                InfDriverVersion driverVersion = ExtractDriverVersion(infFile);
                Console.WriteLine($"\tDriver version: {driverVersion.Date.ToShortDateString()}, {driverVersion.Version}");

                // Extract the manufacturer information
                InfLine manufacturer = ExtractManufacturer(infFile);
                Console.WriteLine($"\tManufacturer: {manufacturer.Key?.Value} = {string.Join(", ", manufacturer.Values.Select(o => o.Value))}");

                // Build the list of supported devices
                if (manufacturer.Values?.Count > 0) {
                    for (int manufacturerIndex = 1; manufacturerIndex < manufacturer.Values.Count; manufacturerIndex++) {
                        string? decoration = manufacturer.Values[manufacturerIndex].Value?.Trim();
                        if (string.IsNullOrWhiteSpace(decoration)) {
                            throw new($"Manufacturer decoration cannot be blank.");
                        }

                        // Parse the OS value
                        InfOs infOs = InfOs.CreateFromManufacturerDecoration(decoration);
                        Console.WriteLine($"\tOs: {infOs}");

                        // Read the list of devices 
                        string sectionName = $"{manufacturer.Values[0].Value?.Trim()}.{decoration}";
                        List<InfLine> sectionLines = infFile.SectionLines(sectionName);

                        foreach (InfLine line in sectionLines) {
                            if (!string.IsNullOrWhiteSpace(line.Key?.Value)) {
                                if (line.Values?.Count == 2) {
                                    string keyText = infFile.GetStringTokenValue(line.Key.Value);
                                    Console.WriteLine($"\t\t{keyText}: {line.Values[1].Value}");
                                }
                                else {
                                    throw new($"Unexpected line '{line}' encountered.");
                                }
                            }
                        }
                    }
                }
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

        // Extract information about the manufacturer
        public static InfLine ExtractManufacturer(InfFile infFile) {
            try {
                // Find the manufacturer section
                List<InfLine> infManufacturerLines = infFile.SectionLines(Constants.ManufacturerSectionName);

                List<InfLine> valueLines = infManufacturerLines.Where(o => o.Key != null).ToList();
                if (valueLines.Count == 0) {
                    throw new($"Expected at least 1 Manufacturer Name line but found 0.");
                }

                InfLine manufacturerLine = valueLines[0];
                if (valueLines.Count > 1) {
                    // If multiple lines, then merge them
                    foreach (InfLine line in valueLines) {
                        if (manufacturerLine.Key?.Value != line.Key?.Value) {
                            throw new($"Found 2 manufacturer names: {manufacturerLine.Key?.Value} and {line.Key?.Value}.");
                        }
                        if (line.Values?.Count > 0) {
                            foreach (InfValue value in line.Values) {
                                if (!manufacturerLine.Values.Any(o => o.Value == value.Value)) {
                                    manufacturerLine.AddValue(value);
                                }
                            }
                        }
                    }
                }

                return manufacturerLine;
            }
            catch (Exception ex) {
                throw new($"Failed to read manufacturer information from INF file.", ex);
            }
        }

        #endregion Public Methods


    }
}

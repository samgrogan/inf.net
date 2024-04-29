using InfNet.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfNet.Models.Public {
    public class InfOs {
        #region Properties

        public OsArchitecture Architecture { get; private set; }

        public int? OsMajorVersion { get; private set; }
        public int? OsMinorVersion { get; private set; }
        public int? ProductType { get; private set; }
        public int? SuiteMask { get; private set; }
        public int? BuildNumber { get; private set; }

        #endregion Properties

        #region Public Methods

        public static InfOs CreateFromManufacturerDecoration(string decoration) {
            InfOs infOs = new();

            // Must start with "NT"
            if (!decoration.StartsWith(Constants.OsDecorationNt, System.StringComparison.OrdinalIgnoreCase)) {
                throw new ArgumentException($"Decoration must start with {Constants.OsDecorationNt}.");
            }

            // Split by period to get the version components
            string[] decorationParts = decoration.Split(Constants.OsDecorationSeparator);

            // Extract the architecture
            infOs.Architecture = ParseArchitecture(decorationParts[0]);

            // Parse the version parts
            infOs.OsMajorVersion = ParseVersionPart(decorationParts, 1);
            infOs.OsMinorVersion = ParseVersionPart(decorationParts, 2);
            infOs.ProductType = ParseVersionPart(decorationParts, 3);
            infOs.SuiteMask = ParseVersionPart(decorationParts, 4);
            infOs.BuildNumber = ParseVersionPart(decorationParts, 5);

            return infOs;
        }

        public override string ToString() {
            List<string> osParts = new() {
                "Windows"
            };

            // Product Type
            if (ProductType.HasValue) {
                switch (ProductType) {
                    case 0x0000001:
                        osParts.Add("Workstation");
                        break;
                    case 2:
                        osParts.Add("Domain Controller");
                        break;
                    case 3:
                        osParts.Add("Server");
                        break;
                    default:
                        osParts.Add("Unknown Product Type");
                        break;
                }
            }

            // Suite Mark
            if (SuiteMask.HasValue) {
                switch (SuiteMask) {
                    default:
                        osParts.Add("Unknown Suite Mask");
                        break;
                }
            }

            // Version, if any
            List<string> versionParts = new();
            if (OsMajorVersion.HasValue) {
                versionParts.Add($"{OsMajorVersion}");
            }
            if (OsMinorVersion.HasValue) {
                versionParts.Add($"{OsMinorVersion.Value}");
            }
            if (BuildNumber.HasValue) {
                versionParts.Add($"{BuildNumber.Value}");
            }
            if (versionParts.Count > 0) {
                osParts.Add(string.Join(".", versionParts));
            }

            // Architecture
            switch (Architecture) {
                case OsArchitecture.x86:
                    osParts.Add("(x86)");
                    break;
                case OsArchitecture.amd64:
                    osParts.Add("(x64)");
                    break;
                default:
                    osParts.Add($"({Architecture})");
                    break;
            }

            return string.Join(" ", osParts);
        }

        #endregion Public Methods

        #region Internal Methods

        private static OsArchitecture ParseArchitecture(string decorationPart) {
            string architecturePart = decorationPart.Substring(Constants.OsDecorationNt.Length).ToLower();
            if (architecturePart == $"{OsArchitecture.x86}") {
                return OsArchitecture.x86;
            }
            else if (architecturePart == $"{OsArchitecture.ia64}") {
                return OsArchitecture.ia64;
            }
            else if (architecturePart == $"{OsArchitecture.amd64}") {
                return OsArchitecture.amd64;
            }
            else if (architecturePart == $"{OsArchitecture.arm}") {
                return OsArchitecture.arm;
            }
            else if (architecturePart == $"{OsArchitecture.arm64}") {
                return OsArchitecture.arm64;
            }

            throw new ArgumentException($"Unknown architecture decoration '{architecturePart}'.");
        }

        private static int? ParseVersionPart(string[] decorationParts, int index) {
            if (index >= decorationParts.Length) {
                return null;
            }
            if (string.IsNullOrWhiteSpace(decorationParts[index])) {
                return null;
            }
            return int.Parse(decorationParts[index]);
        }

        #endregion Internal Methods
    }
}

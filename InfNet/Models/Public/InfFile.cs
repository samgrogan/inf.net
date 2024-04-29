using System.Collections.Generic;
using System.Linq;

namespace InfNet.Models.Public {
    public class InfFile {

        #region Properties

        // Name of the file
        public string FileName { get; private set; }

        // A dictionary of the sections in the INF File
        public List<InfLine> Lines { get; } = new();

        #endregion Properties

        #region Public Methods

        // Create a blank INF file
        public InfFile(string fileName) {
            FileName = fileName;
        }

        // Returns a list of the section names in the file
        public List<string> SectionNames() {
            return Lines.Select(o => o.SectionName ?? string.Empty).Where(o => !string.IsNullOrEmpty(o)).ToList();
        }

        // Gets the lines for the name section
        public List<InfLine> SectionLines(string sectionMame) {
            List<InfLine> sectionLines = new();

            bool inSection = false;
            foreach (InfLine line in Lines) {
                if (string.Compare(line.SectionName, sectionMame, true) == 0) {
                    inSection = true;
                }
                else if (!string.IsNullOrEmpty(line.SectionName) && string.Compare(line.SectionName, sectionMame, true) != 0) {
                    inSection = false;
                }
                if (inSection) {
                    sectionLines.Add(line);
                }
            }

            return sectionLines;
        }

        // Gets the lines by section for all sections
        public Dictionary<string, List<InfLine>> AllSectionLines() {
            Dictionary<string, List<InfLine>> allSectionLines = new();

            // Get the list of section names
            List<string> sectionNames = SectionNames();
            foreach (string sectionName in sectionNames) {
                List<InfLine> sectionLines = SectionLines(sectionName);
                allSectionLines[sectionName] = sectionLines;
            }

            return allSectionLines;
        }

        // Finds the value of the given string token
        public string GetStringTokenValue(string key, string? language = null) {
            // Build the name of the section
            string sectionName = Constants.StringsSectionName;
            if (!string.IsNullOrEmpty(language)) {
                sectionName = $"{sectionName}.{language}";
            }

            // Find the strings section
            List<InfLine> stringLines = SectionLines(sectionName);
            // Can we find the key?
            InfLine? matchingLine = stringLines.SingleOrDefault(o => o.Key?.Value == key);
            if (matchingLine != null) {
                if (matchingLine.Values?.Count == 1) {
                    return matchingLine.Values[0].Value ?? string.Empty;
                }
                throw new($"Expected 1 value for string token but found {matchingLine.Values?.Count}.");
            }

            // If not match found, then just return the key
            return key;
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods
    }
}

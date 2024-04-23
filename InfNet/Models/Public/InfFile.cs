using System.Collections.Generic;
using System.Linq;

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

        // Returns a list of the section names in the file
        public List<string> SectionNames() {
            return Lines.Select(o => o.SectionName ?? string.Empty).Where(o => !string.IsNullOrEmpty(o)).ToList();
        }

        // Gets the lines for the name section
        public List<InfLine> SectionLines(string sectionMame) {
            List<InfLine> sectionLines = new();

            bool inSection = false;
            foreach (InfLine line in Lines) {
                if (line.SectionName == sectionMame) {
                    inSection = true;
                }
                else if (line.Key == null && line.Values == null) {
                    inSection = false;
                }
                else if (!string.IsNullOrEmpty(line.SectionName) && line.SectionName != sectionMame) {
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

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods
    }
}

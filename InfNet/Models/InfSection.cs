using System.Collections.Generic;
using System.Net.Http.Headers;

namespace InfNet.Models {
    public class InfSection {

        // The name of the section
        public string Name { get; set; }

        // The list of entries in the section
        public List<InfEntry> Entries { get; } = new();

        public InfSection(string name) {
            Name = name;
        }
    }
}

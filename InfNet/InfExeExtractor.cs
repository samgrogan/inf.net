namespace InfNet {
    internal class InfExeExtractor : InfFileExtractorBase {
        public InfExeExtractor(string fileName, string outputFolder) : base(fileName, outputFolder) {
        }

        public override bool ExtractInfFiles() {
            return false;
        }
    }
}

using System.IO;

namespace InfNet.Helpers {
    public static class PathHelpers {
        public static string SafePathGetExtension(string? path) {
            if (string.IsNullOrEmpty(path)) {
                return string.Empty;
            }
            try {
                string? extension = Path.GetExtension(path)?.ToLowerInvariant();
                return extension ?? string.Empty;
            }
            catch {
                return string.Empty;
            }
        }
    }
}

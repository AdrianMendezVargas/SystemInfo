using System.Text.RegularExpressions;

namespace SystemInfo.Shared.Extensions {
    public static class StringExtensions {
        public static bool IsRncValid(this string rnc) {
            return !string.IsNullOrWhiteSpace(rnc) && Regex.IsMatch(rnc , "^[0-9]{9}$");
        }
    }
}

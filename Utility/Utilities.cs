using System.Globalization;
using System.Text;

namespace MediaPanelController.Utility
{
    internal static class Utilities
    {
        public static string RemoveDiacritics(string inputString)
        {
            string normalizedString = inputString.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(normalizedString[i]);
                }
            }

            return (stringBuilder.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
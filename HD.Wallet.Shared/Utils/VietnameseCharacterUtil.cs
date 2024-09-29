using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Utils
{
	public class VietnameseCharacterUtil
	{
		public static string VietnameseToLatin(string text)
		{
			//string normalizedString = text.Normalize(NormalizationForm.FormD);

			//// Remove diacritics using a regex
			//Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			//string noDiacriticsString = regex.Replace(normalizedString, "");

			//// Replace specific Vietnamese characters that are not handled by normalization
			//noDiacriticsString = noDiacriticsString.Replace("đ", "d").Replace("Đ", "d");

			//// Convert to lowercase
			//string lowerCaseString = noDiacriticsString.ToLower();

			return text.Normalize(NormalizationForm.FormD); ;
		}

		public static string GenerateTitleDash(string title)
		{
			// Normalize the string to decompose the characters
			string normalized = title.Normalize(NormalizationForm.FormD);

			// Remove accents and special characters
			StringBuilder sb = new StringBuilder();
			foreach (char c in normalized)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(c);
				}
			}

			// Convert to lowercase, replace spaces with hyphens and remove non-alphanumeric characters
			string cleaned = sb.ToString().Normalize(NormalizationForm.FormC)
								  .ToLower()
								  .Replace(' ', '-')
								  .Replace("&", "and")
								  .Replace("đ", "d");

			return new string(cleaned.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

		}
	}
}

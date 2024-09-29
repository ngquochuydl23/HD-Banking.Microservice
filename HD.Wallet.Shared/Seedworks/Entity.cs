using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace HD.Wallet.Shared.Seedworks
{
    public abstract class Entity : IDeleteEntity, IHasCreationTime, ILastUpdatedTime
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public Entity()
        {
            CreatedAt = DateTime.Now;
        }

        protected string GenerateTitleDash(string title)
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


    public abstract class Entity<TKey> : Entity
    {
        public TKey Id { get; set; }
    }
}

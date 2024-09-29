using System.Globalization;
using System.Text;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Accounts
{
    public class AccountBankValueObject
    {
        public string BankName { get; set; }

        public string BankAccountId { get; set; }

        public string BankOwnerName { get; set; }

        public string IdCardNo { get; set; }


        // Hàm chuyển đổi chuỗi thành in hoa và không dấu
        public static string ToUpperCaseWithoutDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Chuẩn hóa chuỗi thành dạng chuẩn Unicode
            string normalizedString = input.Normalize(NormalizationForm.FormD);

            // Loại bỏ các dấu diacritics (dấu tiếng Việt)
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Chuyển thành chuỗi không dấu và in hoa
            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper();

            return result;
        }
    }
}

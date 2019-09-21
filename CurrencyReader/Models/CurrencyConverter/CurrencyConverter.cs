using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurrencyReader.Models
{
    public class CurrencyConverter
    {
        public const int MAXINPUTLENGTH = 28;
        public const decimal MININPUTNUM = -99999999999999999999999999.99m;
        public const decimal MAXINPUTNUM = 99999999999999999999999999.99m;
        public const string AND = "and ";
        public const string NEGATIVEPREFIX = "Negative ";
        public const string DOLLAR = " Dollar";
        public const string DOLLARS = " Dollars";
        public const string CENT = " Cent";
        public const string CENTS = " Cents";
        public const string HUNDRED = "Hundred";
        public const string ErrorMessage_TooLong = "Please input a number which has maximum 28 digits";
        public const string ErrorMessage_InvalidNumber = "Please enter a valid number";
        public const string ErrorMessage_InvalidRange = "The number input should between -99999999999999999999999999.99 and 99999999999999999999999999.99";

        public static readonly string[] UnitsMap =
            new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                    "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        public static readonly string[] TensMap =
            new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        public static readonly string[] ThousandsMap =
            new[] { "Zero", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion" };
        public static readonly Regex NotNumberRegex = new Regex("[^0-9]+");

        /// <summary>
        /// Number string to be converted to a currency representation
        /// </summary>
        public string InputNumber { get; set; }

        /// <summary>
        /// Convert a number string to currency representation
        /// </summary>
        /// <returns></returns>
        public OperationResult<string> Convert()
        {
            if (!TryConvertNumber(InputNumber, out decimal num, out string errorMessage))
            {
                return new OperationResult<string>()
                {
                    Data = string.Empty,
                    ResultMessage = string.IsNullOrEmpty(errorMessage) ? ErrorMessage_InvalidNumber : errorMessage,
                    IsSuccessful = false
                };
            }

            var convertResult = new OperationResult<string>()
            {
                Data = ConvertToEnglish(num),
                ResultMessage = string.Empty,
                IsSuccessful = true
            };
            return convertResult;
        }

        /// <summary>
        /// Get decimal type number from input string
        /// </summary>
        /// <remarks>
        /// Decimal maximum value is 79228162514264337593543950335 and minimum value is -79228162514264337593543950335
        /// However, since decimal value only has 28-29 digit precision, we will limit the data in the range
        /// [-99999999999999999999999999.99, 99999999999999999999999999.99]
        /// Also we will check how many digits exist in the input string, only allow 28 digits,
        /// e.g. 99999999999999999999999999.989 is not allowed because it has 29 digits.
        /// </remarks>
        /// <param name="numStr">String to be converted</param>
        /// <param name="num">Convertion result</param>
        /// <param name="errorMessage">Convertion error message, only available if convertion failed</param>
        /// <returns>Convertion succeeded or not</returns>
        private bool TryConvertNumber(string numStr, out decimal num, out string errorMessage)
        {
            num = 0m;
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(numStr))
            {
                return false;
            }

            var digitStr = NotNumberRegex.Replace(numStr, string.Empty);
            if (digitStr.Length > MAXINPUTLENGTH)
            {
                errorMessage = ErrorMessage_TooLong;
                return false;
            }

            var convertionResult = decimal.TryParse(numStr, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out num);
            if (num > MAXINPUTNUM || num < MININPUTNUM)
            {
                num = 0m;
                errorMessage = ErrorMessage_InvalidRange;
                return false;
            }

            return convertionResult;
        }

        /// <summary>
        /// Convert a decimal type currency number to English representation.
        /// </summary>
        /// <remarks>
        /// Rule:
        /// If it is a negative number, there would be a "Negative " before the digit part representation;
        /// If it is zero, it will return "Zero";
        /// If it is a positive number, consider the following circumstances:
        ///     When it is less than 1 dollar, return "xx Cent(s)"
        ///     When it is larger than or equal to 1 dollar(without decimal), return "xxx Dollar(s)"
        ///     When it is larger than or equal to 1 dollar(with decimal), return "xxx Dollar(s) and xx Cent(s)"
        /// Note:
        ///     1. Pay attention to plural form for Dollar/Dollars or Cent/Cents;
        ///     2. Round the number to two decimal places;
        ///     3. Do NOT add "and" after thousands representation ("Thousand", "Million", "Billion", etc.),
        ///        add "and" between "Hundred" and the next tens or ones place, e.g.:
        ///            130,423 is "One Hundred and Thirty Thousand Four Hundred and Twenty Three Dollars"
        ///            1,000,010,000 is "One Billion Ten Thousand Dollars";
        ///        Exception: If the hundreds place is 0, but the tens place or ones place is not 0,
        ///                   there should be "and" after the last thousands representation, e.g.:
        ///            130,023 is "One Hundred and Thirty Thousand and Twenty Three Dollars"
        /// </remarks>
        /// <param name="num">Currency number to be converted</param>
        /// <returns>English representation of the currency number</returns>
        private string ConvertToEnglish(decimal num)
        {
            var convertResult = string.Empty;

            num = Math.Round(num, 2);

            if (num < 0)
            {
                convertResult = ConvertToEnglish(decimal.Negate(num));
                convertResult = NEGATIVEPREFIX + convertResult;
                return convertResult;
            }

            if (num == 0)
            {
                convertResult = UnitsMap[0];
                return convertResult;
            }
            
            var numericPart = DollarsToEnglish(num);
            var decimalPart = CentsToEnglish(num);

            // At least one of numericPart and decimalPart will not be empty
            if (string.IsNullOrEmpty(numericPart))
            {
                return decimalPart;
            }
            else if (string.IsNullOrEmpty(decimalPart))
            {
                return numericPart;
            }
            else
            {
                return numericPart + " " + AND + decimalPart;
            }
        }

        /// <summary>
        /// Convert a positive decimal type integer to English representation.
        /// </summary>
        /// <remarks>
        /// Name	    Number of Zeros  Groups of (3) Zeros
        /// Ten         1               (10)
        /// Hundred	    2               (100)
        /// Thousand	3               1 (1,000)
        /// Million 	6               2 (1,000,000)
        /// Billion 	9               3 (1,000,000,000)
        /// Trillion	12              4 (1,000,000,000,000)
        /// Quadrillion	15              5
        /// Quintillion	18              6
        /// Sextillion	21              7
        /// Septillion	24              8
        /// Octillion	27              9
        /// </remarks>
        /// <param name="num">Decimal number to be converted</param>
        /// <returns>English representation of the number</returns>
        private string IntegerToEnglish(decimal num)
        {
            string convertResult = string.Empty;

            // Cannot use Math.Pow or Math.Log10 to deal with big decimals, get the number of digits from Scientific Notation
            var numOfDigits = int.Parse(num.ToString("E").Split(new char[] { 'E', '+' }).Last());
            while (numOfDigits >= 3)
            {
                numOfDigits = numOfDigits / 3 * 3; // Get how many groups of (,000) there are, for example, for 12,345,678,999, there are 3 groups of (,000)
                var suffixNumStr = "1E+" + numOfDigits;
                var suffixNum = decimal.Parse(suffixNumStr, System.Globalization.NumberStyles.Float);
                var numberInGroup = (int)(num / suffixNum);
                if (numberInGroup > 0)
                {
                    convertResult += IntegerLessThan1000ToEnglish(numberInGroup) + " " + ThousandsMap[numOfDigits / 3] + " ";
                }
                
                num = num - numberInGroup * suffixNum;
                numOfDigits -= 3;
            }

            #region Handle num less than 1000

            if (!string.IsNullOrEmpty(convertResult) && num < 100 && num > 0)
            {
                convertResult += AND;
            }

            convertResult += IntegerLessThan1000ToEnglish((int)num);

            #endregion

            // if the result ends with ` `, remove it
            convertResult = convertResult.TrimEnd(' ');
            return convertResult;
        }

        /// <summary>
        /// Convert an integer less than 100 to English representation. Ensure integer is less than 1000 and larger than 0
        /// </summary>
        /// <param name="num">Integer number to be converted</param>
        /// <returns>English representation of the number</returns>
        private string IntegerLessThan1000ToEnglish(int num)
        {
            string convertResult = string.Empty;
            if (num / 100 > 0)
            {
                convertResult = IntegerLessThan1000ToEnglish(num / 100) + " " + HUNDRED;
                num %= 100;
            }

            if (num > 0)
            {
                // If Hundred part exists, an `and` is required after Hundred
                if (convertResult != string.Empty)
                {
                    convertResult = convertResult + " " + AND;
                }

                if (num < 20)
                {
                    convertResult += UnitsMap[(int)num];
                }
                else
                {
                    convertResult += TensMap[(int)num / 10];
                    if (num % 10 > 0)
                    {
                        convertResult += " " + UnitsMap[(int)num % 10];
                    }
                }
            }

            return convertResult;
        }

        /// <summary>
        /// Get English representation of the integer part of a currency number
        /// </summary>
        /// <param name="num">Currency number to be converted</param>
        /// <returns>English representation of the integer part</returns>
        private string DollarsToEnglish(decimal num)
        {
            var convertResult = string.Empty;
            decimal dollars = Math.Truncate(num);
            if (dollars == 0)
            {
                return string.Empty;
            }

            convertResult = IntegerToEnglish(dollars);
            convertResult += (dollars > 1 ? DOLLARS : DOLLAR);
            return convertResult;
        }

        /// <summary>
        /// Get English representation of the decimal part of a currency number
        /// </summary>
        /// <param name="num">Currency number to be converted</param>
        /// <returns>English representation of the decimal part</returns>
        private string CentsToEnglish(decimal num)
        {
            var convertResult = string.Empty;
            decimal intPart = Math.Truncate(num);
            decimal cents = Math.Truncate((num - intPart) * 100);

            if (cents == 0)
            {
                return string.Empty;
            }

            convertResult = IntegerToEnglish(cents);
            convertResult += (cents > 1 ? CENTS : CENT);
            return convertResult;
        }
    }
}
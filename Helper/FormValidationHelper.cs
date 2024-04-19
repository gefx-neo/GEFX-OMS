using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GreatEastForex.Helper
{
    public class FormValidationHelper
    {
        public static bool EmailValidation(string email)
        {
            return Regex.IsMatch(email,
                @"^([a-zA-Z0-9])(([a-zA-Z0-9])*([\._\+-])*([a-zA-Z0-9]))*@(([a-zA-Z0-9\-])+(\.))+([a-zA-Z]{2,4})+$");
        }

        public static bool AmountValidation(string amount)
        {
            try
            {
                decimal val = Convert.ToDecimal(amount);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool NonNegativeAmountValidation(string amount)
        {
            try
            {
                decimal val = Convert.ToDecimal(amount);

                if (val >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool NonNegativeAmountValidationWithDP(string amount)
        {
            try
            {
                decimal val = Convert.ToDecimal(amount);

                if (val >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IntegerValidation(string amount)
        {
            try
            {
                int val = Convert.ToInt32(amount);

                if (val >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //AmountFormatter
        public static string AmountFormatter(decimal amount, int dp)
        {
            switch (dp)
            {
                case 0:
                    return amount.ToString("#,##0");
                case 1:
                    return amount.ToString("#,##0.0");
                case 2:
                    return amount.ToString("#,##0.00");
                case 3:
                    return amount.ToString("#,##0.000");
                case 4:
                    return amount.ToString("#,##0.0000");
                case 5:
                    return amount.ToString("#,##0.00000");
                case 6:
                    return amount.ToString("#,##0.000000");
                case 8:
                    return amount.ToString("#,##0.00000000");
                default:
                    return amount.ToString("#,##0.00");
            }
        }
    }
}
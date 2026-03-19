using System;
using System.Collections.Generic;

namespace FractionApp
{
    public class FractionProcessor
    {
        public static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        public static void ReduceFraction(int numerator, int denominator,
            out int reducedNumerator, out int reducedDenominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Знаменатель не может быть равен нулю.");

            int gcd = GCD(numerator, denominator);

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            reducedNumerator = numerator / gcd;
            reducedDenominator = denominator / gcd;
        }
        public static void ExtractWholePart(int numerator, int denominator,
            out int wholePart, out int fractionalNumerator, out int fractionalDenominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Знаменатель не может быть равен нулю.");

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            int reducedNum, reducedDen;
            ReduceFraction(numerator, denominator, out reducedNum, out reducedDen);

            wholePart = reducedNum / reducedDen;
            fractionalNumerator = reducedNum % reducedDen;
            fractionalDenominator = reducedDen;

            if (wholePart < 0 || (wholePart == 0 && reducedNum < 0))
            {
                if (fractionalNumerator != 0)
                {
                    fractionalNumerator = Math.Abs(fractionalNumerator);
                }
            }
        }
        public static (int Numerator, int Denominator) ParseFraction(string fractionString)
        {
            if (string.IsNullOrWhiteSpace(fractionString))
                throw new ArgumentException("Строка не может быть пустой.");

            string[] parts = fractionString.Split('/');
            if (parts.Length != 2)
                throw new FormatException("Неверный формат дроби. Ожидается: числитель/знаменатель");

            if (!int.TryParse(parts[0].Trim(), out int numerator) ||
                !int.TryParse(parts[1].Trim(), out int denominator))
            {
                throw new FormatException("Числитель и знаменатель должны быть целыми числами.");
            }

            return (numerator, denominator);
        }
        public static string GetReducedFractionString(int numerator, int denominator)
        {
            int redNum, redDen;
            ReduceFraction(numerator, denominator, out redNum, out redDen);

            if (redDen == 1)
                return redNum.ToString();
            else
                return $"{redNum}/{redDen}";
        }
        public static string GetMixedFractionString(int numerator, int denominator)
        {
            int whole, fracNum, fracDen;
            ExtractWholePart(numerator, denominator, out whole, out fracNum, out fracDen);

            if (fracNum == 0)
                return whole.ToString();
            else if (whole == 0)
                return $"{fracNum}/{fracDen}";
            else
                return $"{whole} {Math.Abs(fracNum)}/{fracDen}";
        }
        public static List<string> ProcessFractions(List<string> fractionLines)
        {
            var results = new List<string>();

            foreach (string line in fractionLines)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        results.Add("ПУСТАЯ СТРОКА -> пропущено");
                        continue;
                    }
                    var (num, den) = ParseFraction(line.Trim());

                    string reduced = GetReducedFractionString(num, den);
                    string mixed = GetMixedFractionString(num, den);

                    results.Add($"{line.Trim()} -> сокращенная: {reduced}, смешанная: {mixed}");
                }
                catch (Exception ex)
                {
                    results.Add($"{line.Trim()} -> ОШИБКА: {ex.Message}");
                }
            }
            return results;
        }
    }
}
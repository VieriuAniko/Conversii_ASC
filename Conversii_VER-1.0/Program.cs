
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Conversii_VER_1._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int b1, b2;
            decimal numberBase10;

            Console.WriteLine($"Numar cu virgula? 1 pentru 'DA' sau 0 pentru 'NU'");
            int zecimal = int.Parse(Console.ReadLine()); //doar 1 sau 0

            if (zecimal != 1 && zecimal != 0)
            {
                Console.WriteLine("Invalid");
            }
            else
            {
                Console.WriteLine("Introduceti baza sursa: ");
                b1 = int.Parse(Console.ReadLine());

                if (b1 < 2 || b1 > 16)
                {
                    Console.WriteLine("Baza sursa nu este valida. Introduceti o baza care apartine intervalului [2, 16]");
                    return;
                }

                Console.WriteLine("Introduceti numarul pe care doriti sa il convertiti: ");
                string numar = Console.ReadLine();

                if (zecimal == 0)
                {
                    numberBase10 = ConversieBazaSursain10(b1, numar);
                }
                else
                {
                    Console.WriteLine("Introduceti partea intreaga a numarului zecimal: ");
                    int parteIntreaga = int.Parse(Console.ReadLine());

                    Console.WriteLine("Introduceti partea fractionara a numarului zecimal: ");
                    string parteFractionara = Console.ReadLine();

                    numberBase10 = ConversieNumarZecimalInBaza10(b1, parteIntreaga, parteFractionara);
                }

                Console.WriteLine($"Introduceti baza tinta: ");
                b2 = int.Parse(Console.ReadLine());

                string result = ConversieBaza10InBazaTinta(b2, numberBase10);
                Console.WriteLine($"Numarul in baza {b2} este {result}");
            }
        }

        private static string ConversieBaza10InBazaTinta(int b2, decimal numar)
        {
            string digits = "0123456789ABCDEF";
            StringBuilder result = new StringBuilder();

            int parteIntreaga = (int)Math.Floor(numar);
            decimal parteFractionara = numar - parteIntreaga;

            result.Append(ConversieBaza10InBazaTintaParteIntreaga(b2, parteIntreaga, digits));

            if (parteFractionara > 0)
            {
                result.Append('.');
                result.Append(ConversieBaza10InBazaTintaParteFractionara(b2, parteFractionara, digits));
            }

            return result.ToString();
        }

        private static string ConversieBaza10InBazaTintaParteIntreaga(int b2, int parteIntreaga, string digits)
        {
            StringBuilder result = new StringBuilder();

            while (parteIntreaga != 0)
            {
                int rest = parteIntreaga % b2;
                result.Insert(0, digits[rest]);
                parteIntreaga /= b2;
            }

            return result.ToString();
        }

        private static string ConversieBaza10InBazaTintaParteFractionara(int b2, decimal parteFractionara, string digits)
        {
            StringBuilder result = new StringBuilder();
            int maxDecimals = 10; // precizia

            while (parteFractionara > 0 && maxDecimals > 0)
            {
                parteFractionara *= b2;
                int cifra = (int)Math.Floor(parteFractionara);
                result.Append(digits[cifra]);
                parteFractionara -= cifra;
                maxDecimals--;
            }

            return result.ToString();
        }

        private static decimal ConversieBazaSursain10(int b1, string numar)
        {
            string digits = "0123456789ABCDEF";
            decimal result = 0;

            int indexPunct = numar.IndexOf('.');

            if (indexPunct >= 0)
            {
                string parteIntreaga = numar.Substring(0, indexPunct);
                string parteFractionara = numar.Substring(indexPunct + 1);

                result = ConversieBazaSursain10ParteIntreaga(b1, parteIntreaga, digits) +
                         ConversieBazaSursain10ParteFractionara(b1, parteFractionara, digits);
            }
            else
            {
                result = ConversieBazaSursain10ParteIntreaga(b1, numar, digits);
            }

            return result;
        }

        private static decimal ConversieBazaSursain10ParteIntreaga(int b1, string parteIntreaga, string digits)
        {
            decimal result = 0;

            for (int i = 0; i < parteIntreaga.Length; i++)
            {
                int digit = digits.IndexOf(parteIntreaga[i], 0, b1);

                if (digit == -1)
                {
                    Console.WriteLine("Numarul nu este corect");
                    Environment.Exit(0);
                }

                result = result * b1 + digit;
            }

            return result;
        }

        private static decimal ConversieBazaSursain10ParteFractionara(int b1, string parteFractionara, string digits)
        {
            decimal result = 0;

            for (int i = parteFractionara.Length - 1; i >= 0; i--)
            {
                int digit = digits.IndexOf(parteFractionara[i], 0, b1);

                if (digit == -1)
                {
                    Console.WriteLine("Numarul nu este corect");
                    Environment.Exit(0);
                }

                result = (result + digit) / b1;
            }

            return result;
        }

        private static decimal ConversieNumarZecimalInBaza10(int b1, int parteIntreaga, string parteFractionara)
        {
            decimal parteFractionaraDecimal = ConversieBazaSursain10ParteFractionara(b1, parteFractionara, "0123456789");
            return parteIntreaga + parteFractionaraDecimal;
        }
    }
}

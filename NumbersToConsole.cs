using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik_1_homework_1
{
    class FifthProblem
    {
        static string OneDigit(int d)
        {
            string str = "";
            switch (d)
            {
                case 1: str = "one "; break;
                case 2: str = "two "; break;
                case 3: str = "three "; break;
                case 4: str = "four "; break;
                case 5: str = "five "; break;
                case 6: str = "six "; break;
                case 7: str = "seven "; break;
                case 8: str = "eight "; break;
                case 9: str = "nine "; break;
            }
            return str;

        }

        static string TwoDigit(int d)
        {
            string str = "";
            switch (d)
            {
                case 2: str = "twenty "; break;
                case 3: str = "thirty "; break;
                case 4: str = "forty "; break;
                case 5: str = "fifty "; break;
                case 6: str = "sixty "; break;
                case 7: str = "seventy "; break;
                case 8: str = "eighty "; break;
                case 9: str = "ninety "; break;
            }
            return str;

        }

        static string EnDigit(int d)
        {
            string str = "";
            switch (d)
            {
                case 10: str = "ten "; break;
                case 11: str = "eleven "; break;
                case 12: str = "twelve "; break;
                case 13: str = "thirteen "; break;
                case 14: str = "fourteen "; break;
                case 15: str = "fifteen "; break;
                case 16: str = "sixteen "; break;
                case 17: str = "seventeen "; break;
                case 18: str = "eighteen "; break;
                case 19: str = "nineteen "; break;
            }
            return str;

        }


        // /////////---MAIN()--///////////////////////////
        static void Main()
        {
            int n = new int();
            do
            {
                Console.Write("Number:");
                n = Int32.Parse(Console.ReadLine());
            }
            while (n < 0 || n > 999);
            int d1 = new int();
            int d2 = new int();
            int d3 = new int();

            if (n < 10)
            {
                Console.WriteLine(OneDigit(n));
            }

            if (n >= 10 && n <= 19)
            {
                Console.WriteLine(EnDigit(n));
            }

            if (n > 19 && n < 100)
            {

                d1 = n / 10;
                d2 = n % 10;
                Console.WriteLine(TwoDigit(d1) + OneDigit(d2));
            }

            if (n >= 100)
            {
                d1 = n / 100;
                int t = n / 10;
                d2 = t % 10;
                d3 = n % 10;
                int en = n % 100;//en=last two digits. ex 414 => en=14
                if (d2 == 0 && d3 == 0) //ex. X00
                {
                    Console.WriteLine(OneDigit(d1) + "hundred ");
                }
                else
                {
                    if (d2 == 0 && d3 != 0) //ex.X03
                    {
                        Console.WriteLine(OneDigit(d1) + "hundred and " + OneDigit(d3));
                    }
                    else
                    {
                        if (en >= 10 && en <= 19) //from X10 - X19
                        {
                            Console.WriteLine(OneDigit(d1) + "hundred and " + EnDigit(en));
                        }
                        else
                        {
                            if (en != 10 && d3 == 0)  //ex.XY0  Y!=0
                            {
                                Console.WriteLine(OneDigit(d1) + "hundred and " + TwoDigit(d2));
                            }
                            else
                            {
                                Console.WriteLine(OneDigit(d1) + "hundred " + TwoDigit(d2) + OneDigit(d3));
                            }
                        }
                    }
                }
            }
        }
    }
}

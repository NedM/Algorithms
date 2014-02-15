using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class MasterMethod
    {
        public static void Evaluate()
        {
            string order = string.Empty;
            int a, b, d;

            Console.WriteLine(string.Format("{0}++++++++++++++++++++++++++++{0}+ T(n) <= aT(n/b) + O(n^d) +{0}++++++++++++++++++++++++++++", Environment.NewLine));

            Console.WriteLine("'a' represents the number of recursive calls.\n(should be >= 1)");
            Console.Write("a: ");
            string aString = Console.ReadLine();
            int.TryParse(aString, out a);
            if (a < 1)
            {
                throw new ArgumentOutOfRangeException("a", "'a' should be greater than or equal to 1!");
            }
            
            Console.WriteLine("'b' represents the factory by which the size of the subproblems are reduced.\n(should be > 1)");
            Console.Write("b: ");
            string bString = Console.ReadLine();
            int.TryParse(bString, out b);
            if (b <= 1)
            {
                throw new ArgumentOutOfRangeException("b", "'b' should be greater than 1!");
            }

            Console.WriteLine("'d' reresents the exponent running time of the combine step.\n(should be >= 0)");
            Console.Write("d: ");
            string dString = Console.ReadLine();
            int.TryParse(dString, out d);
            if (d < 0)
            {
                throw new ArgumentOutOfRangeException("d", "'d' should be greater than or equal to 0!");
            }

            if (a == Math.Pow(b, d))
            {
                order = "(n^d)log(n)";
            }
            else if (a < Math.Pow(b, d))
            {
                order = "n^d";
            }
            else
            {
                order = "n^(log[base b](a))";
            }

            Console.WriteLine(string.Format("{1}Algorithm running time is O({0}){1}", order, Environment.NewLine));
        }

        public static void PrintAssumptions()
        {
            Console.WriteLine("Assumptions for using the master method:");
            Console.WriteLine("Assuption: All subproblems have equal input size.");
            Console.WriteLine("Assuption: T(n) is < or = a constant for sufficiently small input size, n.");
        }
    }
}

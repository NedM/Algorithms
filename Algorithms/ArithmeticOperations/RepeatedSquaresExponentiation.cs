using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.ArithmeticOperations
{
    public static class RepeatedSquaresExponentiation
    {
        public static int Exp(this int exponentBase, int exponentValue)
        {
            double result = ((double) exponentBase).Exp(exponentValue);

            if (result > Int32.MaxValue)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot return result of {0}^{1} because it is too large for an Int32.",
                                  exponentBase,
                                  exponentValue));
            }

            return Convert.ToInt32(result);
        }

        public static double Exp(this double exponentBase, int exponentValue)
        {
            if (exponentValue < 0)
            {
                throw new ArgumentOutOfRangeException("exponentValue", "exponent must be non-negative");
            }

            if (exponentValue == 0)
            {
                return 0;
            }

            double repeatedSquare = exponentBase;
            double partialResult = 1;
            int[] expArray = exponentValue.ToBinaryArray();
            int lastIndexInArray = expArray.Length - 1;

            for (int i = lastIndexInArray; i >= 0; i--)
            {
                if (expArray[i] == 1)
                {
                    partialResult = partialResult * repeatedSquare;
                }

                repeatedSquare = Math.Pow(repeatedSquare, 2);
            }

            if (partialResult > double.MaxValue)
            {
                throw new InvalidOperationException(string.Format("Cannot return result of {0}^{1} because it is too large for an double."));
            }

            return partialResult;
        }
    }
}

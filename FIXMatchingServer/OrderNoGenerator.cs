using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIXMatchingServer
{
    public class OrderNoGenerator
    {
        private readonly char[] numberList = new char[]{ '0','1','2','3','4','5','6','7','8','9',
                                    'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z' };

        private int[] numberIndex = new int[5];

        public OrderNoGenerator()
        {
            for( int i = 0; i < 5; ++i)
            {
                numberIndex[i] = 0;
            }

            numberIndex[4] = 36;
        }

        public string GetOrderNo()
        {
            string orderNo = string.Format("{0}{1}{2}{3}{4}", numberList[numberIndex[4]], numberList[numberIndex[3]], numberList[numberIndex[2]], numberList[numberIndex[1]], numberList[numberIndex[0]]);

            int processIndex = 0;
            while (processIndex < 5) {
                ++numberIndex[processIndex];
                if (numberIndex[processIndex] >= 62)
                {
                    numberIndex[processIndex] = 0;

                    ++processIndex;
                }
                else
                {
                    break;
                }
            }

            return orderNo;
        }
    }
}

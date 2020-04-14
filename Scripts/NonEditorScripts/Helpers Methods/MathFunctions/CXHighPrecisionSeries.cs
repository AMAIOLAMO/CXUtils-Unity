using System;
using System.Collections.Generic;

namespace CXUtils.CXHighPrecisionSeries
{
    ///<summary>
    ///Cx's High Precision int Class
    ///</summary>
    public class HPInt
    {
        public List<int> ValList = new List<int>();
        public HPInt(string value)
        {
            ValList.Clear();
            for (int i = 0; i < value.Length; i++) ValList.Add(value[i]);
        }
        public HPInt(int value = 0)
        {
            ValList.Clear();
            do
            {
                ValList.Add(value % 10);
                value /= 10;

            } while (value != 0);

        }
        public int ToInt()
        {
            int outPutInt = 0, digit = 1;
            for (int i = 0; i < ValList.Count; i++)
            {
                outPutInt += ValList[i] * digit;
                digit *= 10;
            }
            return outPutInt;
        }
        /// <summary>
        /// adds the two HpInt together
        /// </summary>
        /// <param name="a">The first to add</param>
        /// <param name="b">The second to add</param>
        /// <returns>Added Hp int</returns>
        public static HPInt operator +(HPInt a, HPInt b)
        {
            HPInt ans = new HPInt();
            int Excesive = 0;
            int i, end = Math.Min(a.ValList.Count, b.ValList.Count);
            //loop and add
            for (i = 0; i < end; i++)
            {
                ans.ValList[i] = a.ValList[i] + b.ValList[i] + Excesive;
                Excesive = ans.ValList[i] / 10;

                if (i == (end - 1) && Excesive > 0) end++;
            }
            return ans;
        }
        /// <summary>
        /// adds the hpint with the int together
        /// </summary>
        /// <param name="a">The first to add</param>
        /// <param name="b">The second to add</param>
        /// <returns></returns>
        public static HPInt operator +(HPInt a, int b)
        {
            HPInt ans = new HPInt(b);
            ans += a;
            return ans;
        }
    }

}
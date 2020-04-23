using System;
using System.Collections.Generic;

namespace CXUtils.Experimental.HighPrecisionUtils
{
    ///<summary> Cx's High Precision int Class </summary>
    public class HPInt
    {
        public List<int> ValList = new List<int>();

        public HPInt(string value)
        {
            ValList.Clear();

            for (int i = 0; i < value.Length; i++)
            {
                if (int.TryParse(value[i].ToString(), out int res))
                {
                    ValList.Add(res);
                    continue;
                }

                throw new NotFiniteNumberException("Value not intended");
            }
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

        #region Script Methods
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


        #region Operators
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

                if (i == (end - 1) && Excesive > 0)
                    end++;
            }
            return ans;
        }

        public static HPInt operator +(HPInt a, int b) =>
            new HPInt(b) + a;
        #endregion

        #endregion
    }
}
/*******************************************************************
 * Name: XUNJIN WANG
 * Program: Business Information Technology
 * Course: ADEV-3008 Programming 3
 * Created: 1-12-2021
 * Updated: 1-12-2021
 * TODO:
 * - .
 *******************************************************************/

namespace Utility
{
    /// <summary>
    /// Provides substring process.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Abstract substring.
        /// </summary>
        /// <param name="name">Input string.</param>
        /// <param name="ch">Index character.</param>
        /// <returns></returns>
        public static string SubString(string name, char ch)
        {
            int index = name.IndexOf(ch);
            return name.Substring(0, index);
        }
    }
}
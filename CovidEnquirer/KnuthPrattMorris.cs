using System;

namespace CovidEnquirer
{
    internal class KnuthPrattMorris
    {
        #region Private Properties

        private int[] _LpsArray;
        private string _Pattern;

        #endregion Private Properties

        #region Ctor

        public KnuthPrattMorris(string pattern)
        {
            _Pattern = pattern;
            _LpsArray = new int[pattern.Length];
            // length of the previous longest prefix suffix 
            int len = 0;
            int i = 1;
            _LpsArray[0] = 0; // lps[0] is always 0 

            // the loop calculates lps[i] for i = 1 to M-1 
            while (i < pattern.Length)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    _LpsArray[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len]) 
                {
                    // This is tricky. Consider the example. 
                    // AAACAAAA and i = 7. The idea is similar 
                    // to search step. 
                    if (len != 0)
                    {
                        len = _LpsArray[len - 1];

                        // Also, note that we do not increment 
                        // i here 
                    }
                    else // if (len == 0) 
                    {
                        _LpsArray[i] = len;
                        i++;
                    }
                }
            }
        }

        #endregion Ctor

        #region Public Methods

        /// <summary>
        /// Searches for pattern occurence in text using Knuth Pratt Morris
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public bool Search(string txt)
        {
            int M = _Pattern.Length;
            int N = txt.Length;

            // create lps[] that will hold the longest 
            // prefix suffix values for pattern 
            int j = 0; // index for pattern[]
            int i = 0; // index for txt[] 
            while (i < N)
            {
                if (_Pattern[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if (j == M)
                {
                    return true;
                }

                // mismatch after j matches 
                else if (i < N && _Pattern[j] != txt[i])
                {
                    // Do not match lps[0..lps[j-1]] characters, 
                    // they will match anyway 
                    if (j != 0)
                    {
                        j = _LpsArray[j - 1];
                    }
                    else
                    {
                        i = i + 1;
                    }
                }
            }
          
            return false;
        }

        #endregion Public Methods
    }

}

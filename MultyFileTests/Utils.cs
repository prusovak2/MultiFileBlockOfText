using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MultyFileTests
{
    public static class Utils
    {
        public static bool Diff(string A, string B)
        {
            if (A.Length != B.Length)
                return false;

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i])
                {
                    Console.WriteLine($"Utils.Diff: Difference at index {i}");
                    return false;
                }
            }
            return true;
        }

        public static bool FileDiff(string A, string B)
        {
            return Diff(File.ReadAllText(A), File.ReadAllText(B));
        }
    }
}

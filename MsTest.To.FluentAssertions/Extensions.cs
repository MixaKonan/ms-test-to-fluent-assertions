﻿namespace MsTest.To.FluentAssertions
{
    public static class Extensions
    {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            }

            var indexes = new List<int>();
            for (var index = 0;; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.Ordinal);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
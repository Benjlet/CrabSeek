using System;
using System.Linq;

namespace CrabSeek.UnitTests
{
    internal static class TestExtensions
    {
        public static bool IsOneOf<T>(this T obj, params T[] options)
        {
            return options?.Contains(obj) ?? false;
        }
    }
}

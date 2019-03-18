using System.Text.RegularExpressions;

namespace Company.Function
{
    public static class RegexFind
    {
        public static string FindString(string text, string pattern, string searchingFor = null)
        {
            Match m = Regex.Match(text, pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            System.Console.WriteLine($"Text: {text}, Pattern: {pattern}");

            if (m.Success)
            {
                var groupCount = m.Groups.Count;
                System.Console.WriteLine($"Matched length: {m.Value.Length} match group count: {m.Groups.Count}");
                if (groupCount > 1)
                {
                    groupCount--;
                    System.Console.WriteLine(m.Groups[groupCount]);
                    return m.Groups[groupCount].ToString();
                }
                System.Console.WriteLine("Found '{0}' at position {1}.", m.Value, m.Index);
            }
            else
            {
                System.Console.WriteLine($"Error: could not find {searchingFor}");
                throw new System.Exception($"Error: could not find {searchingFor}");
            }


            return m.Value;
        }

        public static string[] Split(string input, string pattern)
        {
            try
            {
                return Regex.Split(input, pattern);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw e;
            }
        }
    }


}

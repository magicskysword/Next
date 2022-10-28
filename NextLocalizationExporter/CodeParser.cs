using System.Text.RegularExpressions;

namespace NextLocalizationExporter;

public static class CodeParser
{
    public static IEnumerable<string> ParseI18NTodoString(string input)
    {
        var regex = new Regex(@"""(\\""|""""|[^""])*"".I18NTodo\(\)");
        var matches = regex.Matches(input);
        foreach (Match match in matches)
        {
            var matchString = match.Value;
            var startIndex = matchString.IndexOf('"') + 1;
            var endIndex = matchString.LastIndexOf('"');
            var length = endIndex - startIndex;
            var result = matchString.Substring(startIndex, length);
            // if(matchString.StartsWith("@"))
            // {
            //     result = result.Replace("\n", "\\n");
            //     result = result.Replace("\"", "\\\"");
            // }
            yield return result;
        }
    }
    
    public static string? ParseNamespace(string input)
    {
        var regex = new Regex(@"namespace\s+([_\p{L}][0-9_\p{L}.]*);");
        var match = regex.Match(input);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return null;
    }
}
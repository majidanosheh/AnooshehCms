using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApplication16.Services
{
    public class ShortcodeService : IShortcodeService
    {
        private readonly IFormService _formService;
        private readonly IRazorViewToStringRenderer _renderer;
        private static readonly Regex FormShortcodeRegex = new Regex(@"\[form slug=""([\w-]+)""\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ShortcodeService(IFormService formService, IRazorViewToStringRenderer renderer)
        {
            _formService = formService;
            _renderer = renderer;
        }

        public async Task<string> ProcessShortcodesAsync(string? content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            // Using MatchEvaluator for cleaner and more efficient replacement
            string processedContent = await FormShortcodeRegex.ReplaceAsync(content, async (match) =>
            {
                var slug = match.Groups[1].Value;
                var form = await _formService.GetFormBySlugWithFieldsAsync(slug);

                if (form != null && form.IsActive)
                {
                    // The view path must be specific and start from the root
                    return await _renderer.RenderViewToStringAsync("~/Views/FormRenderer/Display.cshtml", form);
                }

                // If form not found or inactive, replace shortcode with an HTML comment
                return $"<!-- Form with slug '{slug}' not found or is inactive -->";
            });

            return processedContent;
        }
    }
}
// Helper extension for async Regex.Replace
public static class RegexExtensions
{
    public static async Task<string> ReplaceAsync(this Regex regex, string input, Func<Match, Task<string>> replacementFn)
    {
        var matches = regex.Matches(input);
        if (matches.Count == 0) return input;

        var sb = new System.Text.StringBuilder();
        var lastIndex = 0;

        foreach (Match match in matches)
        {
            sb.Append(input, lastIndex, match.Index - lastIndex);
            var replacement = await replacementFn(match);
            sb.Append(replacement);
            lastIndex = match.Index + match.Length;
        }
        sb.Append(input, lastIndex, input.Length - lastIndex);

        return sb.ToString();
    }
}


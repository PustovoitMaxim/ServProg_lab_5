using System.Text.RegularExpressions;

namespace SocialApp.Helpers
{
    public static class HashtagHelper
    {
        
        public static List<string> ExtractHashtags(string postContent)
        {
            if (string.IsNullOrEmpty(postContent)) 
                return new List<string>();
            
            var regex = new Regex(@"#\w+");
            return regex.Matches(postContent)
                        .Cast<Match>()
                        .Select(m => m.Value.TrimStart('#'))
                        .Distinct()
                        .ToList();
        }

        
        public static string ConvertHashtagsToLinks(string content)
        {
            if (string.IsNullOrEmpty(content)) 
                return content;
            
            return Regex.Replace(content, @"#(\w+)", 
                "<a href='/ponds/posts/$1' class='hashtag-link' title='Показать посты с тегом #$1' style='color: #0d6efd; text-decoration: none; font-weight: 500;'>#$1</a>");
        }

        
        public static bool IsValidHashtag(string tag)
        {
            return !string.IsNullOrEmpty(tag) && Regex.IsMatch(tag, @"^#?\w+$");
        }

        
        public static string NormalizeHashtag(string tag)
        {
            return tag?.TrimStart('#').ToLower() ?? string.Empty;
        }
    }
}
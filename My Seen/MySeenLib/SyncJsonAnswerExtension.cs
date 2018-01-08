namespace MySeenLib
{
    public static class SyncJsonAnswerExtension
    {
        public static string SplitByWords(this MySeenWebApi.SyncJsonAnswer.Values value)
        {
            var s = value.ToString();
            var ss = string.Empty;

            for (var index = 0; index < s.Length; index++)
            {
                var c = s[index];
                if (index != 0 && char.IsUpper(c)) ss += " ";
                ss += c.ToString();
            }
            return ss;
        }
    }
}
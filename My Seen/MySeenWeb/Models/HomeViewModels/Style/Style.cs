namespace MySeenWeb.Models.Style
{
    public class Style
    {
        public int Theme { get; set; }

        public int AnimationEnabled { get; set; }

        public Style(int theme, int animationEnabled)
        {
            Theme = theme;
            AnimationEnabled = animationEnabled;
        }
    }
}
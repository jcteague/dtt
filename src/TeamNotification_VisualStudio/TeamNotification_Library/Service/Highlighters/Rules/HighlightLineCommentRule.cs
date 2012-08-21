namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class HighlightLineCommentRule
    {
        public string LineStart { get; private set; } 
        public RuleOptions Options { get; private set; }

        public HighlightLineCommentRule()
        {
            LineStart = "//";
            Options = new RuleOptions("#999999", "Normal", "Normal");
        }
    }
}
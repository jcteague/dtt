using System;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class HighlightLongCommentRule
    {
        public Tuple<string, string> Delimiters { get; private set; }
        public RuleOptions Options { get; private set; }

        public HighlightLongCommentRule()
        {
            Delimiters = new Tuple<string, string>("/*", "*/");
            Options = new RuleOptions("#999999", "Normal", "Normal");
        }
    }
}
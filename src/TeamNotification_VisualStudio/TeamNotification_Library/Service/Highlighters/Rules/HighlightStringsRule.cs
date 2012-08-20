namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class HighlightStringsRule
    {
        public RuleOptions Options { get; private set; }
        public string Expression { get; private set; }

        public HighlightStringsRule()
        {
            Options = new RuleOptions("#FF0000", "Normal", "Normal");
            Expression = "\"(?:[^\"\\\\]+|\\\\.)*\"";
        }
    }
}
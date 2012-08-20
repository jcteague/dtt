namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class HighlightNumbersRule
    {
        public RuleOptions Options { get; private set; }
        public string Expression { get; private set; }

        public HighlightNumbersRule()
        {
            Options = new RuleOptions("#0000FF", "Normal", "Normal");
            Expression = "\b([0-9]+)\b";
        }
    }
}
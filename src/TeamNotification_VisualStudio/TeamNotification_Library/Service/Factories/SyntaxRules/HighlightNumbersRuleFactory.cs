using TeamNotification_Library.Service.Highlighters.Rules;

namespace TeamNotification_Library.Service.Factories.SyntaxRules
{
    public class HighlightNumbersRuleFactory : ICreateInstances<HighlightNumbersRule>
    {
        public HighlightNumbersRule GetInstance()
        {
            return new HighlightNumbersRule();
        }
    }
}
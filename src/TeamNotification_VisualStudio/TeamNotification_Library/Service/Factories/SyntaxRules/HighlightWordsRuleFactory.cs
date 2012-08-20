using TeamNotification_Library.Service.Highlighters;
using TeamNotification_Library.Service.Highlighters.Rules;

namespace TeamNotification_Library.Service.Factories.SyntaxRules
{
    public class HighlightWordsRuleFactory : ICreateInstances<HighlightWordsRule>
    {
        public HighlightWordsRule GetInstance()
        {
            return new HighlightWordsRule();
        }
    }
}
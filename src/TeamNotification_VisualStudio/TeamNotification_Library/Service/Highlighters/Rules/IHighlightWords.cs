﻿using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public interface IHighlightWords : IFormatSyntaxAccordingToRule
    {
        int Format(FormattedText text, int previousBlockCode);
    }
}
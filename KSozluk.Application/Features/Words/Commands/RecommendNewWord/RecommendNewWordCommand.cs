﻿using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.RecommendNewWord
{
    public class RecommendNewWordCommand : CommandBase<RecommendNewWordResponse>
    {
        public string WordContent { get; set; }
        public List<string> DescriptionContent { get; set; }
    }
}

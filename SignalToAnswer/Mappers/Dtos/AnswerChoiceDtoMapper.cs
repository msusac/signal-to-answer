﻿using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using System.Collections.Generic;

namespace SignalToAnswer.Mappers.Dtos
{
    public class AnswerChoiceDtoMapper
    {
        public List<AnswerChoiceDto> Map(Question question)
        {
            var items = new List<AnswerChoiceDto>();
            question.AnswerChoices.ForEach(ac =>items.Add(new AnswerChoiceDto(ac)));

            return items;
        }
    }
}

﻿namespace Grove.Ai.TimingRules
{
  using Core;
  using Gameplay.States;

  public class ManaConverter : TimingRule
  {
    private readonly int _relativeCost;

    private ManaConverter() {}

    public ManaConverter(int relativeCost)
    {
      _relativeCost = relativeCost;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      // currently somehow limited because of performance reasons      
      if (Turn.Step != Step.FirstMain && Turn.Step != Step.SecondMain)
        return false;

      var availableMana = p.Controller.GetConvertedMana() - _relativeCost;

      // only cards in hand
      foreach (var card in p.Controller.Hand)
      {
        if (card.ManaCost.Converted <= availableMana && !p.Controller.HasMana(card.ManaCost))
        {
          return true;
        }
      }

      return false;
    }
  }
}
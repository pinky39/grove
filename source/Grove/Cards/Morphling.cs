﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.RepetitionRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class Morphling : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Morphling")
        .ManaCost("{3}{U}{U}")
        .Type("Creature Shapeshifter")
        .Text(
          "{U}: Untap Morphling.{EOL}{U}: Morphling gains flying until end of turn.{EOL}{U}: Morphling gains shroud until end of turn.{EOL}{1}: Morphling gets +1/-1 until end of turn.{EOL}{1}: Morphling gets -1/+1 until end of turn.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Untap Morphling.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new SecondMain());
            p.TimingRule(new OwningCardHas(c => c.IsTapped));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Morphling gains flying until end of turn.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new Steps(Step.BeginningOfCombat));
            p.TimingRule(new OwningCardHas(c => !c.Has().Flying));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Morphling gains shroud until end of turn.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Shroud) {UntilEot = true});

            p.TimingRule(new OwningCardHas(c => !c.Has().Shroud));
            p.TimingRule(new GainHexproof());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{1}: Morphling gets +1/-1 until end of turn.";
            p.Cost = new PayMana(1.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, -1) {UntilEot = true});
            p.TimingRule(new OwningCardHas(c => c.Toughness > 1 && c.Toughness <= 3));
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, -1));
            p.RepetitionRule(new MaxRepetitions(2));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{1}: Morphling gets -1/+1 until end of turn.";
            p.Cost = new PayMana(1.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(-1, 1) {UntilEot = true});
            p.TimingRule(new OwningCardHas(c => c.Toughness >= 3));
            p.TimingRule(new IncreaseOwnersPowerOrToughness(-1, 1));
            p.RepetitionRule(new MaxRepetitions(4));
          });
    }
  }
}
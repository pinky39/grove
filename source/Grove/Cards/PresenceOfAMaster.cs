namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class PresenceOfTheMaster : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Presence of the Master")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text("Whenever a player casts an enchantment spell, counter it.")
        .FlavorText("Peace to all. Peace be all.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a player casts an enchantment spell, counter it.";
            p.Trigger(new OnCastedSpell((_, spell) => spell.Is().Enchantment));
            p.Effect = () => new CounterTopSpell();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
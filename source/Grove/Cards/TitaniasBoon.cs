namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class TitaniasBoon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Titania's Boon")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("Put a +1/+1 counter on each creature you control.")
        .FlavorText(
          "When the winds rock the trees, listen for voices in the creaking of the trunks. If you hear your name, you are one of the Goddess's chosen.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              permanentFilter: (e, c) => c.Is().Creature,
              controlledBy: ControlledBy.SpellOwner,
              modifiers: () => new AddCounters(() => new PowerToughness(1, 1), 1));

            p.TimingRule(new FirstMain());
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class LightningDragon : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lightning Dragon")
        .ManaCost("{2}{R}{R}")
        .Type("Creature - Dragon")
        .Text(
          "{Flying};{echo} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{R}: Lightning Dragon gets +1/+0 until end of turn.")
        .Power(4)
        .Toughness(4)
        .Echo("{2}{R}{R}")
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{R}: Lightning Dragon gets +1/+0 until end of turn.";
            p.Cost = new PayMana(Mana.Red, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 0) {UntilEot = true});
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, 0));
          });
    }
  }
}
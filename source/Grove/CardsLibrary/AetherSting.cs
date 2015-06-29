namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;


  public class AetherSting : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aether Sting")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text("Whenever an opponent casts a creature spell, Aether Sting deals 1 damage to that player.")
        .FlavorText("When Gatha fell, he was called to account for each abomination he created.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent casts a creature spell, Aether Sting deals 1 damage to that player.";

            p.Trigger(new OnCastedSpell((c, ctx) =>
                ctx.Opponent == c.Controller && c.Is().Creature));

            p.Effect = () => new DealDamageToPlayer(1, P(e => e.Controller.Opponent));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
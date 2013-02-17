namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class RuneofProtectionBlack : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rune of Protection: Black")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cast(p => p.TimingRule(new FirstMain()))
        .Cycling("{2}")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.";
            p.Cost = new PayMana(ManaAmount.White, ManaUsage.Abilities);
            p.Effect = () => new PreventNextDamageFromSourceToController();
            
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.HasColors(ManaColors.Black)).On.BattlefieldOrStack();
                trg.Text = "Select damage source.";
              });

            p.TargetingRule(new PreventDamageFromSourceToController());
          });
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class RuneOfProtectionBlack : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rune of Protection: Black")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .Cycling("{2}")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.";
            p.Cost = new PayMana(Mana.White);
            p.Effect = () => new PreventDamageFromSourceToController();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.HasColor(CardColor.Black)).On.BattlefieldOrStack(),
              trg => { trg.Message = "Select damage source."; });

            p.TargetingRule(new EffectPreventDamageFromSourceToController());
          });
    }
  }
}
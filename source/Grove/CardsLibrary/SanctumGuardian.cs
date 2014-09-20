namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;

  public class SanctumGuardian : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanctum Guardian")
        .ManaCost("{1}{W}{W}")
        .Type("Creature Human Cleric")
        .Text(
          "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.")
        .FlavorText("'Protect our mother in her womb.'")
        .Power(1)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.";
            p.Cost = new Sacrifice();
            p.Effect = () => new PreventDamageFromSourceToTarget();
            p.TargetSelector
              .AddEffect(trg =>
                {
                  trg.Is.Card().On.BattlefieldOrStack();
                  trg.Message = "Select damage source.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.CreatureOrPlayer().On.Battlefield();
                  trg.Message = "Select creature or player.";
                });
            p.TargetingRule(new EffectPreventDamageFromSourceToTarget());
          });
    }
  }
}
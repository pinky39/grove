namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class DevouringLight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Devouring Light")
        .ManaCost("{1}{W}{W}")
        .Type("Instant")
        .Text(
          "{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Exile target attacking or blocking creature.")
        .FlavorText("\"Even your shadow is too foul to tolerate.\"")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {
            p.Text = "Exile target attacking or blocking creature.";

            p.Effect = () => new ExileTargets();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && (c.IsAttacker || c.IsBlocker)).On.Battlefield(),
              trg => { trg.Message = "Select target attacking or blocking creature."; });

            p.TimingRule(new Any(new AfterYouDeclareBlockers(), new AfterOpponentDeclaresBlockers()));
            p.TargetingRule(new EffectExileBattlefield());
          });
    }
  }
}
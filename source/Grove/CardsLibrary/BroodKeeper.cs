namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class BroodKeeper : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Brood Keeper")
          .ManaCost("{3}{R}")
          .Type("Creature — Human Shaman")
          .Text("Whenever an Aura becomes attached to Brood Keeper, put a 2/2 red Dragon creature token with flying onto the battlefield. It has \"{R}: This creature gets +1/+0 until end of turn.\"")
          .FlavorText("\"Come, little one. Unfurl your wings, fill your lungs, and release your first fiery breath.\"")
          .Power(2)
          .Toughness(3)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnAttachmentAttached(c => c.Is().Aura));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Dragon")
                .Type("Token Creature - Dragon")
                .Text("{Flying}{EOL}{R}: This creature gets +1/+0 until end of turn.")
                .Power(2)
                .Toughness(2)
                .Colors(CardColor.Red)
                .SimpleAbilities(Static.Flying)
                .ActivatedAbility(ap =>
                {
                  ap.Text = "{R}: This creature gets +1/+0 until end of turn.";
                  ap.Cost = new PayMana(Mana.Red, supportsRepetitions: true);
                  ap.Effect = () => new ApplyModifiersToSelf(
                    () => new AddPowerAndToughness(1, 0){UntilEot = true}).SetTags(EffectTag.IncreasePower);
                  ap.TimingRule(new Any(new PumpOwningCardTimingRule(1, 0), new OnEndOfOpponentsTurn()));
                  ap.RepetitionRule(new RepeatMaxTimes());
                }));
          });
    }
  }
}

namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Outmaneuver : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Outmaneuver")
        .ManaCost("{R}").HasXInCost()
        .Type("Instant")
        .Text("X target blocked creatures assign their combat damage this turn as though they weren't blocked.")
        .FlavorText("'Push one goblin into sight, an' run a lot. That's tactics.'{EOL}—Jula, goblin raider")
        .Cast(p =>
          {
            p.Text = "X target blocked creatures assign their combat damage this turn as though they weren't blocked.";
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.AssignsDamageAsThoughItWasntBlocked) {UntilEot = true});

            p.TargetSelector.AddEffect(trg =>
            {
              trg.MinCount = Value.PlusX;
              trg.MaxCount = Value.PlusX;
              trg.Is.Card(c => c.HasBlockers).On.Battlefield();
            });


          });
    }
  }
}
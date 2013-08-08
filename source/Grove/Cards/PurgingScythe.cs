namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class PurgingScythe : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Purging Scythe")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, Purging Scythe deals 2 damage to the creature with the least toughness. If two or more creatures are tied for least toughness, you choose one of them.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, Purging Scythe deals 2 damage to the creature with the least toughness. If two or more creatures are tied for least toughness, you choose one of them.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              activeTurn: true
              ));

            p.Effect = () => new DealDamageToCreatureWithAttributeSelectIfMoreThanOne(
              amount: 2,
              getAttribute: g =>
                {
                  var creatures = g.Players.Permanents()
                    .Where(x => x.Is().Creature)
                    .OrderBy(x => x.Toughness)
                    .ToList();

                  return creatures.Count == 0 ? null : creatures[0].Toughness;
                },
              hasAttribute: (c, r) => c.Toughness == r);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
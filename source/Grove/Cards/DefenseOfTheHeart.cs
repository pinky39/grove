namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class DefenseOfTheHeart : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Defense of the Heart")
        .ManaCost("{3}{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, if an opponent controls three or more creatures, sacrifice Defense of the Heart, search your library for up to two creature cards, and put those cards onto the battlefield. Then shuffle your library.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if an opponent controls three or more creatures, sacrifice Defense of the Heart, search your library for up to two creature cards, and put those cards onto the battlefield. Then shuffle your library.";

            p.Trigger(new OnStepStart(step: Step.Upkeep)
              {
                Condition = (t, g) => t.OwningCard.Controller.Opponent.Battlefield.Count(c => c.Is().Creature) >= 3
              });

            p.Effect = () => new CompoundEffect(
              new SacrificeOwner(),
              new SearchLibraryPutToZone(
                c => c.PutToBattlefield(),
                minCount: 0,
                maxCount: 2,
                validator: (e, c) => c.Is().Creature,
                text: "Search your library for up to two creatures."));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Infrastructure;
  using Triggers;

  public class CovetousDragon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Covetous Dragon")
        .ManaCost("{4}{R}")
        .Type("Creature Dragon")
        .Text("{Flying}{EOL}When you control no artifacts, sacrifice Covetous Dragon.")
        .FlavorText("Gatha survived as long as he did by giving all Keld's predators exactly what they wanted.")
        .Power(6)
        .Toughness(5)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When you control no artifacts, sacrifice Covetous Dragon.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) => ability.OwningCard.Controller.Opponent
                .Battlefield.None(x => x.Is().Artifact)));

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
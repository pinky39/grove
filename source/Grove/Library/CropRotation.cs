namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class CropRotation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crop Rotation")
        .ManaCost("{G}")
        .Type("Instant")
        .Text(
          "As an additional cost to cast Crop Rotation, sacrifice a land.{EOL}Search your library for a land card and put that card onto the battlefield. Then shuffle your library.")
        .FlavorText("Hmm . . . maybe lotuses this year.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana(Mana.Green, ManaUsage.Spells),
              new Sacrifice());

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().Land,
              text: "Search your library for a land card.");

            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Card(c => c.Is().Land, ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select a land to sacrifice.";
              });

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new CostSacrificeLandToSearchLand());
          });
    }
  }
}
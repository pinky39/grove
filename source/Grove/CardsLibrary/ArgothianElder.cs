namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class ArgothianElder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Argothian Elder")
        .ManaCost("{3}{G}")
        .Type("Creature Elf Druid")
        .Text("{T}: Untap two target lands.")
        .FlavorText("Sharpen your ears")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Untap two target lands.";
            p.Cost = new Tap();
            p.Effect = () => new UntapTargetPermanents();
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(card => card.Is().Land).On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectUntapLand());
          });
    }
  }
}
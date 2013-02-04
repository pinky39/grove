namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;

  public class ArgothianElder : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Elder")
        .ManaCost("{3}{G}")
        .Type("Creature Elf Druid")
        .Text("{T}: Untap two target lands.")
        .FlavorText("Sharpen your ears{EOL}—Elvish expression meaning 'grow wiser'")
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
            p.TimingRule(new SecondMain());
            p.TargetingRule(new UntapLands());
          });
    }
  }
}
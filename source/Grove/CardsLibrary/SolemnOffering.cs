namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class SolemnOffering : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Solemn Offering")
        .ManaCost("{2}{W}")
        .Type("Sorcery")
        .Text("Destroy target artifact or enchantment.{EOL}You gain 4 life.")
        .FlavorText(
          "\"You will be reimbursed for your donation.\"{EOL}\"The reimbursement is spiritual.\"{EOL}—Temple signs")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DestroyTargetPermanents(),
              new ChangeLife(amount: 4, forYou: true));

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact || card.Is().Enchantment)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}
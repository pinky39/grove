namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class SetAdrift : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Set Adrift")
        .ManaCost("{5}{U}")
        .Type("Sorcery")
        .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}Put target nonland permanent on top of its owner's library.")
        .FlavorText("The envoy spoke, and Sidisi replied.")
        .SimpleAbilities(Static.Delve)
        .Cast(p =>
        {
          p.Text = "Put target nonland permanent on top of its owner's library.";
          p.Effect = () => new PutTargetsOnTopOfLibrary();
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => !c.Is().Land).On.Battlefield());

          p.TargetingRule(new EffectPutOnTopOfLibrary());
        });
    }
  }
}

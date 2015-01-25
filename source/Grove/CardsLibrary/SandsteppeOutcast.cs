namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SandsteppeOutcast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sandsteppe Outcast")
        .ManaCost("{2}{W}")
        .Type("Creature — Human Warrior")
        .Text("When Sandsteppe Outcast enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Sandsteppe Outcast.{EOL}• Put a 1/1 white Spirit creature token with flying onto the battlefield.")
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "When Sandsteppe Outcast enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Sandsteppe Outcast.{EOL}• Put a 1/1 white Spirit creature token with flying onto the battlefield.";
          p.Trigger(new OnZoneChanged(to:Zone.Battlefield));
          p.Effect = () => new ChooseOneEffectFromGiven(
            message: "Press 'Yes' to put a +1/+1 counter on Sandsteppe Outcast. Press 'No' to put a creature token.",
            ifTrueEffect: new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1)),
            ifFalseEffect: new CreateTokens(
              count: 1,
              token: Card
                .Named("Spirit")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Spirit")
                .Text("{Flying}")
                .Colors(CardColor.White)
                .SimpleAbilities(Static.Flying)), 
            chooseAi: e =>
            {
              // TODO: Add tweaks for choosing first effect
              return false; // AI prefers to put token instead adding a counter. Always.
            });
        });
    }
  }
}

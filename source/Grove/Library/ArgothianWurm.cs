namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class ArgothianWurm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Argothian Wurm")
        .ManaCost("{3}{G}")
        .Type("Creature - Wurm")
        .Text(
          "{Trample}{EOL}When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Trample)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PutOnTopOfLibraryUnlessOpponentSacsLand();
          }
        );
    }
  }
}
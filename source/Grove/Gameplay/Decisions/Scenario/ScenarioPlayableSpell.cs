namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using System.Linq;
  using System.Runtime.Serialization;
  using Results;

  [Serializable]
  public class ScenarioPlayableSpell : PlayableSpell
  {
    public ScenarioPlayableSpell() {}

    protected ScenarioPlayableSpell(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public override bool CanPlay()
    {
      if (Card == null)
        throw new InvalidOperationException("Did you forget to add card to players hand?");

      var manaCost = Card.GetSpellManaCost(Index);

      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

      Controller.AddManaToManaPool(manaCost);
      var prerequisites = Card.CanCast();

      return prerequisites.Any(x => x.Index == Index);
    }
  }
}
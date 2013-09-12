namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using System.Linq;
  using System.Runtime.Serialization;
  using Results;

  [Serializable]
  public class ScenarioPlayableAbility : PlayableAbility
  {
    public ScenarioPlayableAbility() {}

    protected ScenarioPlayableAbility(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public override bool CanPlay()
    {
      var manaCost = Card.GetActivatedAbilityManaCost(Index);

      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

      Card.Controller.AddManaToManaPool(manaCost);

      var prerequisites = Card.CanActivateAbilities().Where(x => x.CanPay.Value);
      return prerequisites.Any(x => x.Index == Index);
    }
  }
}
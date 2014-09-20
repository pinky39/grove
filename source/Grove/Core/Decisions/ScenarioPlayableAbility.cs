namespace Grove.Decisions
{
  using System;
  using System.Runtime.Serialization;

  [Serializable]
  public class ScenarioPlayableAbility : PlayableAbility
  {
    public ScenarioPlayableAbility() {}

    protected ScenarioPlayableAbility(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public override void Play()
    {
      // so we don't have to have it available
      // to shorten scenario definition
      AddActivationCostToManaPool();

      base.Play();
    }

    private void AddActivationCostToManaPool()
    {
      var manaCost = Card.GetActivatedAbilityManaCost(Index);

      if (ActivationParameters.X.HasValue)
      {
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());
      }

      Card.Controller.AddManaToManaPool(manaCost);
    }
  }
}
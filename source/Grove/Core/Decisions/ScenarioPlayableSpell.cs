namespace Grove.Decisions
{
  using System;
  using System.Runtime.Serialization;
  using Grove.Infrastructure;

  [Serializable]
  public class ScenarioPlayableSpell : PlayableSpell
  {
    public ScenarioPlayableSpell() {}

    protected ScenarioPlayableSpell(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public override void Play()
    {
      Asrt.True(Card != null,
        "Did you forget to add card to players hand?");

      // so we don't have to have it available
      // to shorten scenario definition
      AddCastingCostToManaPool();

      base.Play();
    }

    private void AddCastingCostToManaPool()
    {
      var manaCost = Card.GetSpellManaCost(Index);
      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

      Controller.AddManaToManaPool(manaCost);
    }
  }
}
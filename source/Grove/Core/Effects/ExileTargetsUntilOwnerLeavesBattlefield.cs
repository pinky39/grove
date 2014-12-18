namespace Grove.Effects
{
  using AI;
  using Modifiers;
  using Triggers;

  public class ExileTargetsUntilOwnerLeavesBattlefield : Effect
  {
    public ExileTargetsUntilOwnerLeavesBattlefield()
    {
      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      foreach (Card target in ValidEffectTargets)
      {
        
        // exile first, otherwise modifier 
        // will be removed when moving to exile
        target.Exile();
        
        if (!target.Is().Token)
        {
          var tp = new TriggeredAbility.Parameters
            {
              Text = string.Format("When {0} leaves play, return exiled creature to battlefield.",
                Source.OwningCard.Name),
              Effect = () => new PutOwnerToBattlefield(Zone.Exile),
            };

          tp.Trigger(new WhenPermanentLeavesPlay(Source.OwningCard));

          var mp = new ModifierParameters
            {
              SourceCard = Source.OwningCard,
              SourceEffect = this
            };

          target.AddModifier(new AddTriggeredAbility(new TriggeredAbility(tp)), mp);
        }        
      }
    }
  }
}
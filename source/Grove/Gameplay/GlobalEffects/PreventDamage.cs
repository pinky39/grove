namespace Mtg.Core.GlobalEffects
{
  using Queries;

  public class PreventDamage : GlobalEffect, IAnswersQuery<IsDamagePrevented, bool>
  {
    public bool DoNotPreventDamageToSelf { get; set; }
        
    public bool GetResult(IsDamagePrevented query)
    {
      if (EffectController == query.DamageReceiver.Controller)
      {
        
        if ((DoNotPreventDamageToSelf) && (query.DamageReceiver == EffectSource))
        {
          return false;
        }

        return true;
      }

      return false;
    }
  }
}
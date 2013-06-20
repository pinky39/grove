namespace Grove.Gameplay.DamageHandling
{
  using System;
  using Counters;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  public class ReplaceDamageToPlayersCreaturesWithCounters : DamagePrevention
  {
    private readonly Player _player;
    private readonly Func<Counter> _counter;
    private readonly Func<Card, bool> _filter;

    private ReplaceDamageToPlayersCreaturesWithCounters() {}

    public ReplaceDamageToPlayersCreaturesWithCounters(Player player, Func<Counter> counter, Func<Card, bool> filter = null)
    {
      _player = player;
      _counter = counter;
      _filter = filter ?? delegate { return true; };
    }

    public override int CalculateHash(HashCalculator calc)
    {
       return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_player));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Target.IsPlayer())
        return 0;

      var targetCard = parameters.Target.Card();
            
      if (targetCard.Controller != _player)
        return 0;

      if (_filter(targetCard) == false)
        return 0;
      
      if (parameters.QueryOnly)
        return parameters.Amount;      

      var mp = new ModifierParameters
        {
          SourceCard = targetCard,          
        };

      var modifier = new AddCounters(_counter, parameters.Amount);      
      targetCard.AddModifier(modifier, mp);

      return parameters.Amount;
    }
  }
}
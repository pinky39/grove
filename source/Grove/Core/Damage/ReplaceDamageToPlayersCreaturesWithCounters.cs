namespace Grove
{
  using System;
  using Grove.Infrastructure;
  using Modifiers;

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

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target.IsPlayer())
        return 0;

      var targetCard = p.Target.Card();
            
      if (targetCard.Controller != _player)
        return 0;

      if (_filter(targetCard) == false)
        return 0;
      
      if (p.QueryOnly)
        return p.Amount;      

      var mp = new ModifierParameters
        {
          SourceCard = targetCard,          
        };

      var modifier = new AddCounters(_counter, p.Amount);      
      targetCard.AddModifier(modifier, mp);

      return p.Amount;
    }
  }
}
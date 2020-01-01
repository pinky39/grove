namespace Grove
{
  using System;

  public class PreventDamageToPermanentsOrPlayers : DamagePrevention
  {
    private readonly Func<Player, Context, bool> _playerSelector;
    private readonly Func<Card, Context, bool> _permanentSelector;
    private readonly Func<object, Context, int> _amount;    
    private readonly Func<Card, Context, bool> _sourceSelector;

    private PreventDamageToPermanentsOrPlayers() {}

    public PreventDamageToPermanentsOrPlayers(
      Func<Player, Context, bool> playerSelector = null,
      Func<Card, Context, bool> _permanentSelector = null,
      Func<object, Context, int> amount = null, 
      Func<Card, Context, bool> sourceSelector = null)
    {
      _playerSelector = playerSelector ?? delegate { return false; };
      this._permanentSelector = _permanentSelector ?? delegate { return false; };
      _amount = amount ?? delegate { return int.MaxValue; };
      _sourceSelector = sourceSelector ?? delegate { return true; };      
    }    

    public override int PreventDamage(PreventDamageParameters p)
    {
      var ctx = Ctx(p);

      var player = p.Target as Player;
      var peramenentSelector = p.Target as Card;
      
      if (player != null && !_playerSelector(player, ctx))
        return 0;

      if (peramenentSelector != null && !_permanentSelector(peramenentSelector, ctx))
        return 0;

      if (!_sourceSelector(p.Source, ctx))
        return 0;

      return _amount(p.Target, ctx);                  
    }
  }
}
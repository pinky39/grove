namespace Grove.Effects
{
  public class DrawCardsCreateTokens : Effect
  {
    private readonly DynParam<int> _count;    
    private readonly DynParam<Player> _player;
    private readonly CreateTokens _effect;
    private readonly DynParam<bool> _createTokenIf;

    private DrawCardsCreateTokens() {}

    public DrawCardsCreateTokens(DynParam<int> count, CreateTokens effect,
      DynParam<bool> createTokenIf = null, DynParam<Player> player = null)
    {
      _count = count;
      _player = player;
      _createTokenIf = createTokenIf;
      _effect = effect;

      RegisterDynamicParameters(count, createTokenIf, player);
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      _effect.Initialize(p, game, evaluateParameters);

      return this;
    }

    protected override void ResolveEffect()
    {
      Player player = _player ?? Controller;

      player.DrawCards(_count.Value);
    
      if(_createTokenIf)
        _effect.BeginResolve();
    }
  }
}

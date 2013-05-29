namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using Misc;

  public class CreateTokens : Effect
  {
    private readonly Action<Card, Game> _afterTokenComesToPlay;
    private readonly DynParam<int> _count;
    private readonly Action<Effect, CardFactory> _setTokenParameters = delegate { };
    private readonly DynParam<Player> _tokenController;
    private readonly List<CardFactory> _tokenFactories = new List<CardFactory>();

    private CreateTokens() {}

    public CreateTokens(params CardFactory[] tokens)
    {
      _tokenFactories.AddRange(tokens);
      _afterTokenComesToPlay = delegate { };
      _count = 1;
    }

    public CreateTokens(
      DynParam<int> count,
      CardFactory token,
      Action<Card, Game> afterTokenComesToPlay = null,
      DynParam<Player> tokenController = null,
      Action<Effect, CardFactory> tokenParameters = null)
    {
      _afterTokenComesToPlay = afterTokenComesToPlay ?? delegate { };
      _tokenController = tokenController;
      _setTokenParameters = tokenParameters ?? delegate { };

      _count = count;
      _tokenFactories.Add(token);

      RegisterDynamicParameters(count, tokenController);
    }

    protected override void ResolveEffect()
    {
      var controller = _tokenController ?? Controller;

      for (var i = 0; i < _count.Value; i++)
      {
        foreach (var tokenFactory in _tokenFactories)
        {
          _setTokenParameters(this, tokenFactory);

          var token = tokenFactory.CreateCard();
          token.Initialize(controller.Value, Game);
          token.PutToBattlefield();

          _afterTokenComesToPlay(token, Game);
        }
      }
    }
  }
}
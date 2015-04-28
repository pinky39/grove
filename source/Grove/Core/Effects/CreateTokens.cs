namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;

  public class CreateTokens : Effect
  {
    private readonly EffectAction<Card> _afterTokenComesToPlay;
    private readonly DynParam<int> _count;
    private readonly EffectAction<CardTemplate> _setTokenParameters = delegate { };
    private readonly DynParam<Player> _tokenController;
    private readonly List<CardTemplate> _tokenFactories = new List<CardTemplate>();

    private CreateTokens() {}

    public CreateTokens(params CardTemplate[] tokens)
    {
      _tokenFactories.AddRange(tokens);
      _afterTokenComesToPlay = delegate { };
      _count = 1;
    }

    public CreateTokens(
      DynParam<int> count,
      CardTemplate token,
      EffectAction<Card> afterTokenComesToPlay = null,
      DynParam<Player> tokenController = null,
      EffectAction<CardTemplate> tokenParameters = null)
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
          _setTokenParameters(tokenFactory, Ctx);

          var token = tokenFactory.CreateCard();
          token.Initialize(controller.Value, Game);
          token.PutToBattlefield();

          _afterTokenComesToPlay(token, Ctx);
        }
      }
    }
  }
}
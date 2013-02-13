namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class CreateTokens : Effect
  {
    private readonly Action<Card, Game> _afterTokenComesToPlay;
    private readonly Value _count;
    private readonly Func<Effect, Player> _tokenController;
    private readonly List<CardFactory> _tokenFactories = new List<CardFactory>();

    private CreateTokens() {}

    public CreateTokens(Value count = null, Action<Card, Game> afterTokenComesToPlay = null,
      Func<Effect, Player> tokenController = null, params CardFactory[] tokens)
    {
      _afterTokenComesToPlay = afterTokenComesToPlay ?? delegate { };
      _tokenController = tokenController ?? (e => e.Controller);
      _count = count ?? 1;
      _tokenFactories.AddRange(tokens);
    }

    protected override void ResolveEffect()
    {
      var controller = _tokenController(this);

      for (var i = 0; i < _count.GetValue(X); i++)
      {
        foreach (var tokenFactory in _tokenFactories)
        {
          var token = tokenFactory.CreateCard();
          token.Initialize(controller, Game);
          token.PutToBattlefield();

          _afterTokenComesToPlay(token, Game);
        }
      }
    }
  }
}
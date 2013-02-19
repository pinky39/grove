namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class CreateTokens : Effect
  {
    private readonly Action<Card, Game> _afterTokenComesToPlay;
    private readonly Func<Effect, int> _count;
    private readonly Func<Effect, Player> _tokenController;
    private readonly List<CardFactory> _tokenFactories = new List<CardFactory>();

    private CreateTokens() {}

    public CreateTokens(params CardFactory[] tokens)
    {
      _tokenFactories.AddRange(tokens);
      _afterTokenComesToPlay = delegate { };
      _tokenController = e => e.Controller;
      _count = e => 1;
    }

    public CreateTokens(Value count, CardFactory token, Action<Card, Game> afterTokenComesToPlay = null,
      Func<Effect, Player> tokenController = null) : this(
        e => count.GetValue(e.X), token, afterTokenComesToPlay, tokenController) {}

    public CreateTokens(Func<Effect, int> count, CardFactory token, Action<Card, Game> afterTokenComesToPlay = null,
      Func<Effect, Player> tokenController = null)
    {
      _afterTokenComesToPlay = afterTokenComesToPlay ?? delegate { };
      _tokenController = tokenController ?? (e => e.Controller);
      _count = count;
      _tokenFactories.Add(token);
    }

    protected override void ResolveEffect()
    {
      var controller = _tokenController(this);

      for (var i = 0; i < _count(this); i++)
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
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

    public CreateTokens(Value count, Action<Card, Game> afterTokenComesToPlay = null,
      Func<Effect, Player> tokenController = null, params CardFactory[] tokens) : this(
        e => count.GetValue(e.X), afterTokenComesToPlay, tokenController, tokens) {}

    public CreateTokens(Func<Effect, int> count, Action<Card, Game> afterTokenComesToPlay = null,
      Func<Effect, Player> tokenController = null, params CardFactory[] tokens)
    {
      _afterTokenComesToPlay = afterTokenComesToPlay ?? delegate { };
      _tokenController = tokenController ?? (e => e.Controller);
      _count = count;
      _tokenFactories.AddRange(tokens);
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
namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class CreateTokens : Effect
  {
    private readonly List<ICardFactory> _tokenFactories = new List<ICardFactory>();

    public Value Count = 1;
    public Action<Card, Game> AfterTokenComesToPlay = delegate { };

    protected override void ResolveEffect()
    {
      for (int i = 0; i < Count.GetValue(X); i++)
      {
        foreach (var tokenFactory in _tokenFactories)
        {
          var token = tokenFactory.CreateCard(Controller);
          Controller.PutCardIntoPlay(token);
          AfterTokenComesToPlay(token, Game);
        }
      }
    }

    public void Tokens(params ICardFactory[] tokenFactories)
    {
      _tokenFactories.AddRange(tokenFactories);
    }
  }
}
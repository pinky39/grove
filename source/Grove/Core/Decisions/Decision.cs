namespace Grove.Decisions
{
  using System;
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Decision
  {
    private readonly Func<IDecisionHandler> _machine;
    private readonly Func<IDecisionHandler> _playback;
    private readonly Func<IDecisionHandler> _scenario;
    private readonly Func<IDecisionHandler> _ui;

    protected Decision()
    {
      /* copyable */
    }

    protected Decision(Player controller, Func<IDecisionHandler> ui, Func<IDecisionHandler> machine,
      Func<IDecisionHandler> scenario, Func<IDecisionHandler> playback)
    {
      _ui = ui;
      _machine = machine;
      _scenario = scenario;
      _playback = playback;

      Controller = controller;
    }

    public Player Controller { get; private set; }

    public IDecisionHandler CreateHandler(Game game)
    {
      var handler = GetHandler(game);
      return handler.Initialize(this, game);
    }    

    private IDecisionHandler GetHandler(Game game)
    {
      if (game.Ai.IsSearchInProgress)
        return _machine();

      if (game.Recorder.IsPlayback)
      {
        return _playback();
      }

      switch (Controller.Type)
      {
        case (PlayerType.Human):
          return _ui();
        case (PlayerType.Scenario):
          return _scenario();
        default:
          return _machine();
      }
    }
  }
}
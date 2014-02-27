namespace Mtg.Core.Cards.Modifiers
{
  using GlobalEffects;

  public class AddGlobalEffect : Modifier
  {
    public GlobalEffect Effect { get; set; }

    public override IAction Apply()
    {
      return new SimpleAction{
        DoExecute = game => {
          Effect.EffectSource = Source;
          game.AddGlobalEffect(Effect);
        }
      };
    }

    public override IAction Remove()
    {
      return new SimpleAction{
        DoExecute = game => game.RemoveGlobalEffect(Effect)
      };
    }
  }
}
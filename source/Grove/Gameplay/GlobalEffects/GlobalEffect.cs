namespace Mtg.Core.GlobalEffects
{
  using Cards;
  using Infrastructure;

  public abstract class GlobalEffect : ICopyable
  {
    public Player EffectController
    {
      get { return EffectSource.Controller; }
    }

    public Card EffectSource { get; set; }


    public void Copy(object original, ICopyService copyService)
    {
      var org = (GlobalEffect) original;
      EffectSource = copyService.Copy(org.EffectSource);
    }

    public virtual void Copy(GlobalEffect original, ICopyService copyService)
    {
      
    }
  }
}
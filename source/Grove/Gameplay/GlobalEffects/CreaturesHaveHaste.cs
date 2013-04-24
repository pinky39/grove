namespace Mtg.Core.GlobalEffects
{
  public class CreaturesHaveHaste : GlobalEffect, IAnswersQuery<HasHaste, bool>
  {
    public bool GetResult(HasHaste query)
    {
      if (EffectController == query.Card.Controller)
      {
        return true;
      }

      return false;
    }
  }
}
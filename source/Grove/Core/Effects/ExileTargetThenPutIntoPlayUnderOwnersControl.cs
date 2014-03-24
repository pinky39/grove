namespace Grove.Effects
{
  public class ExileTargetThenPutIntoPlayUnderOwnersControl : Effect
  {
    protected override void ResolveEffect()
    {
      Target.Card().ExileFrom(Zone.Battlefield);
      Target.Card().PutToBattlefieldFrom(Zone.Exile);
    }
  }
}
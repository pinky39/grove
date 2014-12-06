namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class TargetPlayerSacrificesPermanents : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly DynParam<int> _count;
    private readonly Func<Card, bool> _filter;
    private readonly string _text;

    private TargetPlayerSacrificesPermanents() {}

    public TargetPlayerSacrificesPermanents(DynParam<int> count, Func<Card, bool> filter, string text)
    {
      _count = count;
      _filter = filter;
      _text = text;

      RegisterDynamicParameters(_count);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => x.Score)
        .Take(_count)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }
    }

    protected override void ResolveEffect()
    {
      if (Target == null || !Target.IsPlayer())
        return;

      Enqueue(new SelectCards(
        Target.Player(),
        p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.SetValidator(_filter);
            p.Zone = Zone.Battlefield;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }));
    }
  }
}

namespace Grove.Core.Effects
{
  using System;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;
  using Grove.Core.Messages;
  using Grove.Core.Zones;

  public class SearchLibraryPutToHand : Effect, IProcessDecisionResults<ChosenCards>
  {
    public int MaxCount;
    public int MinCount;
    public bool DiscardRandomCardAfterwards;
    public string Text;
    public bool RevealCards = true;
    public Func<Card, bool> Validator = delegate { return true; };

    public void ProcessResults(ChosenCards results)
    {                       
      if (RevealCards)
      {
        foreach (var card in results)
        {
          Game.Publish(new CardWasRevealed {Card = card});  
        }                
      }
      else
      {
        foreach (var card in results)
        {
          card.ResetVisibility();
        }
      }
            
      if (DiscardRandomCardAfterwards)
      {
        Controller.DiscardRandomCard();
      }
            
      Controller.ShuffleLibrary();
    }

    protected override void ResolveEffect()
    {
      Controller.RevealLibrary();
      
      Game.Enqueue<SelectCardsPutToHand>(
        controller: Controller,
        init: p =>
          {
            p.MinCount = MinCount;
            p.MaxCount = MaxCount;
            p.Validator = Validator;
            p.Zone = Zone.Library;
            p.Text = FormatText(Text);
            p.AiOrdersByDescendingScore = true;
            p.ProcessDecisionResults = this;
          });
    }
  }
}
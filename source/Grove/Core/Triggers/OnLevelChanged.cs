namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnLevelChanged : Trigger, IReceive<CardChangedLevel>
  {
    public int Level { get; set; }    
    
    public void Receive(CardChangedLevel message)
    {
      if (message.Card == Ability.OwningCard)
      {
        if (message.Card.Level == Level)
        {
          Set();  
        }                
      }
    }
  }
}
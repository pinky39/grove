namespace Grove.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decisions;
    using Infrastructure;

    public class SacrificeCreatureOrPayLifeOrOpponentDrawsCard : CustomizableEffect,
        IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
    {
        private const string Sacrifice = "Sacrifice a creature";
        private const string DrawCard = "Opponent draws a card";
        
        private readonly int _lifeAmount;

        private SacrificeCreatureOrPayLifeOrOpponentDrawsCard() { }

        public SacrificeCreatureOrPayLifeOrOpponentDrawsCard(int lifeAmount)
        {
            _lifeAmount = lifeAmount;
        }

        protected override void ResolveEffect()
        {
            Enqueue(new ChooseEffectOptions(Controller.Opponent, p =>
            {
                p.ProcessDecisionResults = this;
                p.ChooseDecisionResults = this;
                p.Text = GetText();
                p.Choices = GetChoices().ToList();
            }));
        }

        public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
        {
            if (Controller.Opponent.Life > 15 && Controller.Battlefield.Count < 6)
            {
                return new ChosenOptions(
                    "Pay " + _lifeAmount + " life");
            }

            if (Controller.Opponent.Battlefield.Creatures.Any(c => c.Attachments.Any(a => a.Controller == Controller)))
            {
                return new ChosenOptions(
                    Sacrifice);
            }

            return new ChosenOptions(
                    DrawCard);
        }

        public override void ProcessResults(ChosenOptions results)
        {
            var action = (string)results.Options[0];

            if (action.Equals(Sacrifice, StringComparison.OrdinalIgnoreCase))
            {
                Enqueue(new SelectCards(
                  Controller.Opponent,
                  p =>
                  {
                      p.SetValidator(card => card.Is().Creature);
                      p.Zone = Zone.Battlefield;
                      p.Text = "Select a creature to sacrifice and press Enter.";
                      p.OwningCard = Source.OwningCard;
                      p.ProcessDecisionResults = this;
                      p.ChooseDecisionResults = this;
                      p.MinCount = 1;
                      p.MaxCount = 1;
                  }));
                return;
            }

            if (action.Equals(DrawCard, StringComparison.OrdinalIgnoreCase))
            {
                Controller.DrawCard();
                return;
            }

            Controller.Opponent.Life -= _lifeAmount;
        }

        public ChosenCards ChooseResult(List<Card> candidates)
        {
            if (Controller.Opponent.Battlefield.Creatures.Any(c => c.Attachments.Any(a => a.Controller == Controller)))
            {
                return candidates
                  .OrderBy(x => x.Score)
                  .Take(1)
                  .ToList();
            }

            return new ChosenCards();
        }

        public void ProcessResults(ChosenCards results)
        {
            if (results.None())
            {
                Controller.DrawCard();

                return;
            }

            results[0].Sacrifice();
        }

        public override string GetText()
        {
            return "Select an effect: #0.";
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            yield return new DiscreteEffectChoice(
                "Pay " + _lifeAmount + " life",
                Sacrifice,
                DrawCard);            
        }
    }
}

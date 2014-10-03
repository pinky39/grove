namespace Grove.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using Decisions;
    using Infrastructure;

    public class ChangeZoneForSelectedCard : CustomizableEffect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
    {
//        private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
        private readonly string _text;
        private readonly Func<Card, bool> _validator;
        private readonly Zone _zoneTo;
        private Zone _zoneFrom;

        private readonly int _maxCount;
        private readonly int _minCount;
        private readonly DynParam<Player> _player;

        private readonly bool _aurasNeedTarget;
        private readonly Action<Card, Effect> _afterPutToZone;

        private ChangeZoneForSelectedCard() {}

        // TODO: Add support for modifiers

        public ChangeZoneForSelectedCard(string text, Func<Card, bool> validator, Zone zoneTo = Zone.Battlefield, int maxCount = 1, int minCount = 0, bool? aurasNeedTarget = null,
            Action<Card, Effect> afterPutToZone = null, DynParam<Player> player = null) //, params CardModifierFactory[] modifiers
        {
            _text = text ?? "Search your #0 for a card.";


            _zoneTo = zoneTo;
            _validator = validator;

            _maxCount = maxCount;
            _minCount = minCount;
            _aurasNeedTarget = aurasNeedTarget ?? zoneTo == Zone.Battlefield;

            _afterPutToZone = afterPutToZone ?? delegate { };
//            _modifiers.AddRange(modifiers);

            _player = player ?? new DynParam<Player>((e, g) => e.Controller, EvaluateAt.OnResolve);
            RegisterDynamicParameters(_player);
        }

        public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
        {
            var zone = Zone.Hand;

            if (Controller.Graveyard.Any(_validator))
                zone = Zone.Graveyard;

            if (Controller.Library.Any(_validator))
                zone = Zone.Library;

            return new ChosenOptions(ChoiceToZoneMap.Single(x => x.Zone == zone).Choice);
        }

        public override void ProcessResults(ChosenOptions results)
        {
            _zoneFrom = ChoiceToZoneMap.Single(x => x.Choice.Equals(results.Options[0])).Zone;

            if(_zoneFrom == Zone.Library)
                _player.Value.PeekLibrary();

            Enqueue(new SelectCards(Controller,
            p =>
                {
                p.SetValidator(_validator);
                p.Zone = _zoneFrom;
                p.MinCount = _minCount;
                p.MaxCount = _maxCount;
                p.Text = "Search your " + _zoneFrom.ToString() + " for a card";
                p.OwningCard = Source.OwningCard;
                p.ProcessDecisionResults = this;
                p.ChooseDecisionResults = this;
                p.AurasNeedTarget = _aurasNeedTarget;
                }
            ));
        }

        public ChosenCards ChooseResult(List<Card> candidates)
        {
            return CardPicker.ChooseBestCards(
                controller: _player.Value,
                candidates: candidates,
                count: _maxCount,
                aurasNeedTarget: _aurasNeedTarget);
        }

        public void ProcessResults(ChosenCards results)
        {
            var i = 0;

            while (i < results.Count)
            {
                var card = results[i++];
                Card attachTo = null;

                if (_aurasNeedTarget && card.Is().Aura)
                {
                    attachTo = results[i++];
                }

                PutToZone(card, attachTo);

//                if (_revealCards)
//                {
//                    Publish(new CardWasRevealedEvent(card));
//                }
//                else
                {
                    card.ResetVisibility();
                }
            }

            if(_zoneFrom == Zone.Library)
                Controller.ShuffleLibrary();
        }

        private void PutToZone(Card card, Card attachTo)
        {
            // TODO: Add support for another zones
            switch (_zoneTo)
            {
//                case (Zone.Hand):
//                    {
//                        card.PutToHandFrom(Zone.Library);
//                        break;
//                    }
                case (Zone.Battlefield):
                    {
                        if (attachTo != null)
                        {
                            card.EnchantWithoutPayingCost(attachTo);
                            break;                            
                        }

                        if (card.Is().Aura)
                        {
                            card.EnchantWithoutPayingCost(Source.OwningCard);
                            break;
                        }

                        card.PutToBattlefield();
                        break;                        
                    }
//                case (Zone.Graveyard):
//                    {
//                        card.PutToGraveyard();
//                        break;
//                    }
//                case (Zone.Exile):
//                    {
//                        card.Exile();
//                        break;
//                    }
                default:
                    {
                        Asrt.Fail(String.Format("Zone not supported: {0}.", _zoneTo));
                        break;
                    }
            }

            _afterPutToZone(card, this);
        }

        public override string GetText()
        {
            return _text;
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            yield return new DiscreteEffectChoice(
              EffectOption.Library,
              EffectOption.Graveyard,
              EffectOption.Hand);
        }
    }
}

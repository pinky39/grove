namespace Grove
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Costs;
    using Effects;
    using Events;
    using Grove.AI;
    using Grove.AI.TargetingRules;
    using Grove.Infrastructure;

    public class CastRule : GameObject, IEffectSource
    {
        private readonly Parameters _p;
        private Card _card;

        private CastRule() { }

        public CastRule(Parameters p) { _p = p; }
        public bool HasXInCost { get { return _p.Cost.HasX; } }
        public Card OwningCard { get { return _card; } }
        public Card SourceCard { get { return _card; } }

        public void EffectCountered(SpellCounterReason reason)
        {
            _card.PutToGraveyard();

            Publish(new SpellCounteredEvent(_card, reason));
        }

        public void EffectPushedOnStack()
        {
            _card.ChangeZone(
              destination: Stack,
              add: delegate { });
        }

        public void EffectResolved()
        {
            var putToZone = _p.PutToZoneAfterResolve ?? PutToZoneAfterResolve;
            putToZone(_card);
        }

        public bool IsTargetStillValid(ITarget target, object triggerMessage) { return _p.TargetSelector.IsValidEffectTarget(target, triggerMessage); }

        public bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets)
        {
            return _p.TargetSelector.ValidateTargetDependencies(new ValidateTargetDependenciesParam
            {
                Cost = costTargets,
                Effect = effectTargets
            });
        }

        public int CalculateHash(HashCalculator calc) { return 0; }

        private void PutToZoneAfterResolve(Card card)
        {
            if (card.Is().Sorcery || card.Is().Instant)
            {
                card.PutToGraveyard();
                return;
            }

            if (card.Is().Aura)
            {
                var attachedCardController = card.GetControllerOfACardThisIsAttachedTo();
                attachedCardController.PutCardToBattlefield(card);
                return;
            }

            card.PutToBattlefield();
        }

        public void Initialize(Card card, Game game)
        {
            Game = game;
            _card = card;

            _p.TargetSelector.Initialize(card, game);

            foreach (var aiInstruction in _p.Rules)
            {
                aiInstruction.Initialize(game);
            }

            _p.Cost.Initialize(card, game, _p.TargetSelector.Cost.FirstOrDefault());
        }

        private bool CanCastCardType()
        {
            if (_card.Is().Instant || _card.Has().Flash)
                return true;

            if (!_card.Controller.IsActive || !Turn.Step.IsMain() || !Stack.IsEmpty)
                return false;

            return !_card.Is().Land || _card.Controller.CanPlayLands;
        }

        private bool CanCastWithConvoke(IManaAmount amount)
        {
            var actualCost = Game.GetActualCost(amount, ManaUsage.Spells, _card);

            var cards = _card.Controller.Battlefield.Creatures
                .Where(c => c.CanBeTapped);

            foreach (var card in cards)
            {
                var mana = Mana.ParseCardColors(card.Colors);

                // Try remove colored mana first. If not, the card reduces colorless cost
                var currentCost = actualCost.Remove(mana);
                if (currentCost.Converted == actualCost.Converted)
                    currentCost = currentCost.Remove(new SingleColorManaAmount(ManaColor.Colorless, 1));

                actualCost = currentCost;

                if (actualCost.Converted == 0)
                    break;
            }
            
            return _card.Controller.HasMana(actualCost, ManaUsage.Spells);
        }

        public bool CanCast(out ActivationPrerequisites prerequisites)
        {
            prerequisites = null;

            if (CanCastCardType() == false)
            {
                return false;
            }

            if (_p.Condition(_card.Controller, Game) == false)
            {
                return false;
            }            

            var result = _p.Cost.CanPay();            

            prerequisites = new ActivationPrerequisites
            {
                CanPay = result.CanPay(),
                Description = _p.Text,
                Selector = _p.TargetSelector,
                MaxX = result.MaxX(),
                DistributeAmount = _p.DistributeAmount,
                Card = _card,
                Rules = _p.Rules
            };

            // TODO: Add support of an aggregated cost and 'X' in costs
            if (_card.HasConvoke)
            {
                prerequisites.CanPay = new Lazy<bool>(() => CanCastWithConvoke(_card.ManaCost));
            }
            
            return true;
        }

        public void Cast(ActivationParameters p)
        {
            var parameters = new EffectParameters
            {
                Source = this,
                Targets = p.Targets,
                X = p.X
            };

            var effect = _p.Effect().Initialize(parameters, Game);

            if (p.PayCost)
            {
                if (_card.Controller.IsMachine && _card.HasConvoke)
                {
                    PayConvoke(_card.Controller);
                }

                _p.Cost.Pay(p.Targets, p.X);
            }

            if (p.SkipStack)
            {
                effect.QuickResolve();
                return;
            }

            if (_card.Is().Land)
            {
                _card.Controller.LandsPlayedCount++;

                effect.QuickResolve();
                Publish(new LandPlayedEvent(effect.Source.OwningCard));

                return;
            }

            Publish(new SpellCastEvent(_card, p.Targets));


            Stack.Push(effect);
            Publish(new SpellPutOnStackEvent(_card, p.Targets));
        }

        private void PayConvoke(Player player)
        {
            var candidates = player.Battlefield.Creatures
                .Where(c => c.CanBeTapped)
                .OrderBy(x =>
                {
                    var mana = Mana.ParseCardColors(x.Colors);

                    var manaCost = Game.GetActualCost(_card.ManaCost, ManaUsage.Spells, _card);
                    var result = manaCost.Remove(mana); // Try remove color mana
                    
                    if (result.Converted < manaCost.Converted)
                    {
                        if (!player.HasMana(mana, ManaUsage.Spells))
                            return 1;

                        if (!player.HasMana(result, ManaUsage.Spells))
                            return 2;
                    }
                    
                    return 3;
                })
                .ThenBy(x =>
                {
                    if (x.HasSummoningSickness)
                        return 1;

                    if (!x.CanAttack && !x.CanBlock())
                        return 2;

                    if (!x.CanAttack)
                        return 3;

                    return 4;
                })
                .Take(_card.ManaCost.Converted)
                .ToList();

            var amount = Game.GetActualCost(_card.ManaCost, ManaUsage.Spells, _card);

            foreach (var card in candidates)
            {
                // All selected card can be tapped for convoke even through the mana cost was be reduced to zero,
                // but for AI player it is difficult, so he taps creatures only while he does not have mana
                if (player.HasMana(amount, ManaUsage.Spells))
                    break;

                var mana = Mana.ParseCardColors(card.Colors);

                card.Tap();

                player.AddManaToManaPool(mana, ManaUsage.Spells);
            }

            Asrt.True(player.HasMana(_card.ManaCost, ManaUsage.Spells), "Convoke has worked incorrect.");
        }

        public bool CanTarget(ITarget target) { return _p.TargetSelector.Effect[0].IsTargetValid(target, _card); }

        public bool IsGoodTarget(ITarget target, Player controller)
        {
            return TargetingHelper.IsGoodTargetForSpell(
              target, OwningCard, controller, _p.TargetSelector,
              _p.Rules.Where(r => r is TargetingRule).Cast<TargetingRule>());
        }

        public IManaAmount GetManaCost() { return _p.Cost.GetManaCost(); }
        public override string ToString() { return _card.ToString(); }

        public class Parameters : AbilityParameters
        {
            public Cost Cost;
            public string KickerDescription = "Cast {0} with kicker.";
            public Action<Card> PutToZoneAfterResolve;

            public Func<Player, Game, bool> Condition = delegate { return true; };
        }
    }
}
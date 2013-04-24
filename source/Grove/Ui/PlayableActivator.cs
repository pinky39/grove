﻿namespace Grove.Ui
{
  using System;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Decisions.Results;

  public class PlayableActivator
  {
    public Func<ActivationParameters, Playable> GetPlayable;
    public ActivationPrerequisites Prerequisites;
  }
}
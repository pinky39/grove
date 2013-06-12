namespace Grove.Gameplay.Modifiers
{
  using System;
  using Characteristics;

  public class ChangeController : Modifier, ICardModifier
  {
    private readonly Func<Modifier, Player> _getNewController;
    private readonly Player _newController;
    private ControllerCharacteristic _controller;
    private ControllerSetter _controllerSetter;

    private ChangeController() {}

    public ChangeController(Player newController)
    {
      _newController = newController;
    }

    public ChangeController(Func<Modifier, Player> getNewController)
    {
      _getNewController = getNewController;
    }

    private Player NewController { get { return _newController ?? _getNewController(this); } }

    public override void Apply(ControllerCharacteristic controller)
    {
      _controller = controller;
      _controllerSetter = new ControllerSetter(NewController);
      _controllerSetter.Initialize(ChangeTracker);
      _controller.AddModifier(_controllerSetter);
    }

    protected override void Unapply()
    {
      _controller.RemoveModifier(_controllerSetter);
    }
  }
}
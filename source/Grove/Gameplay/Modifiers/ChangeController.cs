namespace Grove.Gameplay.Modifiers
{
  using System;
  using Characteristics;

  public class ChangeController : Modifier, ICardModifier
  {
    private readonly Func<Modifier, Player> _getNewController;
    private ControllerCharacteristic _controller;
    private ControllerSetter _controllerSetter;
    private Player _newController;

    private ChangeController() {}

    public ChangeController(Player newController)
    {
      _newController = newController;
    }

    public ChangeController(Func<Modifier, Player> getNewController)
    {
      _getNewController = getNewController;
    }

    public override void Apply(ControllerCharacteristic controller)
    {
      _controller = controller;
      _controllerSetter = new ControllerSetter(_newController);
      _controllerSetter.Initialize(ChangeTracker);
      _controller.AddModifier(_controllerSetter);
    }

    protected override void Initialize()
    {
      if (_newController == null)
      {
        _newController = _getNewController(this);
      }
    }

    protected override void Unapply()
    {
      _controller.RemoveModifier(_controllerSetter);
    }
  }
}
namespace Grove.Core.Modifiers
{
  public class ChangeController : Modifier
  {
    private readonly Player _newController;
    private ControllerCharacteristic _controller;
    private ControllerSetter _controllerSetter;

    private ChangeController() {}

    public ChangeController(Player newController)
    {
      _newController = newController;
    }

    public override void Apply(ControllerCharacteristic controller)
    {
      _controller = controller;
      _controllerSetter = new ControllerSetter(_newController, ChangeTracker);
      controller.AddModifier(_controllerSetter);
    }

    protected override void Unapply()
    {
      _controller.RemoveModifier(_controllerSetter);
    }
  }
}
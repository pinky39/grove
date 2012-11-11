namespace Grove.Core.Details.Cards.Modifiers
{
  public class ChangeController : Modifier
  {
    public Player NewController;
    private ControllerCharacteristic _controller;
    private ControllerSetter _controllerSetter;
   
    public override void Apply(ControllerCharacteristic controller)
    {
      _controller = controller;
      _controllerSetter = new ControllerSetter(NewController, ChangeTracker);
      controller.AddModifier(_controllerSetter);
    }

    protected override void Unapply()
    {
      _controller.RemoveModifier(_controllerSetter);
    }
  }
}
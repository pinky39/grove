namespace Grove.Ui
{
  using Core;
  using Shell;

  public abstract class ViewModelBase : GameObject
  {
    public DistributeDamage.ViewModel.IFactory DistributeDamageDialog { get; set; }
    public SelectAbility.ViewModel.IFactory SelectAbilityVmFactory { get; set; }
    public SelectTarget.ViewModel.IFactory SelectTargetVmFactory { get; set; }
    public SelectXCost.ViewModel.IFactory SelectXCostVmFactory { get; set; }
    public new Game Game { get; set; }
    public IShell Shell { get; set; }
  }
}
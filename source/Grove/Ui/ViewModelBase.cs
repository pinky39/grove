namespace Grove.Ui
{
  using DistributeDamage;
  using Gameplay;
  using Gameplay.Common;
  using Shell;

  public abstract class ViewModelBase : GameObject
  {
    public ViewModel.IFactory DistributeDamageDialog { get; set; }
    public SelectAbility.ViewModel.IFactory SelectAbilityDialog { get; set; }
    public SelectTarget.ViewModel.IFactory SelectTargetDialog { get; set; }
    public SelectXCost.ViewModel.IFactory SelectXCostDialog { get; set; }
    public IShell Shell { get; set; }
    public new Game Game { get { return base.Game; } set { base.Game = value; } }
  }
}
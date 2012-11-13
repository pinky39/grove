namespace Grove.Ui.SelectAbility
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Cards;
  using Infrastructure;

  public class ViewModel
  {
    private readonly List<SpellPrerequisites> _prerequisites = new List<SpellPrerequisites>();

    public ViewModel(IEnumerable<SpellPrerequisites> prerequisites)
    {
      _prerequisites.AddRange(prerequisites);
    }

    public IEnumerable<SpellPrerequisites> SatisfyableAbilities { get { return _prerequisites.Where(x => x.CanBeSatisfied); } }

    public bool WasCanceled { get; private set; }
    public SpellPrerequisites Selected { get; private set; }

    public void Select(SpellPrerequisites selected)
    {
      Selected = selected;
      this.Close();
    }

    public void Cancel()
    {
      WasCanceled = true;
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<SpellPrerequisites> prerequisites);
    }
  }
}
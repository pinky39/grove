namespace Grove.Infrastructure
{
  public interface ICopyContributor
  {
    void AfterMemberCopy(object original);
  }
}
namespace Grove.Utils
{
  using System;

  public class TaskRegistration
  {
    public TaskRegistration(string keyword, Action<Arguments> task)
    {
      Keyword = keyword;
      Task = task;
    }

    public string Keyword { get; private set; }
    public Action<Arguments> Task { get; private set; }
  }
}
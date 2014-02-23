namespace Grove.Gameplay
{
  using System;
  using System.Runtime.Serialization;
  using Infrastructure;

  public class RenameBinder : SerializationBinder
  {
    public override Type BindToType(string assemblyName, string typeName)
    {
      var type = Type.GetType(typeName) ?? GetTypeWithRename(typeName);

      Asrt.True(type != null,
        String.Format("This version of save file is not supported because type {0} could not be deserialized.",
          typeName));

      return type;
    }

    private Type GetTypeWithRename(string typeName)
    {
      // handle all renames of serialized classes here 
      // so older saved games will still work

      typeName = typeName.Replace(".Persistance", ".Gameplay");
      typeName = typeName.Replace(".Tournaments", String.Empty);
      typeName = typeName.Replace(".Sets", String.Empty);
      typeName = typeName.Replace(".Results", String.Empty);
      typeName = typeName.Replace(".Abilities", String.Empty);
      typeName = typeName.Replace(".Targeting", String.Empty);
      return Type.GetType(typeName);
    }
  }
}
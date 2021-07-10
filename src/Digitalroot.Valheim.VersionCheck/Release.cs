namespace Digitalroot.Valheim.VersionCheck
{
  /// <summary>
  /// Details of the repo releases
  /// </summary>
  public class Release
  {
    /// <summary>
    /// Name of the release
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Tag name of the release
    /// </summary>
    public readonly string Tag;

    /// <summary>
    /// Is this version disabled
    /// </summary>
    public readonly bool IsDisabled;


    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="name">Name of the release</param>
    /// <param name="tag">Tag name of the release</param>
    /// <param name="isDisabled">Is this version disabled</param>
    public Release(string name, string tag, bool isDisabled)
    {
      Name = name;
      Tag = tag;
      IsDisabled = isDisabled;
    }
  }
}

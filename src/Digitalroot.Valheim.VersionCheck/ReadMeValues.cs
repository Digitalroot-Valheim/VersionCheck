namespace Digitalroot.Valheim.VersionCheck
{
  /// <summary>
  /// Details of the ReadMe file
  /// </summary>
  public class ReadMeValues
  {
    /// <summary>
    /// GUID for single use rate limit. Do not abuse. Rate is 5000 GETs per hour
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Version number returned in the ReadMe file
    /// </summary>
    public readonly string Version;

    /// <summary>
    /// Is this version disabled
    /// </summary>
    public readonly bool IsDisabled;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="name">Name returned in the ReadMe file</param>
    /// <param name="version">Version number returned in the ReadMe file</param>
    /// <param name="isDisabled">Is this version disabled</param>
    public ReadMeValues(string name, string version, bool isDisabled)
    {
      Name = name;
      Version = version;
      IsDisabled = isDisabled;
    }
  }
}

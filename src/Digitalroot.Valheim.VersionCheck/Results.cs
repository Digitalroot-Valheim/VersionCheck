using System;
using System.Collections.Generic;

namespace Digitalroot.Valheim.VersionCheck
{
  /// <summary>
  /// Results collection
  /// </summary>
  public class Results
  {
    /// <summary>
    /// Results collection for Release mode
    /// </summary>
    public readonly List<Release> Releases = new List<Release>();

    /// <summary>
    /// Results collection for ReadMe mode
    /// </summary>
    public readonly List<ReadMeValues> ReadMeValues = new List<ReadMeValues>();

    /// <summary>
    /// Was there an error in the request, see this.Exception to gets the error details.
    /// </summary>
    public bool IsErrored { get; internal set; }

    /// <summary>
    /// Exception details
    /// </summary>
    public Exception Exception { get; internal set; }
  }
}

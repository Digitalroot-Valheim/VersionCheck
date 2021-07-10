using Octokit;
using System;
using System.Collections.Generic;
using System.IO;

namespace Digitalroot.Valheim.VersionCheck
{
  /// <summary>
  /// Check for disabled releases
  /// </summary>
  public class VersionCheckClient : IDisposable
  {
    /// <summary>
    /// Account username or organization name
    /// </summary>
    private readonly string _owner;

    /// <summary>
    /// Name of the repo
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// Releases | ReadMe
    /// ReadMe: Scans for Scans README.md, README
    /// Releases: Scans Releases
    /// </summary>
    private readonly VersionCheckMode _mode;

    /// <summary>
    /// Octokit GitHub Client
    /// </summary>
    private GitHubClient _gitHubClient;

    /// <summary>
    /// ReadMe from repo
    /// </summary>
    private Readme _readme;

    /// <summary>
    /// ReadMe file from repo
    /// </summary>
    public Readme Readme
    {
      get => _readme;
      private set => _readme = value;
    }

    /// <summary>
    /// List of releases from GitHub
    /// </summary>
    private IReadOnlyList<Octokit.Release> _releases;

    /// <summary>
    /// List of releases from Repo
    /// </summary>
    public IReadOnlyList<Octokit.Release> Releases
    {
      get => _releases;
      private set => _releases = value;
    }

    /// <summary>
    /// Results from Execute
    /// </summary>
    public readonly Results Results = new Results();

    /// <summary>
    /// https://github.com/{owner}/{name}
    /// </summary>
    /// <param name="owner">Account username or organization name</param>
    /// <param name="name">Name of the repo</param>
    /// <param name="mode">Scans README.md, README|Releases</param>
    public VersionCheckClient(string owner, string name, VersionCheckMode mode)
    {
      _owner = owner;
      _name = name;
      _mode = mode;
      var guid = Guid.NewGuid().ToString("D"); // GUID for single use rate limit. Do not abuse. Rate is 5000 GETs per hour
      _gitHubClient = new GitHubClient(new ProductHeaderValue(guid));
    }

    /// <summary>
    /// Execute the call to GitHub
    /// </summary>
    /// <returns></returns>
    public Results Execute()
    {
      try
      {
        if (_releases != null) return Results;
        if (_readme != null) return Results;

        switch (_mode)
        {
          case VersionCheckMode.Releases:
            ExecuteReleases();
            break;
          case VersionCheckMode.ReadMe:
            ExecuteReadMe();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      catch (Exception e)
      {
        // There are many reasons this could error. 
        // A real use case is if no connection to the internet is found.
        // It is up to the author to handle.
        Results.IsErrored = true;
        Results.Exception = e;
      }

      return Results;
    }

    /// <summary>
    /// ReadMe flow
    /// </summary>
    private void ExecuteReadMe()
    {
      Readme = _gitHubClient.Repository.Content.GetReadme(_owner, _name).Result;
      if (Readme.Content.Contains("IsEnabled") || Readme.Content.Contains("IsDisabled"))
      {
        using (StringReader stringReader = new StringReader(Readme.Content))
        {
          string line;
          while ((line = stringReader.ReadLine()) != null)
          {
            if (string.IsNullOrEmpty(line)) continue;
            if (!line.Contains("IsEnabled") && !line.Contains("IsDisabled")) continue;
            var split = line.Replace("<!--", string.Empty).Replace("-->", string.Empty).Trim().Split('|');
            if (split.Length == 3)
            {
              Results.ReadMeValues.Add(new ReadMeValues(split[0], split[1], split[2].Equals("IsDisabled")));
            }
          }
        }
      }
    }

    /// <summary>
    /// Release flow
    /// </summary>
    private void ExecuteReleases()
    {
      Releases = _gitHubClient.Repository.Release.GetAll(_owner, _name).Result;
      foreach (Octokit.Release release in Releases)
      {
        Results.Releases.Add(new Release(release.Name, release.TagName, release.Body.Contains("IsDisabled")));
      }
    }

    #region IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
      _gitHubClient = null;
      _readme = null;
      _releases = null;
    }

    #endregion
  }
}

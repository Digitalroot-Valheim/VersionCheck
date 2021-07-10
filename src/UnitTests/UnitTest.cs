using Digitalroot.Valheim.VersionCheck;
using NUnit.Framework;
using System.Linq;

namespace UnitTests
{
  public class Tests
  {
    private string _owner = "DR";
    private string _name = "digitalroot-valheim-mods";

    [Test]
    public void ReleasesTest()
    {
      using (var versionCheckClient = new VersionCheckClient(_owner, _name, VersionCheckMode.Releases))
      {
        Results results = versionCheckClient.Execute();
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Releases, Is.Not.Null);
        Assert.That(results.Releases, Is.Not.Empty);
        Assert.That(results.ReadMeValues, Is.Empty);
        Assert.That(results.Releases.Where(r => r.Tag == "jbtf.v1.0.0" && r.IsDisabled).ToList().Count, Is.EqualTo(1));

        foreach (var result in results.Releases.Where(r => r.Tag != "jbtf.v1.0.0"))
        {
          Assert.That(result.IsDisabled, Is.False);
        }
      }
    }

    [Test]
    public void ReadMeTest()
    {
      using (var versionCheckClient = new VersionCheckClient(_owner, _name, VersionCheckMode.ReadMe))
      {
        Results results = versionCheckClient.Execute();
        Assert.That(results.ReadMeValues, Is.Not.Null);
        Assert.That(results.ReadMeValues, Is.Not.Empty);
        Assert.That(results.Releases, Is.Empty);
        Assert.That(results.ReadMeValues.Where(r => r.IsDisabled).ToList().Count, Is.EqualTo(2));

        foreach (var result in results.ReadMeValues.Where(r => !r.IsDisabled))
        {
          Assert.That(result.IsDisabled, Is.False);
        }
      }
    }

    [Test]
    public void OfflineTest()
    {
      using (var versionCheckClient = new VersionCheckClient(_owner, _name, VersionCheckMode.ReadMe))
      {
        Results results = versionCheckClient.Execute();
        Assert.That(results.ReadMeValues, Is.Empty);
        Assert.That(results.Releases, Is.Empty);
        Assert.That(results.IsErrored, Is.True);
        Assert.That(results.Exception, Is.Not.Null);
      }
    }

    [Test]
    [Explicit]
    public void XplitReleasesExampleTest()
    {
      // GitHub repository url format. https://github.com/githubtraining/hellogitworld
      // GitHub repository url format. https://github.com/{owner}/{name}

      // Create VersionCheckClient
      using var versionCheckClient = new VersionCheckClient("githubtraining", "hellogitworld", VersionCheckMode.Releases);

      // Execute the request
      var results = versionCheckClient.Execute();

      // Get the result for a specific tag.
      var release = results.Releases.FirstOrDefault(r => r.Tag == "RELEASE_1.1");

      if (release?.IsDisabled == true)
      {
        // The release is marked as disabled.
        // Add your code here for how you want to handle a disabled release.
      }
      else
      {
        // The release was not found or it is not marked as disabled.
        // Add your code here for how you want to handle an enabled release.
      }
    }

    [Test]
    [Explicit]
    public void XplitReadMeExampleTest()
    {
      // GitHub repository url format. https://github.com/githubtraining/hellogitworld
      // GitHub repository url format. https://github.com/{owner}/{name}

      // Create VersionCheckClient
      using var versionCheckClient = new VersionCheckClient("githubtraining", "hellogitworld", VersionCheckMode.ReadMe);

      // Execute the request
      var results = versionCheckClient.Execute();

      // Get the result for a specific tag.
      var readMeValues = results.ReadMeValues.FirstOrDefault(r => r.Name == "digitalroot.mods.betterclubs" && r.Version == "1.0.0");

      if (readMeValues == null || readMeValues.IsDisabled == false)
      {

      }

      if (readMeValues?.IsDisabled == true)
      {
        // The release is marked as disabled.
        // Add your code here for how you want to handle a disabled release.
      }
      else
      {
        // The release was not found or it is not marked as disabled.
        // Add your code here for how you want to handle an enabled release.
      }
    }
  }
}
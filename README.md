# VersionCheck

Check a GitHub repository for info about releases. With this 
your code can react to the info returned. 

# Purpose
Restore a level of control to a creator who cares enough 
about their content to implement a solution, like what 
is found in this repository, when a platform limits their 
choices. `Necessity is the mother of invention`

# How it works

The **Version Check Client** fetches info from a GitHub repository 
and looks for infomation in the repository's **ReadMe** or **Releases**. 
The two modes are explained below.

**WARNING:** The GitHub API throttles at 5000 requests per hour. 
Please do not abuse.

## Modes
### ReadMe

The **ReadMe** mode fetches the **README** files from a repository. 
The file can be named _README.md_ or _README_. It is case insensitive. 
If GitHub auto displays your readme file when browsing to your 
repository then this code will work.

The content of the **README** file is parsed looking for html 
comments in the format below. 

``` html
<!-- name|version|IsDisabled -->
```
<br />

Field | Description
------------ | -------------
name | This value is mapped to `ReadMeValues.Name`
version | This value is mapped to `ReadMeValues.Version`
IsDisabled | This value is mapped to `ReadMeValues.IsDisabled` as `true` or `false`. It only maps to `true` if the value is `IsDisabled`. Anything else maps to `false`.

###### Example Readme.md
In this example all pre-v1.0.0 version of the digitalroot.mods.betterclubs mod are disabled.
``` html
<!-- digitalroot.mods.betterclubs|1.0.1|IsEnabled -->
<!-- digitalroot.mods.betterclubs|1.0.0|IsEnabled -->
<!-- digitalroot.mods.betterclubs|0.9.0|IsDisabled -->
<!-- digitalroot.mods.betterclubs|0.9.1|IsDisabled -->
```

### Releases

The **Releases** mode fetches all the **Releases** from a repository. 
The content of the **Releases** (body) is parsed looking for an html  
comment in the format below.

``` html
<!-- IsDisabled -->
```

<br />

Field | Description
------------ | -------------
Name of the release | This value is mapped to `Release.Name`
Tag name of the release | This value is mapped to `Release.Tag`
IsDisabled | This value is mapped to `Release.IsDisabled` as `true` or `false`. It only maps to `true` if `<!-- IsDisabled -->` is found in the release body. If it is not found then `Release.IsDisabled` is set to `false`.

<br />

# Errors
If the **VersionCheckClient** encounters any errors, `Results.IsErrored` 
is set to `true` and `Results.Exception` is set to the exception encountered.

<br />

# Installation
Install it as a NuGet package.
- Supports
  - .NET Standard 2.1
  - .NET 4.6.2
  - .NET 4.7.2
  - .NET 4.8

###### Package Manager
`PM> Install-Package Digitalroot.Valheim.VersionCheck`

###### .NET CLI
`dotnet add package Digitalroot.Valheim.VersionCheck`

# How to use
There are many ways to use the this library. What an author does 
with a disabled release is left up to the author. Here are a few examples.
<br />

###### ReadMe
``` C#
// GitHub repository url format. https://github.com/githubtraining/hellogitworld
// GitHub repository url format. https://github.com/{owner}/{name}

// Create VersionCheckClient
using var versionCheckClient = new VersionCheckClient("githubtraining", "hellogitworld", VersionCheckMode.ReadMe);

// Execute the request
var results = versionCheckClient.Execute();

// Get the result for a specific name and version.
var readMeValues = results.ReadMeValues.FirstOrDefault(r => r.Name == "digitalroot.mods.betterclubs" && r.Version == "1.0.0");

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
```

<br />

###### Releases
``` C#
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
```

<br />

###### Example with a mod using Jotunn

``` C#
namespace Digitalroot.Valheim.BetterClubs
{
  [BepInPlugin(Guid, Name, Version)]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "1.0.0";
    public const string Name = "Digitalroot Better Clubs";
    public const string Guid = "digitalroot.mods.betterclubs";

    private void Awake()
    {
      using var versionCheckClient = new VersionCheckClient("MyGitHubUserName", "MyGitHubRepoName", VersionCheckMode.ReadMe);
      var results = versionCheckClient.Execute();
      var readMeValues = results.ReadMeValues.FirstOrDefault(r => r.Name == Guid && r.Version == Version);

      if (readMeValues == null || readMeValues.IsDisabled == false)
      {
        AddLocalizations();
        ItemManager.OnVanillaItemsAvailable += AddClonedItems;
      }
    }
  }
}
```

<br />

###### Example with a mod using BepInEx

``` C#
namespace Digitalroot.Valheim.BetterClubs
{
  [BepInPlugin(Guid, Name, Version)]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "1.0.0";
    public const string Name = "Digitalroot Better Clubs";
    public const string Guid = "digitalroot.mods.betterclubs";
    private Harmony _harmony;

    private void Awake()
    {
      using var versionCheckClient = new VersionCheckClient("MyGitHubUserName", "MyGitHubRepoName", VersionCheckMode.ReadMe);
      var results = versionCheckClient.Execute();
      var readMeValues = results.ReadMeValues.FirstOrDefault(r => r.Name == Guid && r.Version == Version);

      if (readMeValues == null || readMeValues.IsDisabled == false)
      {
        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
      }
    }
  }
}
```

<br />

# Disclamer
Any refrences or inference to anything real, anyone real or 
real events is unintentional and purely coincidental.

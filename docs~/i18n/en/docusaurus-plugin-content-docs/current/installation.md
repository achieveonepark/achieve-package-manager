---
sidebar_position: 3
---

# Installation

## 1. Add via Git URL in Unity Package Manager (recommended)

Open `Window > Package Manager` in Unity, click the **+** button in the top-left, select **Add package from git URL...**, then paste:

```
https://github.com/achieveonepark/achieve-package-manager.git
```

To pin a specific version, append a tag:

```
https://github.com/achieveonepark/achieve-package-manager.git#1.0.0
```

## 2. Edit `manifest.json` directly

Open `<Project>/Packages/manifest.json` and add an entry to `dependencies`:

```json
{
  "dependencies": {
    "com.achieve.achieve-package-manager": "https://github.com/achieveonepark/achieve-package-manager.git"
  }
}
```

Unity will fetch the package on the next domain reload.

## Requirements

* Unity **2022.3** or newer
* `com.unity.nuget.newtonsoft-json` (installed automatically as a dependency)
* `com.unity.serialization` (installed automatically as a dependency)
* Network access (for the OpenUPM catalog and Git clones)

## Updating

This package is a Git dependency, so Unity does not auto-update it. To get the latest commit:

* Open Package Manager, select the package, and click **Update**, or
* Delete `Library/PackageCache/com.achieve.achieve-package-manager@*` and let Unity re-fetch

If you pinned a tag, change the tag in `manifest.json` to the new version.

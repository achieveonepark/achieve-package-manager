# Custom Git URL Tab

Any package not in the curated list can still be installed with one click — just paste its Git URL.

## Supported URL formats

Anything UPM accepts as a Git URL works:

* `https://github.com/user/repo.git`
* `git@github.com:user/repo.git` (SSH keys must be configured)
* `https://gitlab.com/user/repo.git`
* `https://bitbucket.org/user/repo.git`
* `?path=` to point at a subfolder  
  e.g. `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`
* `#branch` or `#tag` to pin a version  
  e.g. `https://github.com/Cysharp/UniTask.git#2.5.0`

## Input fields

| Field | Description |
|-------|-------------|
| Git URL | Required. A URL in the format above. |
| Package name (optional) | Optional. Used as the manifest key if provided. |

## Inferred package name

Leave `Package name` blank and the tool builds a name from the URL's last segment:

```
https://github.com/Cysharp/UniTask.git
→ com.git.unitask
```

The name is always lowercase, in the form `com.git.<repo>`.

> If the inferred name is ugly or doesn't match the package's actual `package.json` name, **enter the correct name manually**. Matching the real package id is what lets Unity detect duplicates and "already installed" state correctly.

## Validation

Before writing to the manifest, the tool validates the package name:

* Only `a-z`, `0-9`, `-`, `_`, `.` allowed
* First character must be a lowercase letter or digit
* Length ≤ 214

If the name fails validation the manifest is not written, a `Failed to install ...` notification is shown, and a Console error is logged. (A single invalid name can block the entire manifest from resolving.)

## Installed Git packages list

Below the input card, a list shows the **Git-sourced packages** currently in the project. Same row format as the Installed tab; the `Remove` button works the same way.

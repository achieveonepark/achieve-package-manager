# Installed Tab

Shows packages currently registered in `Packages/manifest.json` along with their direct/indirect dependencies.

## Columns

| Column | Description |
|--------|-------------|
| Package | Display name + package id (e.g. `com.cysharp.unitask`) |
| Version | Installed version |
| Source | Auto-detected source — `Registry` / `Git` / `Local` / `Embedded` / `Built-in` |
| (action) | `Remove` button when removable |

## Source detection rules

* `packageId` contains `https://`, `git@`, or `.git` → **Git**
* `packageId` starts with `file:` → **Local**
* `PackageSource.BuiltIn` or `com.unity.modules.*` → **Built-in** (Remove disabled)
* Anything else → Unity's reported `PackageSource`

## Remove behavior

The `Remove` button:

1. Shows a confirmation dialog
2. Deletes the entry from `manifest.json` dependencies
3. Calls `Client.Resolve()` so Unity re-resolves immediately

> **Built-in modules (`com.unity.modules.*`) and this package itself cannot be removed for safety.**

## Unresolved entries

Entries that exist in `manifest.json` but Unity could not resolve (invalid names, broken Git repos, network failures, etc.) appear at the top with a **yellow warning banner**:

```
⚠ 1 manifest entry did not resolve. An invalid entry blocks ALL package
   resolution - remove it to recover.
```

Each unresolved entry has its own `Remove` button so you can **recover a broken manifest from inside the tool**.

> If the manifest is broken badly enough that Package Manager itself fails to load, edit `Packages/manifest.json` directly to delete the bad entry. See [Troubleshooting](/en/guides/troubleshooting).

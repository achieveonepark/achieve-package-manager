# Architecture

## Folder layout

```
Editor/
├── AchievePackageManagerEditorWindow.cs   # UI Toolkit window
├── Dto/
│   ├── PackageCenter.cs                   # manifest read/write + package list
│   └── OpenUpmPackageInfo.cs              # OpenUPM package DTO
└── Services/
    ├── OpenUpmService.cs                  # OpenUPM API client
    └── CuratedPackages.cs                 # curated Git package list
```

## Core components

### `PackageCenter` (static, `[InitializeOnLoad]`)

* Single entry point for reading/writing `manifest.json`
* Calls `Client.List(true)` asynchronously and caches `InstalledPackages`
* Exposes an `InstalledPackagesRefreshed` event for subscribers
* After every write: `AssetDatabase.Refresh()` → `Client.Resolve()` → re-fetch list
* `IsValidPackageName` — validates UPM package-name rules
* `ManifestData.ToJson()` — preserves unknown top-level fields when serializing (so `lockFile`, etc., are not lost)

### `OpenUpmService` (static)

* Calls `https://package.openupm.com/-/all` (entire catalog in one request)
* In-memory cache (`_cache`) — one request per session
* `forceRefresh` option to invalidate the cache
* `ParseAll` calls `PackageCenter.IsValidPackageName` to pre-filter npm-scoped names
* `FetchPackageDetail` — per-package detail endpoint (not used by the UI today, kept for extension)

### `CuratedPackages` (static list)

* Compile-time list — no external calls
* Each entry: `Id`, `DisplayName`, `Description`, `GitUrl`, `Repository`, `Category`
* The Featured tab groups this list by `Category` and renders cards

### `AchievePackageManagerEditorWindow`

* `EditorWindow` + UI Toolkit (`rootVisualElement`)
* Single window; tab content is rebuilt by `Clear()` + re-add
* Styles are inline (`IStyle`) — no USS file required

## Data flow (OpenUPM install)

```
UI click (Install)
    ↓
window.InstallOpenUpm(pkg)
    ↓
PackageCenter.AddOpenUpmPackage(name, version)
    ↓
1) IsValidPackageName check
2) LoadManifest()
3) Manifest.RegisterOpenUpmScope(name)   ← auto-register scope
4) Manifest.AddDependency(name, version)
5) SaveManifest()                        ← write manifest.json to disk
6) ResolveAndRefresh()
    ├ AssetDatabase.Refresh()
    ├ Client.Resolve()                   ← Unity downloads the new package
    └ Refresh() → Client.List(true)      ← UI updates
```

## External API usage

| Call | Frequency | Notes |
|------|-----------|-------|
| `https://package.openupm.com/-/all` | First OpenUPM tab open + Refresh | Cached in memory, once per session |
| GitHub API | **Never** | Git installs only edit the manifest; Unity does the `git clone` |
| `Client.List` / `Client.Resolve` | On every refresh / write | Local operations |

Far below any rate limit you'd run into.

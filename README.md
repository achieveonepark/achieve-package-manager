# Achieve Package Manager

A clean UI Toolkit-based package manager for Unity. Install OpenUPM packages, curated GitHub utilities, or any Git URL with one click - straight into `Packages/manifest.json`.

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)
![License](https://img.shields.io/badge/license-MIT-blue)

## Features

- **UI Toolkit window** with sidebar categories, search, and status bar
- **Installed** - browse every package in your manifest with source detection (Registry / Git / Local / Built-in)
- **OpenUPM** - browse the entire OpenUPM registry, search, and install with auto-scope registration
- **Featured (Git)** - one-click install for curated utilities (UniTask, NuGetForUnity, VContainer, DOTween, ...)
- **Custom Git URL** - paste any `https://`, `git@`, or `.git` URL and install
- **Remove** any non-built-in package from the manifest

## Installation

Add the package via Git URL from `Window > Package Manager > + > Add package from git URL`:

```
https://github.com/achieveonepark/achieve-package-manager.git
```

Or add the entry directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.achieve.achieve-package-manager": "https://github.com/achieveonepark/achieve-package-manager.git"
  }
}
```

## Usage

Open the window from the Unity menu:

```
Tools > Achieve Package Manager
```

### Installed tab

Shows all packages from `Packages/manifest.json` (plus their dependencies). Each row exposes name, version, and detected source. Non-built-in entries have a **Remove** button.

### OpenUPM tab

Fetches the full OpenUPM package list (`https://package.openupm.com/-/all`). Search filters by name, id, or description. Clicking **Install**:

1. Adds (or merges into) the `package.openupm.com` scoped registry
2. Registers the package scope automatically (e.g. `com.cysharp` for `com.cysharp.unitask`)
3. Writes the package + latest version to `dependencies`
4. Triggers `Client.Resolve()`

### Featured tab

Hand-picked Git packages, grouped by category. One click writes the dependency entry into the manifest. Each card also links to the source repository.

Current curated list:

| Category | Packages |
|----------|----------|
| Async | UniTask, R3 |
| Animation | DOTween, PrimeTween |
| Performance | ZString |
| Messaging | MessagePipe |
| Serialization | Newtonsoft Json, MemoryPack, OdinSerializer |
| Logging | ZLogger |
| Dependency Injection | VContainer, Extenject (Zenject) |
| Networking | Mirror |
| UI | UI Effect, Soft Mask, Particle Effect for uGUI, Unmask for uGUI |
| Editor | NaughtyAttributes, xNode |
| Tools | NuGet for Unity |
| Debug | In-Game Debug Console, Graphy, Asset Usage Detector, Runtime Inspector |

### Custom Git URL tab

Paste any Git URL. The package name is inferred from the URL (`com.git.<repo>` by default) and can be overridden. The installed Git packages are listed below for quick reference.

## How it works

- Manifest reads / writes go through `Editor/Dto/PackageCenter.cs`, which preserves unknown top-level fields and formats JSON consistently.
- OpenUPM browsing uses `UnityWebRequest` against the npm-style `package.openupm.com` endpoints.
- Curated entries live in `Editor/Services/CuratedPackages.cs`. Add or edit entries there to extend the Featured list.
- After every write, the package list is refreshed via `Client.List(true)` and `Client.Resolve()`.

## Requirements

- Unity 2022.3 or newer
- `com.unity.nuget.newtonsoft-json` (added as a dependency)
- Network access (for OpenUPM browsing and Git installation)

## License

MIT

# Featured Tab

Frequently used free and open-source Git packages, grouped by category as cards. One click on `Install` and you're done.

## Current curated list

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

## Anatomy of a card

Each card shows:

* **Display name** (e.g. `UniTask`)
* **Package id** (key written into manifest, e.g. `com.cysharp.unitask`)
* One-line **description**
* **Install / Reinstall** button
* **GitHub** button — opens the repository

The `Install` button writes the card's `GitUrl` value into the manifest dependency. Most are `https://github.com/...git?path=...` URLs, but for OpenUPM-standard packages (e.g. Newtonsoft Json) a version string like `3.2.1` is written instead.

## Adding or editing entries

Edit the static list in `Editor/Services/CuratedPackages.cs`. See [Extending the curated list](/en/reference/extending-curated-list) for full details.

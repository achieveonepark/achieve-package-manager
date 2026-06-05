---
sidebar_position: 2
---

# Getting Started

## Opening the window

From the Unity menu:

```
Tools > Achieve Package Manager
```

## Layout

```
┌─────────────────────────────────────────────────────────┐
│ Achieve Package Manager   ↻ Refresh  [search]  Open manifest │  ← toolbar
├──────────┬──────────────────────────────────────────────┤
│ CATEGORIES                                              │
│ ▸ Installed     │  (active tab content)                 │
│ ▸ OpenUPM       │                                       │
│ ▸ Featured (Git)│                                       │
│ ▸ Custom Git URL│                                       │
├──────────┴──────────────────────────────────────────────┤
│ N packages installed                                    │  ← status bar
└─────────────────────────────────────────────────────────┘
```

* **Toolbar** — refresh, search, and "open manifest.json"
* **Sidebar** — four category tabs
* **Status bar** — installed package count and refresh state

## The four tabs

| Tab | Purpose |
|-----|---------|
| [Installed](/en/guides/installed) | View and remove packages in the manifest, surface unresolved entries |
| [OpenUPM](/en/guides/openupm) | Browse and install from the OpenUPM registry |
| [Featured (Git)](/en/guides/featured) | One-click install for curated Git packages |
| [Custom Git URL](/en/guides/git-url) | Install any package from a Git URL |

## How search works

The toolbar search applies to the **active tab's list**:

* Installed — filters by name and id
* OpenUPM — filters by name, id, and description
* Featured — filters by name, id, description, and category
* Custom Git URL — filters the list of installed Git packages

## Refresh

The `↻ Refresh` button:

1. Re-fetches the Unity package list (`Client.List`)
2. Force-refreshes the OpenUPM catalog

The OpenUPM catalog is cached for the session, so you rarely need to hit Refresh.

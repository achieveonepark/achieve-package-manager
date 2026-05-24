# Troubleshooting

## `Project has invalid dependencies: <name> is invalid.`

Cause: an entry in `manifest.json` violates UPM package-name rules. Unity **rejects the whole resolve** when even one entry is invalid, which is why every install afterward fails with the same error.

**Recover:**

1. **From the tool (preferred)** — if Package Manager still loads, open `Tools > Achieve Package Manager > Installed`. The bad entry appears at the top with a yellow warning banner and a `Remove` button.
2. **Manually** — if the tool itself does not load, open `<Project>/Packages/manifest.json` and delete the offending line. Don't forget to clean up the surrounding commas.

The tool blocks invalid names at install time now, so this won't reoccur.

## `error CS0246: 'ToolbarSearchField' could not be found`

The `UnityEditor.UIElements` using directive was missing. It is already present in current code — if you see this error, update the package.

## Toolbar appears compressed at the top

`flexShrink` defaults to 1 in UI Toolkit, so when content grew tall the toolbar and status bar were being squeezed. Fixed in 1.0.0+ by setting `flexShrink = 0` on the toolbar, sidebar, and status bar. Update the package if you're on an older revision.

## Package isn't updating to the latest version

Git dependencies are not auto-updated by Unity.

* Open Package Manager, select the package, click **Update**
* If that doesn't help, delete `Library/PackageCache/com.achieve.achieve-package-manager@*` and restart Unity
* Or remove the entry from `manifest.json` and re-add it

## OpenUPM list is empty or fails to load

* Verify network access to `https://package.openupm.com/-/all`
* In restricted corporate networks, OpenUPM may need to be allow-listed
* Click `↻ Refresh` to re-request

The response is several MB, so the first load may take a few seconds.

## Manifest looks reformatted / alphabetized

When the tool rewrites `manifest.json`, dependencies and scopes are sorted alphabetically. This is intentional and does not change the manifest's meaning.

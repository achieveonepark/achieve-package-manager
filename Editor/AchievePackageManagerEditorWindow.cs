using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Achieve.Package.Manager
{
    public class AchievePackageManagerEditorWindow : EditorWindow
    {
        private enum Tab { Installed, OpenUpm, Featured, GitUrl }

        // Palette (kept consistent across sections).
        private static readonly Color BgRoot       = new Color(0.18f, 0.18f, 0.18f);
        private static readonly Color BgPanel      = new Color(0.22f, 0.22f, 0.22f);
        private static readonly Color BgSidebar    = new Color(0.16f, 0.16f, 0.16f);
        private static readonly Color BgRow        = new Color(0.24f, 0.24f, 0.24f);
        private static readonly Color BgRowAlt     = new Color(0.26f, 0.26f, 0.26f);
        private static readonly Color BgRowHover   = new Color(0.30f, 0.30f, 0.30f);
        private static readonly Color BgSelected   = new Color(0.20f, 0.40f, 0.68f);
        private static readonly Color BorderColor  = new Color(0.10f, 0.10f, 0.10f);
        private static readonly Color TextPrimary  = new Color(0.92f, 0.92f, 0.92f);
        private static readonly Color TextMuted    = new Color(0.65f, 0.65f, 0.65f);
        private static readonly Color AccentColor  = new Color(0.32f, 0.62f, 0.93f);
        private static readonly Color AccentHover  = new Color(0.42f, 0.72f, 1.00f);
        private static readonly Color DangerColor  = new Color(0.78f, 0.32f, 0.32f);

        private Tab _activeTab = Tab.Installed;
        private string _search = string.Empty;

        // Cached UI roots per tab.
        private VisualElement _contentRoot;
        private Label _statusLabel;
        private ToolbarSearchField _searchField;
        private readonly Dictionary<Tab, Button> _tabButtons = new Dictionary<Tab, Button>();

        // Data caches.
        private List<OpenUpmPackageInfo> _openUpmAll;
        private bool _openUpmLoading;
        private string _openUpmError;

        [MenuItem("Tools/Achieve Package Manager")]
        public static void ShowWindow()
        {
            var w = GetWindow<AchievePackageManagerEditorWindow>("Achieve PM");
            w.minSize = new Vector2(880, 480);
            w.Show();
        }

        private void OnEnable()
        {
            BuildUi();
            PackageCenter.InstalledPackagesRefreshed += OnPackagesRefreshed;
        }

        private void OnDisable()
        {
            PackageCenter.InstalledPackagesRefreshed -= OnPackagesRefreshed;
        }

        private void OnPackagesRefreshed()
        {
            UpdateStatus();
            RenderActiveTab();
        }

        // ---------- Layout ----------

        private void BuildUi()
        {
            var root = rootVisualElement;
            root.Clear();
            root.style.flexDirection = FlexDirection.Column;
            root.style.backgroundColor = BgRoot;

            root.Add(BuildToolbar());

            var body = new VisualElement();
            body.style.flexDirection = FlexDirection.Row;
            body.style.flexGrow = 1;
            root.Add(body);

            body.Add(BuildSidebar());

            _contentRoot = new VisualElement();
            _contentRoot.style.flexGrow = 1;
            _contentRoot.style.backgroundColor = BgPanel;
            _contentRoot.style.paddingLeft = 12;
            _contentRoot.style.paddingRight = 12;
            _contentRoot.style.paddingTop = 10;
            _contentRoot.style.paddingBottom = 10;
            body.Add(_contentRoot);

            root.Add(BuildStatusBar());

            SelectTab(Tab.Installed);
        }

        private VisualElement BuildToolbar()
        {
            var bar = new VisualElement();
            bar.style.flexDirection = FlexDirection.Row;
            bar.style.alignItems = Align.Center;
            bar.style.height = 34;
            bar.style.paddingLeft = 10;
            bar.style.paddingRight = 10;
            bar.style.backgroundColor = BgSidebar;
            bar.style.borderBottomWidth = 1;
            bar.style.borderBottomColor = BorderColor;

            var title = new Label("Achieve Package Manager");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.color = TextPrimary;
            title.style.fontSize = 13;
            title.style.marginRight = 16;
            bar.Add(title);

            var refresh = AccentButton("↻  Refresh", () =>
            {
                OpenUpmService.FetchAll(_ => { }, err => Debug.LogError($"[AchievePM] {err}"), forceRefresh: true);
                PackageCenter.Refresh();
                UpdateStatus();
            });
            refresh.style.marginRight = 10;
            bar.Add(refresh);

            _searchField = new ToolbarSearchField();
            _searchField.style.flexGrow = 1;
            _searchField.style.maxWidth = 360;
            _searchField.RegisterValueChangedCallback(evt =>
            {
                _search = evt.newValue ?? string.Empty;
                RenderActiveTab();
            });
            bar.Add(_searchField);

            var openManifest = SecondaryButton("Open manifest.json", () =>
            {
                EditorUtility.RevealInFinder(PackageCenter.ManifestPath);
            });
            openManifest.style.marginLeft = 10;
            bar.Add(openManifest);

            return bar;
        }

        private VisualElement BuildSidebar()
        {
            var side = new VisualElement();
            side.style.width = 200;
            side.style.backgroundColor = BgSidebar;
            side.style.borderRightWidth = 1;
            side.style.borderRightColor = BorderColor;
            side.style.paddingTop = 8;
            side.style.paddingBottom = 8;

            AddSidebarLabel(side, "CATEGORIES");
            AddTabButton(side, Tab.Installed, "Installed");
            AddTabButton(side, Tab.OpenUpm,   "OpenUPM");
            AddTabButton(side, Tab.Featured,  "Featured (Git)");
            AddTabButton(side, Tab.GitUrl,    "Custom Git URL");

            return side;
        }

        private VisualElement BuildStatusBar()
        {
            var bar = new VisualElement();
            bar.style.height = 22;
            bar.style.flexDirection = FlexDirection.Row;
            bar.style.alignItems = Align.Center;
            bar.style.paddingLeft = 10;
            bar.style.paddingRight = 10;
            bar.style.backgroundColor = BgSidebar;
            bar.style.borderTopWidth = 1;
            bar.style.borderTopColor = BorderColor;

            _statusLabel = new Label();
            _statusLabel.style.color = TextMuted;
            _statusLabel.style.fontSize = 11;
            bar.Add(_statusLabel);

            UpdateStatus();
            return bar;
        }

        private void AddSidebarLabel(VisualElement parent, string text)
        {
            var l = new Label(text);
            l.style.color = TextMuted;
            l.style.fontSize = 10;
            l.style.unityFontStyleAndWeight = FontStyle.Bold;
            l.style.marginTop = 6;
            l.style.marginBottom = 4;
            l.style.marginLeft = 14;
            parent.Add(l);
        }

        private void AddTabButton(VisualElement parent, Tab tab, string text)
        {
            var btn = new Button(() => SelectTab(tab)) { text = text };
            btn.style.height = 30;
            btn.style.marginLeft = 6;
            btn.style.marginRight = 6;
            btn.style.marginTop = 2;
            btn.style.marginBottom = 2;
            btn.style.unityTextAlign = TextAnchor.MiddleLeft;
            btn.style.paddingLeft = 12;
            btn.style.backgroundColor = BgSidebar;
            btn.style.borderBottomWidth = btn.style.borderTopWidth = btn.style.borderLeftWidth = btn.style.borderRightWidth = 0;
            btn.style.color = TextPrimary;
            btn.style.borderTopLeftRadius = btn.style.borderTopRightRadius = btn.style.borderBottomLeftRadius = btn.style.borderBottomRightRadius = 4;
            btn.RegisterCallback<MouseEnterEvent>(_ =>
            {
                if (_activeTab != tab) btn.style.backgroundColor = BgRowHover;
            });
            btn.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                if (_activeTab != tab) btn.style.backgroundColor = BgSidebar;
            });
            _tabButtons[tab] = btn;
            parent.Add(btn);
        }

        private void SelectTab(Tab tab)
        {
            _activeTab = tab;
            foreach (var kv in _tabButtons)
            {
                kv.Value.style.backgroundColor = (kv.Key == tab) ? BgSelected : BgSidebar;
                kv.Value.style.color = TextPrimary;
            }
            RenderActiveTab();
        }

        private void RenderActiveTab()
        {
            if (_contentRoot == null) return;
            _contentRoot.Clear();

            switch (_activeTab)
            {
                case Tab.Installed: RenderInstalledTab(); break;
                case Tab.OpenUpm:   RenderOpenUpmTab();   break;
                case Tab.Featured:  RenderFeaturedTab();  break;
                case Tab.GitUrl:    RenderGitUrlTab();    break;
            }
        }

        private void UpdateStatus()
        {
            if (_statusLabel == null) return;
            var installed = PackageCenter.InstalledPackages?.Count ?? 0;
            var loadingTxt = PackageCenter.IsRefreshing ? "  •  Refreshing…" : string.Empty;
            _statusLabel.text = $"{installed} packages installed{loadingTxt}";
        }

        // ---------- Installed tab ----------

        private void RenderInstalledTab()
        {
            _contentRoot.Add(SectionTitle("Installed Packages",
                "Packages currently listed in Packages/manifest.json (plus built-in dependencies)."));

            var packages = (PackageCenter.InstalledPackages ?? new List<PackageInfo>())
                .Where(p => string.IsNullOrEmpty(_search)
                    || (p.displayName ?? string.Empty).IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.name ?? string.Empty).IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.displayName ?? p.name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var list = ScrollPanel();
            _contentRoot.Add(list);

            if (packages.Count == 0)
            {
                list.Add(EmptyState("No installed packages match the current filter."));
                return;
            }

            list.Add(InstalledHeaderRow());

            var alt = false;
            foreach (var pkg in packages)
            {
                list.Add(InstalledRow(pkg, alt));
                alt = !alt;
            }
        }

        private VisualElement InstalledHeaderRow()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.paddingLeft = 12;
            row.style.paddingRight = 12;
            row.style.height = 26;
            row.style.alignItems = Align.Center;
            row.style.borderBottomWidth = 1;
            row.style.borderBottomColor = BorderColor;
            row.Add(HeaderCell("Package",  flexGrow: 2));
            row.Add(HeaderCell("Version",  width: 140));
            row.Add(HeaderCell("Source",   width: 110));
            row.Add(HeaderCell(string.Empty, width: 110));
            return row;
        }

        private VisualElement InstalledRow(PackageInfo pkg, bool alt)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            row.style.paddingLeft = 12;
            row.style.paddingRight = 12;
            row.style.height = 34;
            row.style.backgroundColor = alt ? BgRowAlt : BgRow;
            row.RegisterCallback<MouseEnterEvent>(_ => row.style.backgroundColor = BgRowHover);
            row.RegisterCallback<MouseLeaveEvent>(_ => row.style.backgroundColor = alt ? BgRowAlt : BgRow);

            var nameCol = new VisualElement();
            nameCol.style.flexGrow = 2;
            nameCol.style.flexDirection = FlexDirection.Column;

            var display = new Label(pkg.displayName ?? pkg.name);
            display.style.color = TextPrimary;
            display.style.unityFontStyleAndWeight = FontStyle.Bold;
            display.style.fontSize = 12;
            nameCol.Add(display);

            var sub = new Label(pkg.name);
            sub.style.color = TextMuted;
            sub.style.fontSize = 10;
            nameCol.Add(sub);

            row.Add(nameCol);

            row.Add(BodyCell(pkg.version, width: 140, muted: true));
            row.Add(BodyCell(SourceLabel(pkg), width: 110, muted: true));

            var action = new VisualElement();
            action.style.width = 110;
            action.style.flexDirection = FlexDirection.Row;
            action.style.justifyContent = Justify.FlexEnd;

            if (CanRemove(pkg))
            {
                var btn = DangerButton("Remove", () => OnRemoveClicked(pkg));
                action.Add(btn);
            }
            else
            {
                var l = new Label("Built-in");
                l.style.color = TextMuted;
                l.style.fontSize = 11;
                action.Add(l);
            }
            row.Add(action);

            return row;
        }

        private static string SourceLabel(PackageInfo pkg)
        {
            if (!string.IsNullOrEmpty(pkg.packageId))
            {
                if (pkg.packageId.Contains("https://") || pkg.packageId.Contains("git@") || pkg.packageId.Contains(".git"))
                    return "Git";
                if (pkg.packageId.Contains("file:"))
                    return "Local";
            }

            switch (pkg.source)
            {
                case PackageSource.Registry: return "Registry";
                case PackageSource.BuiltIn:  return "Built-in";
                case PackageSource.Embedded: return "Embedded";
                case PackageSource.Local:    return "Local";
                case PackageSource.Git:      return "Git";
                case PackageSource.LocalTarball: return "Tarball";
                default: return pkg.source.ToString();
            }
        }

        private static bool CanRemove(PackageInfo pkg)
        {
            // Don't let users remove built-ins, modules, or this package itself.
            if (pkg.source == PackageSource.BuiltIn) return false;
            if (pkg.name.StartsWith("com.unity.modules.")) return false;
            if (pkg.name == "com.achieve.achieve-package-manager") return false;
            return true;
        }

        private void OnRemoveClicked(PackageInfo pkg)
        {
            if (!EditorUtility.DisplayDialog(
                "Remove package",
                $"Remove '{pkg.displayName ?? pkg.name}' from manifest.json?",
                "Remove", "Cancel")) return;

            if (PackageCenter.RemoveDependency(pkg.name))
            {
                ShowNotification(new GUIContent($"Removed {pkg.name}"));
            }
            else
            {
                EditorUtility.DisplayDialog("Remove package",
                    "Package was not found in dependencies (may be an indirect dependency).", "OK");
            }
        }

        // ---------- OpenUPM tab ----------

        private void RenderOpenUpmTab()
        {
            _contentRoot.Add(SectionTitle("OpenUPM",
                "Browse and install packages from the OpenUPM registry. Selecting a package will register its scope and add it to your manifest."));

            var list = ScrollPanel();
            _contentRoot.Add(list);

            if (_openUpmAll == null && !_openUpmLoading)
            {
                _openUpmLoading = true;
                _openUpmError = null;
                OpenUpmService.FetchAll(
                    onSuccess: pkgs =>
                    {
                        _openUpmAll = pkgs.ToList();
                        _openUpmLoading = false;
                        if (_activeTab == Tab.OpenUpm) RenderActiveTab();
                    },
                    onError: err =>
                    {
                        _openUpmError = err;
                        _openUpmLoading = false;
                        if (_activeTab == Tab.OpenUpm) RenderActiveTab();
                    });
            }

            if (_openUpmLoading)
            {
                list.Add(EmptyState("Loading OpenUPM registry…"));
                return;
            }

            if (!string.IsNullOrEmpty(_openUpmError))
            {
                list.Add(EmptyState($"Failed to load OpenUPM packages: {_openUpmError}"));
                return;
            }

            var filtered = (_openUpmAll ?? new List<OpenUpmPackageInfo>())
                .Where(p => string.IsNullOrEmpty(_search)
                    || (p.Name ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.DisplayName ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.Description ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0)
                .Take(500)
                .ToList();

            var countLabel = new Label(
                $"{filtered.Count} of {_openUpmAll?.Count ?? 0} packages" +
                (filtered.Count == 500 ? " (showing first 500 - refine search to narrow)" : string.Empty));
            countLabel.style.color = TextMuted;
            countLabel.style.fontSize = 10;
            countLabel.style.marginBottom = 6;
            list.Add(countLabel);

            var alt = false;
            foreach (var pkg in filtered)
            {
                list.Add(OpenUpmRow(pkg, alt));
                alt = !alt;
            }
        }

        private VisualElement OpenUpmRow(OpenUpmPackageInfo pkg, bool alt)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            row.style.paddingLeft = 12;
            row.style.paddingRight = 12;
            row.style.paddingTop = 6;
            row.style.paddingBottom = 6;
            row.style.backgroundColor = alt ? BgRowAlt : BgRow;
            row.RegisterCallback<MouseEnterEvent>(_ => row.style.backgroundColor = BgRowHover);
            row.RegisterCallback<MouseLeaveEvent>(_ => row.style.backgroundColor = alt ? BgRowAlt : BgRow);

            var info = new VisualElement();
            info.style.flexGrow = 1;
            info.style.flexDirection = FlexDirection.Column;

            var top = new VisualElement();
            top.style.flexDirection = FlexDirection.Row;
            top.style.alignItems = Align.Center;

            var name = new Label(pkg.DisplayName ?? pkg.Name);
            name.style.color = TextPrimary;
            name.style.unityFontStyleAndWeight = FontStyle.Bold;
            name.style.fontSize = 12;
            name.style.marginRight = 8;
            top.Add(name);

            if (!string.IsNullOrEmpty(pkg.LatestVersion))
            {
                var ver = new Label(pkg.LatestVersion);
                ver.style.color = TextMuted;
                ver.style.fontSize = 10;
                top.Add(ver);
            }
            info.Add(top);

            var pkgId = new Label(pkg.Name);
            pkgId.style.color = TextMuted;
            pkgId.style.fontSize = 10;
            info.Add(pkgId);

            if (!string.IsNullOrEmpty(pkg.Description))
            {
                var desc = new Label(Truncate(pkg.Description, 180));
                desc.style.color = TextMuted;
                desc.style.fontSize = 11;
                desc.style.marginTop = 2;
                desc.style.whiteSpace = WhiteSpace.Normal;
                info.Add(desc);
            }

            row.Add(info);

            var actions = new VisualElement();
            actions.style.flexDirection = FlexDirection.Row;
            actions.style.alignItems = Align.Center;
            actions.style.marginLeft = 10;

            var installed = IsInstalled(pkg.Name);
            if (installed)
            {
                var badge = new Label("Installed");
                badge.style.color = AccentColor;
                badge.style.fontSize = 11;
                badge.style.marginRight = 6;
                actions.Add(badge);
            }

            var install = AccentButton(installed ? "Reinstall" : "Install",
                () => InstallOpenUpm(pkg));
            actions.Add(install);

            row.Add(actions);
            return row;
        }

        private void InstallOpenUpm(OpenUpmPackageInfo pkg)
        {
            if (string.IsNullOrEmpty(pkg.LatestVersion))
            {
                EditorUtility.DisplayDialog("Install",
                    $"No version metadata available for {pkg.Name}.", "OK");
                return;
            }

            var result = PackageCenter.AddOpenUpmPackage(pkg.Name, pkg.LatestVersion);
            ShowResultNotification(pkg.Name, result);
        }

        // ---------- Featured tab ----------

        private void RenderFeaturedTab()
        {
            _contentRoot.Add(SectionTitle("Featured Utilities",
                "Hand-picked Git packages. One click writes the entry into Packages/manifest.json."));

            var list = ScrollPanel();
            _contentRoot.Add(list);

            var filtered = CuratedPackages.All
                .Where(p => string.IsNullOrEmpty(_search)
                    || (p.DisplayName ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.Id ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.Description ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0
                    || (p.Category ?? "").IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0)
                .GroupBy(p => p.Category ?? "Other")
                .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

            var any = false;
            foreach (var group in filtered)
            {
                any = true;
                var header = new Label(group.Key.ToUpperInvariant());
                header.style.color = TextMuted;
                header.style.fontSize = 10;
                header.style.unityFontStyleAndWeight = FontStyle.Bold;
                header.style.marginTop = 8;
                header.style.marginBottom = 6;
                list.Add(header);

                var grid = new VisualElement();
                grid.style.flexDirection = FlexDirection.Row;
                grid.style.flexWrap = Wrap.Wrap;
                list.Add(grid);

                foreach (var pkg in group.OrderBy(p => p.DisplayName, StringComparer.OrdinalIgnoreCase))
                    grid.Add(CuratedCard(pkg));
            }

            if (!any) list.Add(EmptyState("No featured packages match the current filter."));
        }

        private VisualElement CuratedCard(CuratedPackage pkg)
        {
            var card = new VisualElement();
            card.style.width = 320;
            card.style.minHeight = 140;
            card.style.marginRight = 10;
            card.style.marginBottom = 10;
            card.style.paddingLeft = 12;
            card.style.paddingRight = 12;
            card.style.paddingTop = 10;
            card.style.paddingBottom = 10;
            card.style.backgroundColor = BgRow;
            card.style.borderTopLeftRadius = card.style.borderTopRightRadius =
                card.style.borderBottomLeftRadius = card.style.borderBottomRightRadius = 6;
            card.style.borderLeftWidth = card.style.borderRightWidth =
                card.style.borderTopWidth = card.style.borderBottomWidth = 1;
            card.style.borderLeftColor = card.style.borderRightColor =
                card.style.borderTopColor = card.style.borderBottomColor = BorderColor;
            card.RegisterCallback<MouseEnterEvent>(_ => card.style.backgroundColor = BgRowHover);
            card.RegisterCallback<MouseLeaveEvent>(_ => card.style.backgroundColor = BgRow);

            var title = new Label(pkg.DisplayName);
            title.style.color = TextPrimary;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 13;
            card.Add(title);

            var id = new Label(pkg.Id);
            id.style.color = TextMuted;
            id.style.fontSize = 10;
            id.style.marginBottom = 4;
            card.Add(id);

            var desc = new Label(pkg.Description);
            desc.style.color = TextMuted;
            desc.style.fontSize = 11;
            desc.style.whiteSpace = WhiteSpace.Normal;
            desc.style.flexGrow = 1;
            card.Add(desc);

            var actions = new VisualElement();
            actions.style.flexDirection = FlexDirection.Row;
            actions.style.marginTop = 8;
            actions.style.alignItems = Align.Center;

            var installed = IsInstalled(pkg.Id);
            if (installed)
            {
                var badge = new Label("Installed");
                badge.style.color = AccentColor;
                badge.style.fontSize = 11;
                badge.style.marginRight = 6;
                actions.Add(badge);
            }

            var install = AccentButton(installed ? "Reinstall" : "Install", () =>
            {
                var result = PackageCenter.AddDependency(pkg.Id, pkg.GitUrl);
                ShowResultNotification(pkg.DisplayName, result);
            });
            install.style.marginRight = 6;
            actions.Add(install);

            if (!string.IsNullOrEmpty(pkg.Repository))
            {
                var view = SecondaryButton("GitHub", () => Application.OpenURL(pkg.Repository));
                actions.Add(view);
            }

            card.Add(actions);
            return card;
        }

        // ---------- Git URL tab ----------

        private string _gitUrlInput = string.Empty;
        private string _gitNameOverride = string.Empty;

        private void RenderGitUrlTab()
        {
            _contentRoot.Add(SectionTitle("Install from Git URL",
                "Paste a Git URL (https://, git@, or .git). The package name will be inferred but can be overridden."));

            var card = new VisualElement();
            card.style.backgroundColor = BgRow;
            card.style.paddingLeft = 12;
            card.style.paddingRight = 12;
            card.style.paddingTop = 10;
            card.style.paddingBottom = 12;
            card.style.borderTopLeftRadius = card.style.borderTopRightRadius =
                card.style.borderBottomLeftRadius = card.style.borderBottomRightRadius = 6;
            card.style.borderLeftWidth = card.style.borderRightWidth =
                card.style.borderTopWidth = card.style.borderBottomWidth = 1;
            card.style.borderLeftColor = card.style.borderRightColor =
                card.style.borderTopColor = card.style.borderBottomColor = BorderColor;
            _contentRoot.Add(card);

            card.Add(FormLabel("Git URL"));
            var urlField = new TextField { value = _gitUrlInput };
            urlField.style.marginBottom = 8;
            urlField.RegisterValueChangedCallback(evt => _gitUrlInput = evt.newValue ?? string.Empty);
            card.Add(urlField);

            card.Add(FormLabel("Package name (optional)"));
            var nameField = new TextField { value = _gitNameOverride };
            nameField.style.marginBottom = 10;
            nameField.RegisterValueChangedCallback(evt => _gitNameOverride = evt.newValue ?? string.Empty);
            card.Add(nameField);

            var actions = new VisualElement();
            actions.style.flexDirection = FlexDirection.Row;

            var install = AccentButton("Install", () =>
            {
                if (string.IsNullOrWhiteSpace(_gitUrlInput))
                {
                    EditorUtility.DisplayDialog("Install from Git", "Please enter a Git URL.", "OK");
                    return;
                }

                AddManifestResult result;
                if (!string.IsNullOrWhiteSpace(_gitNameOverride))
                    result = PackageCenter.AddDependency(_gitNameOverride.Trim(), _gitUrlInput.Trim());
                else
                    result = PackageCenter.AddGitDependency(_gitUrlInput);

                ShowResultNotification(_gitNameOverride.Trim(), result);
                if (result == AddManifestResult.Success || result == AddManifestResult.Update)
                {
                    _gitUrlInput = string.Empty;
                    _gitNameOverride = string.Empty;
                    RenderActiveTab();
                }
            });
            install.style.marginRight = 6;
            actions.Add(install);

            var clear = SecondaryButton("Clear", () =>
            {
                _gitUrlInput = string.Empty;
                _gitNameOverride = string.Empty;
                RenderActiveTab();
            });
            actions.Add(clear);

            card.Add(actions);

            _contentRoot.Add(SectionSubTitle("Installed Git Packages"));

            var list = ScrollPanel();
            _contentRoot.Add(list);

            var gits = (PackageCenter.InstalledPackages ?? new List<PackageInfo>())
                .Where(p => p.source == PackageSource.Git ||
                            (p.packageId != null && (p.packageId.Contains(".git") || p.packageId.Contains("git@"))))
                .OrderBy(p => p.displayName ?? p.name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (gits.Count == 0)
            {
                list.Add(EmptyState("No Git-based packages installed yet."));
                return;
            }

            list.Add(InstalledHeaderRow());
            var alt = false;
            foreach (var pkg in gits)
            {
                list.Add(InstalledRow(pkg, alt));
                alt = !alt;
            }
        }

        // ---------- UI helpers ----------

        private VisualElement SectionTitle(string title, string subtitle)
        {
            var wrap = new VisualElement();
            wrap.style.marginBottom = 8;

            var t = new Label(title);
            t.style.color = TextPrimary;
            t.style.fontSize = 16;
            t.style.unityFontStyleAndWeight = FontStyle.Bold;
            wrap.Add(t);

            if (!string.IsNullOrEmpty(subtitle))
            {
                var s = new Label(subtitle);
                s.style.color = TextMuted;
                s.style.fontSize = 11;
                s.style.whiteSpace = WhiteSpace.Normal;
                s.style.marginTop = 2;
                wrap.Add(s);
            }

            var rule = new VisualElement();
            rule.style.height = 1;
            rule.style.backgroundColor = BorderColor;
            rule.style.marginTop = 8;
            wrap.Add(rule);

            return wrap;
        }

        private VisualElement SectionSubTitle(string title)
        {
            var t = new Label(title);
            t.style.color = TextPrimary;
            t.style.fontSize = 13;
            t.style.unityFontStyleAndWeight = FontStyle.Bold;
            t.style.marginTop = 14;
            t.style.marginBottom = 6;
            return t;
        }

        private VisualElement ScrollPanel()
        {
            var scroll = new ScrollView(ScrollViewMode.Vertical);
            scroll.style.flexGrow = 1;
            scroll.contentContainer.style.paddingRight = 4;
            return scroll;
        }

        private VisualElement EmptyState(string text)
        {
            var l = new Label(text);
            l.style.color = TextMuted;
            l.style.fontSize = 12;
            l.style.marginTop = 14;
            l.style.unityTextAlign = TextAnchor.MiddleCenter;
            return l;
        }

        private VisualElement HeaderCell(string text, float? width = null, float flexGrow = 0)
        {
            var l = new Label(text);
            l.style.color = TextMuted;
            l.style.fontSize = 10;
            l.style.unityFontStyleAndWeight = FontStyle.Bold;
            if (width.HasValue) l.style.width = width.Value;
            if (flexGrow > 0) l.style.flexGrow = flexGrow;
            return l;
        }

        private VisualElement BodyCell(string text, float? width = null, float flexGrow = 0, bool muted = false)
        {
            var l = new Label(text);
            l.style.color = muted ? TextMuted : TextPrimary;
            l.style.fontSize = 11;
            if (width.HasValue) l.style.width = width.Value;
            if (flexGrow > 0) l.style.flexGrow = flexGrow;
            return l;
        }

        private Label FormLabel(string text)
        {
            var l = new Label(text);
            l.style.color = TextMuted;
            l.style.fontSize = 10;
            l.style.unityFontStyleAndWeight = FontStyle.Bold;
            l.style.marginBottom = 4;
            return l;
        }

        private Button AccentButton(string text, Action action)
        {
            var b = new Button(action) { text = text };
            b.style.backgroundColor = AccentColor;
            b.style.color = Color.white;
            b.style.height = 24;
            b.style.paddingLeft = b.style.paddingRight = 12;
            b.style.borderTopLeftRadius = b.style.borderTopRightRadius =
                b.style.borderBottomLeftRadius = b.style.borderBottomRightRadius = 3;
            b.style.borderLeftWidth = b.style.borderRightWidth =
                b.style.borderTopWidth = b.style.borderBottomWidth = 0;
            b.style.unityFontStyleAndWeight = FontStyle.Bold;
            b.RegisterCallback<MouseEnterEvent>(_ => b.style.backgroundColor = AccentHover);
            b.RegisterCallback<MouseLeaveEvent>(_ => b.style.backgroundColor = AccentColor);
            return b;
        }

        private Button SecondaryButton(string text, Action action)
        {
            var b = new Button(action) { text = text };
            b.style.backgroundColor = BgRow;
            b.style.color = TextPrimary;
            b.style.height = 24;
            b.style.paddingLeft = b.style.paddingRight = 12;
            b.style.borderTopLeftRadius = b.style.borderTopRightRadius =
                b.style.borderBottomLeftRadius = b.style.borderBottomRightRadius = 3;
            b.style.borderLeftWidth = b.style.borderRightWidth =
                b.style.borderTopWidth = b.style.borderBottomWidth = 1;
            b.style.borderLeftColor = b.style.borderRightColor =
                b.style.borderTopColor = b.style.borderBottomColor = BorderColor;
            b.RegisterCallback<MouseEnterEvent>(_ => b.style.backgroundColor = BgRowHover);
            b.RegisterCallback<MouseLeaveEvent>(_ => b.style.backgroundColor = BgRow);
            return b;
        }

        private Button DangerButton(string text, Action action)
        {
            var b = new Button(action) { text = text };
            b.style.backgroundColor = new Color(0, 0, 0, 0);
            b.style.color = DangerColor;
            b.style.height = 24;
            b.style.paddingLeft = b.style.paddingRight = 10;
            b.style.borderTopLeftRadius = b.style.borderTopRightRadius =
                b.style.borderBottomLeftRadius = b.style.borderBottomRightRadius = 3;
            b.style.borderLeftWidth = b.style.borderRightWidth =
                b.style.borderTopWidth = b.style.borderBottomWidth = 1;
            b.style.borderLeftColor = b.style.borderRightColor =
                b.style.borderTopColor = b.style.borderBottomColor = DangerColor;
            return b;
        }

        private static bool IsInstalled(string packageName)
        {
            if (PackageCenter.InstalledPackages == null) return false;
            return PackageCenter.InstalledPackages.Any(p =>
                string.Equals(p.name, packageName, StringComparison.Ordinal));
        }

        private void ShowResultNotification(string label, AddManifestResult result)
        {
            string msg;
            switch (result)
            {
                case AddManifestResult.Success: msg = $"Installed {label}"; break;
                case AddManifestResult.Update:  msg = $"Updated {label}"; break;
                case AddManifestResult.Valid:   msg = $"{label} is already up to date"; break;
                case AddManifestResult.NotFound: msg = $"{label} not found"; break;
                default: msg = $"Failed to install {label}"; break;
            }
            ShowNotification(new GUIContent(msg));
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s;
            return s.Substring(0, max - 1) + "…";
        }
    }
}

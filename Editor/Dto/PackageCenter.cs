using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Achieve.Package.Manager
{
    public enum AddManifestResult
    {
        Fail,
        Success,
        Valid,
        Update,
        NotFound,
    }

    [InitializeOnLoad]
    public static class PackageCenter
    {
        internal const string OpenUpmRegistryName = "package.openupm.com";
        internal const string OpenUpmRegistryUrl  = "https://package.openupm.com";

        internal static List<UnityEditor.PackageManager.PackageInfo> InstalledPackages =
            new List<UnityEditor.PackageManager.PackageInfo>();

        internal static event Action InstalledPackagesRefreshed;
        internal static bool IsRefreshing { get; private set; }

        private static ListRequest _listRequest;

        internal static ManifestData Manifest { get; private set; }

        static PackageCenter()
        {
            LoadManifest();
            Refresh();
        }

        internal static string ManifestPath =>
            Path.GetFullPath(Path.Combine(Application.dataPath, "../Packages/manifest.json"));

        internal static void LoadManifest()
        {
            try
            {
                if (!File.Exists(ManifestPath))
                {
                    Debug.LogError($"[AchievePM] manifest.json not found at: {ManifestPath}");
                    Manifest = new ManifestData();
                    return;
                }

                var json = File.ReadAllText(ManifestPath);
                Manifest = ManifestData.Parse(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AchievePM] Failed to parse manifest.json: {e.Message}");
                Manifest = new ManifestData();
            }
        }

        internal static void Refresh()
        {
            if (IsRefreshing) return;
            IsRefreshing = true;

            LoadManifest();
            _listRequest = Client.List(true);
            EditorApplication.update += OnListProgress;
        }

        private static void OnListProgress()
        {
            if (_listRequest == null || !_listRequest.IsCompleted) return;

            if (_listRequest.Status == StatusCode.Success)
            {
                InstalledPackages = _listRequest.Result.ToList();
            }
            else if (_listRequest.Status >= StatusCode.Failure)
            {
                Debug.LogError($"[AchievePM] Failed to retrieve package list: {_listRequest.Error?.message}");
            }

            EditorApplication.update -= OnListProgress;
            _listRequest = null;
            IsRefreshing = false;
            InstalledPackagesRefreshed?.Invoke();
        }

        // UPM package names must be lowercase reverse-DNS (e.g. com.company.tool).
        // Writing anything else into manifest.json breaks ALL package resolution.
        internal static bool IsValidPackageName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (name.Length > 214) return false;

            var first = name[0];
            if (!((first >= 'a' && first <= 'z') || (first >= '0' && first <= '9')))
                return false;

            foreach (var c in name)
            {
                var ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')
                         || c == '-' || c == '_' || c == '.';
                if (!ok) return false;
            }
            return true;
        }

        internal static AddManifestResult AddDependency(string packageName, string versionOrUrl)
        {
            if (string.IsNullOrWhiteSpace(packageName) || string.IsNullOrWhiteSpace(versionOrUrl))
                return AddManifestResult.Fail;

            packageName = packageName.Trim();
            if (!IsValidPackageName(packageName))
            {
                Debug.LogError($"[AchievePM] Invalid UPM package name '{packageName}'. " +
                               "Names must be lowercase reverse-DNS (e.g. com.company.tool).");
                return AddManifestResult.Fail;
            }

            LoadManifest();
            var result = Manifest.AddDependency(packageName, versionOrUrl);
            SaveManifest();
            ResolveAndRefresh();
            return result;
        }

        internal static AddManifestResult AddGitDependency(string gitUrl)
        {
            if (string.IsNullOrWhiteSpace(gitUrl))
                return AddManifestResult.Fail;

            var name = ExtractPackageNameFromGitUrl(gitUrl);
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError($"[AchievePM] Cannot infer package name from Git URL: {gitUrl}");
                return AddManifestResult.Fail;
            }

            return AddDependency(name, gitUrl.Trim());
        }

        internal static AddManifestResult AddOpenUpmPackage(string packageName, string version)
        {
            if (string.IsNullOrWhiteSpace(packageName) || string.IsNullOrWhiteSpace(version))
                return AddManifestResult.Fail;

            packageName = packageName.Trim();
            if (!IsValidPackageName(packageName))
            {
                Debug.LogError($"[AchievePM] Invalid UPM package name '{packageName}'. " +
                               "Names must be lowercase reverse-DNS (e.g. com.company.tool).");
                return AddManifestResult.Fail;
            }

            LoadManifest();
            Manifest.RegisterOpenUpmScope(packageName);
            var result = Manifest.AddDependency(packageName, version);
            SaveManifest();
            ResolveAndRefresh();
            return result;
        }

        internal static bool RemoveDependency(string packageName)
        {
            LoadManifest();
            if (!Manifest.RemoveDependency(packageName)) return false;

            SaveManifest();
            ResolveAndRefresh();
            return true;
        }

        internal static void SaveManifest()
        {
            try
            {
                File.WriteAllText(ManifestPath, Manifest.ToJson());
            }
            catch (Exception e)
            {
                Debug.LogError($"[AchievePM] Failed to write manifest.json: {e.Message}");
            }
        }

        private static void ResolveAndRefresh()
        {
            AssetDatabase.Refresh();
            Client.Resolve();
            Refresh();
        }

        private static string ExtractPackageNameFromGitUrl(string url)
        {
            // Strip query (#branch, ?path=...) and trailing .git, take last segment.
            var clean = url.Trim();
            var hashIdx = clean.IndexOf('#');
            if (hashIdx >= 0) clean = clean.Substring(0, hashIdx);
            var qIdx = clean.IndexOf('?');
            if (qIdx >= 0) clean = clean.Substring(0, qIdx);
            clean = clean.TrimEnd('/');
            if (clean.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
                clean = clean.Substring(0, clean.Length - 4);

            var lastSlash = clean.LastIndexOf('/');
            var repo = lastSlash >= 0 ? clean.Substring(lastSlash + 1) : clean;
            if (string.IsNullOrEmpty(repo)) return null;

            // Use a stable reverse-DNS-style fallback name. User can rename later if needed.
            return $"com.git.{repo.ToLowerInvariant()}";
        }

        public sealed class ManifestData
        {
            public Dictionary<string, string> Dependencies { get; private set; } =
                new Dictionary<string, string>();

            public List<ScopedRegistry> ScopedRegistries { get; private set; } =
                new List<ScopedRegistry>();

            // Preserves any other top-level fields (e.g., enableLockFile, testables) on write.
            private JObject _root;

            public static ManifestData Parse(string json)
            {
                var data = new ManifestData();
                var root = JObject.Parse(json);
                data._root = root;

                if (root["dependencies"] is JObject deps)
                {
                    foreach (var prop in deps.Properties())
                        data.Dependencies[prop.Name] = prop.Value.ToString();
                }

                if (root["scopedRegistries"] is JArray regs)
                {
                    foreach (var item in regs.OfType<JObject>())
                    {
                        data.ScopedRegistries.Add(new ScopedRegistry
                        {
                            name = item.Value<string>("name"),
                            url = item.Value<string>("url"),
                            scopes = item["scopes"]?.Values<string>().ToList() ?? new List<string>(),
                        });
                    }
                }

                return data;
            }

            public AddManifestResult AddDependency(string packageName, string versionOrUrl)
            {
                if (Dependencies.TryGetValue(packageName, out var existing))
                {
                    if (string.Equals(existing, versionOrUrl, StringComparison.Ordinal))
                        return AddManifestResult.Valid;

                    Dependencies[packageName] = versionOrUrl;
                    return AddManifestResult.Update;
                }

                Dependencies.Add(packageName, versionOrUrl);
                return AddManifestResult.Success;
            }

            public bool RemoveDependency(string packageName) => Dependencies.Remove(packageName);

            public void RegisterOpenUpmScope(string packageName)
            {
                var registry = ScopedRegistries.FirstOrDefault(r =>
                    string.Equals(r.url, OpenUpmRegistryUrl, StringComparison.OrdinalIgnoreCase));

                if (registry == null)
                {
                    registry = new ScopedRegistry
                    {
                        name = OpenUpmRegistryName,
                        url = OpenUpmRegistryUrl,
                        scopes = new List<string>(),
                    };
                    ScopedRegistries.Add(registry);
                }

                // Find the broadest matching parent scope (e.g., "com.cysharp" for "com.cysharp.unitask").
                var alreadyCovered = registry.scopes.Any(s =>
                    packageName == s || packageName.StartsWith(s + ".", StringComparison.Ordinal));
                if (alreadyCovered) return;

                var parts = packageName.Split('.');
                var scope = parts.Length >= 2 ? $"{parts[0]}.{parts[1]}" : packageName;
                if (!registry.scopes.Contains(scope))
                    registry.scopes.Add(scope);
            }

            public string ToJson()
            {
                var root = _root ?? new JObject();

                var deps = new JObject();
                foreach (var kv in Dependencies.OrderBy(k => k.Key, StringComparer.Ordinal))
                    deps[kv.Key] = kv.Value;
                root["dependencies"] = deps;

                if (ScopedRegistries.Count > 0)
                {
                    var arr = new JArray();
                    foreach (var r in ScopedRegistries)
                    {
                        var o = new JObject
                        {
                            ["name"] = r.name,
                            ["url"] = r.url,
                            ["scopes"] = new JArray(r.scopes.Distinct().OrderBy(s => s, StringComparer.Ordinal)),
                        };
                        arr.Add(o);
                    }
                    root["scopedRegistries"] = arr;
                }
                else
                {
                    root.Remove("scopedRegistries");
                }

                return root.ToString(Formatting.Indented);
            }

            [Serializable]
            public class ScopedRegistry
            {
                public string name;
                public string url;
                public List<string> scopes = new List<string>();
            }
        }
    }
}

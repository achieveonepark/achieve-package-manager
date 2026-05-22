using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Achieve.Package.Manager
{
    internal static class OpenUpmService
    {
        private const string AllPackagesUrl = "https://package.openupm.com/-/all";
        private const string PackageInfoUrlFormat = "https://package.openupm.com/{0}";

        private static List<OpenUpmPackageInfo> _cache;
        private static bool _fetching;

        internal static IReadOnlyList<OpenUpmPackageInfo> Cache => _cache;

        internal static void FetchAll(
            Action<IReadOnlyList<OpenUpmPackageInfo>> onSuccess,
            Action<string> onError,
            bool forceRefresh = false)
        {
            if (_cache != null && !forceRefresh)
            {
                onSuccess?.Invoke(_cache);
                return;
            }

            if (_fetching) return;
            _fetching = true;

            var request = UnityWebRequest.Get(AllPackagesUrl);
            request.timeout = 30;
            var op = request.SendWebRequest();
            op.completed += _ =>
            {
                try
                {
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        onError?.Invoke($"OpenUPM request failed: {request.error}");
                        return;
                    }

                    _cache = ParseAll(request.downloadHandler.text);
                    onSuccess?.Invoke(_cache);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"OpenUPM parse error: {e.Message}");
                }
                finally
                {
                    request.Dispose();
                    _fetching = false;
                }
            };
        }

        internal static void FetchPackageDetail(
            string packageName,
            Action<OpenUpmPackageInfo> onSuccess,
            Action<string> onError)
        {
            var url = string.Format(PackageInfoUrlFormat, UnityWebRequest.EscapeURL(packageName));
            var request = UnityWebRequest.Get(url);
            request.timeout = 15;
            var op = request.SendWebRequest();
            op.completed += _ =>
            {
                try
                {
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        onError?.Invoke(request.error);
                        return;
                    }

                    var root = JObject.Parse(request.downloadHandler.text);
                    var info = BuildInfo(packageName, root);
                    onSuccess?.Invoke(info);
                }
                catch (Exception e)
                {
                    onError?.Invoke(e.Message);
                }
                finally
                {
                    request.Dispose();
                }
            };
        }

        private static List<OpenUpmPackageInfo> ParseAll(string json)
        {
            var root = JObject.Parse(json);
            var list = new List<OpenUpmPackageInfo>(root.Count);

            foreach (var prop in root.Properties())
            {
                if (prop.Name == "_updated") continue;
                if (!(prop.Value is JObject obj)) continue;

                // OpenUPM's /-/all is an npm-format index and can include npm-scoped
                // names (@scope/name) that are not valid UPM package names. Skip them.
                if (!PackageCenter.IsValidPackageName(prop.Name)) continue;

                list.Add(BuildInfo(prop.Name, obj));
            }

            list.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            return list;
        }

        private static OpenUpmPackageInfo BuildInfo(string name, JObject obj)
        {
            var info = new OpenUpmPackageInfo
            {
                Name = name,
                DisplayName = obj.Value<string>("displayName") ?? name,
                Description = obj.Value<string>("description") ?? string.Empty,
                LatestVersion = obj["dist-tags"]?.Value<string>("latest")
                    ?? obj.Value<string>("version")
                    ?? string.Empty,
            };

            if (obj["versions"] is JObject versionsObj)
            {
                info.Versions = versionsObj.Properties().Select(p => p.Name).ToList();
            }
            else if (obj["versions"] is JArray versionsArr)
            {
                info.Versions = versionsArr.Values<string>().ToList();
            }

            return info;
        }
    }
}

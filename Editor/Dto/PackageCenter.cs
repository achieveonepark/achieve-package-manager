using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Serialization.Json;
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
    public class PackageCenter
    {
        internal static List<UnityEditor.PackageManager.PackageInfo> InstalledPackages = new List<UnityEditor.PackageManager.PackageInfo>();
        private static ListRequest listRequest;

        internal static readonly ManifestData Manifest;
        
        static PackageCenter()
        {
            var manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            
            if (File.Exists(manifestPath))
            {
                var json = File.ReadAllText(manifestPath);
                Manifest = JsonSerialization.FromJson<PackageCenter.ManifestData>(json);
            }
            else
            {
                Debug.LogError($"manifest.json not found at path: {manifestPath}");
                return;
            }
            
            listRequest = Client.List(true); // true: 개발 패키지 포함
            EditorApplication.update += OnPackageListRequestCompleted;
        }
        
        private static void OnPackageListRequestCompleted()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    InstalledPackages = listRequest.Result.ToList();
                }
                else
                {
                    Debug.LogError("Failed to retrieve package list.");
                }

                EditorApplication.update -= OnPackageListRequestCompleted;
            }
        }

        [Serializable]
        public class ManifestData
        {
            public Dictionary<string, string> dependencies;
            public List<ScopedRegistry> scopedRegistries;

            public AddManifestResult AddManifest(string packageName, string version)
            {
                var result = AddManifestResult.Fail;
                
                if (dependencies.TryGetValue(packageName, out var package))
                {
                    if (version == package)
                    {
                        result = AddManifestResult.Valid;
                        return result;
                    }
                    
                    result = AddManifestResult.Update;
                    dependencies.Remove(packageName);
                }
                
                dependencies.Add(packageName, version);

                var manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
                var jsonString = JsonSerialization.ToJson(dependencies);
                File.WriteAllText(manifestPath, jsonString);

                if (result == AddManifestResult.Fail)
                {
                    result = AddManifestResult.Success;
                }
                
                AssetDatabase.Refresh();

                return result;
            }
            
            [Serializable]
            public class ScopedRegistry
            {
                public string name;
                public string url;
                public List<string> scopes;
            }
        }
    }
}
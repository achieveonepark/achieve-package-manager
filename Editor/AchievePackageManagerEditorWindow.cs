using System.IO;
using System.Numerics;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Serialization.Json;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Achieve.Package.Manager
{
    public class AchievePackageManagerEditorWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        
        [MenuItem("Tools/Manifest Data Viewer")]
        public static void ShowWindow()
        {
            GetWindow<AchievePackageManagerEditorWindow>("Manifest Data Viewer");
        }
        
        private void OnGUI()
        {
            DrawToolbar();
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawDependencies();
            EditorGUILayout.Space();

            // Scoped Registries List
            EditorGUILayout.LabelField("Scoped Registries", EditorStyles.boldLabel);
            if (PackageCenter.Manifest.scopedRegistries != null)
            {
                foreach (var registry in PackageCenter.Manifest.scopedRegistries)
                {
                    EditorGUILayout.LabelField("Name:", registry.name, EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("URL:", registry.url);
                    EditorGUILayout.LabelField("Scopes:", string.Join(", ", registry.scopes));
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.EndScrollView();

            if (showPopup)
            {
                DrawPopup();
            }
        }
        
        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // + 버튼 (새 패키지 추가)
                if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    showPopup = !showPopup;
                    // 팝업 창을 열거나, 새로운 패키지를 추가하는 기능 구현 가능
                }             

                if (GUILayout.Button("R", EditorStyles.toolbarButton, GUILayout.Width(30)))
                {
                    Debug.Log("Add package clicked");
                    // 팝업 창을 열거나, 새로운 패키지를 추가하는 기능 구현 가능
                }
            }
        }
        
        private bool showPopup = false;
        private Rect popupRect = new Rect(0, 0, 250, 150);
        private string newPackageName = "";
        private string newPackageVersion = "";
        
        private void DrawDependencies()
        {
            EditorGUILayout.LabelField("Dependencies", EditorStyles.boldLabel);

            if (PackageCenter.InstalledPackages == null || PackageCenter.InstalledPackages.Count == 0)
            {
                EditorGUILayout.HelpBox("No dependencies found.", MessageType.Info);
                return;
            }

            // 테이블 스타일 배경
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Package Name", EditorStyles.boldLabel, GUILayout.Width(300));
                    EditorGUILayout.LabelField("Version", EditorStyles.boldLabel, GUILayout.Width(400));
                    EditorGUILayout.LabelField("Type", EditorStyles.boldLabel, GUILayout.Width(60));
                }

                EditorGUILayout.Space(2);

                foreach (var package in PackageCenter.InstalledPackages)
                {
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.LabelField(package.displayName, GUILayout.Width(300));
                        EditorGUILayout.LabelField(package.version, GUILayout.Width(400));

                        if (package.packageId.Contains("https://"))
                        {
                            GUI.enabled = string.IsNullOrEmpty(package.documentationUrl) is false;
                            if(GUILayout.Button("Github URL", GUILayout.Width(80)))
                            {
                                Application.OpenURL(package.documentationUrl);
                            }

                            GUI.enabled = true;
                        }
                        else if (package.packageId.Contains("file:"))
                        {
                            GUI.enabled = false;
                            GUILayout.Button("Local", GUILayout.Width(80));
                            GUI.enabled = true;
                        }
                        else
                        {
                            GUI.enabled = false;
                            GUILayout.Button("Cached", GUILayout.Width(80));
                            GUI.enabled = true;
                        }
                    }
                }
            }
        }        
        
        private void DrawPopup()
        {
            // 반투명한 배경 추가
            Color backgroundColor = new Color(0, 0, 0, 1);
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), backgroundColor);

            // 팝업 창 UI
            Rect centeredRect = new Rect((position.width - popupRect.width) / 2, 
                (position.height - popupRect.height) / 2, 
                popupRect.width, popupRect.height);
            
            GUI.Box(centeredRect, "", EditorStyles.helpBox); // 회색 박스 스타일 추가
            GUILayout.BeginArea(centeredRect);

            GUILayout.Space(10);
            GUILayout.Label("Package Name:");
            newPackageName = GUILayout.TextField(newPackageName);

            GUILayout.Label("Version:");
            newPackageVersion = GUILayout.TextField(newPackageVersion);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                AddPackage(newPackageName, newPackageVersion);
                showPopup = false;
            }

            if (GUILayout.Button("Cancel"))
            {
                showPopup = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void AddPackage(string packageName, string version)
        {
            if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(version))
            {
                EditorUtility.DisplayDialog("Error", "입력된 값이 없습니다.", "OK");
                return;
            }

            if (PackageCenter.Manifest != null && PackageCenter.Manifest.dependencies != null)
            {
                var result = PackageCenter.Manifest.AddManifest(packageName, version);
                EditorUtility.DisplayDialog("패키지 추가하기", $"패키지 추가 동작이 수행되었습니다.\n결과 : {result.ToString()}", "확인");
            }
        }
    }
}
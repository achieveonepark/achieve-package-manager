using System.Collections.Generic;

namespace Achieve.Package.Manager
{
    internal class CuratedPackage
    {
        public string Id;          // manifest dependency key (e.g. com.cysharp.unitask)
        public string DisplayName;
        public string Description;
        public string GitUrl;      // value written to manifest dependency
        public string Repository;  // for "View on GitHub"
        public string Category;
    }

    internal static class CuratedPackages
    {
        internal static readonly IReadOnlyList<CuratedPackage> All = new List<CuratedPackage>
        {
            new CuratedPackage
            {
                Id = "com.cysharp.unitask",
                DisplayName = "UniTask",
                Description = "Provides an efficient allocation-free async/await integration for Unity.",
                GitUrl = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
                Repository = "https://github.com/Cysharp/UniTask",
                Category = "Async",
            },
            new CuratedPackage
            {
                Id = "com.github-glitchenzo.nugetforunity",
                DisplayName = "NuGet for Unity",
                Description = "A NuGet package manager for Unity Editor.",
                GitUrl = "https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity",
                Repository = "https://github.com/GlitchEnzo/NuGetForUnity",
                Category = "Tools",
            },
            new CuratedPackage
            {
                Id = "com.neuecc.unirx",
                DisplayName = "UniRx",
                Description = "Reactive Extensions for Unity.",
                GitUrl = "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts",
                Repository = "https://github.com/neuecc/UniRx",
                Category = "Async",
            },
            new CuratedPackage
            {
                Id = "com.cysharp.zstring",
                DisplayName = "ZString",
                Description = "Zero allocation StringBuilder for .NET and Unity.",
                GitUrl = "https://github.com/Cysharp/ZString.git?path=src/ZString.Unity/Assets/Scripts/ZString",
                Repository = "https://github.com/Cysharp/ZString",
                Category = "Performance",
            },
            new CuratedPackage
            {
                Id = "com.cysharp.messagepipe",
                DisplayName = "MessagePipe",
                Description = "High performance in-memory/distributed messaging pipeline for .NET and Unity.",
                GitUrl = "https://github.com/Cysharp/MessagePipe.git?path=src/MessagePipe.Unity/Assets/Plugins/MessagePipe",
                Repository = "https://github.com/Cysharp/MessagePipe",
                Category = "Messaging",
            },
            new CuratedPackage
            {
                Id = "com.cysharp.r3",
                DisplayName = "R3",
                Description = "The new future of dotnet/reactive and UniRx.",
                GitUrl = "https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity",
                Repository = "https://github.com/Cysharp/R3",
                Category = "Async",
            },
            new CuratedPackage
            {
                Id = "com.coffee.ui-effect",
                DisplayName = "UI Effect",
                Description = "UIEffect provides various visual effects for Unity UI.",
                GitUrl = "https://github.com/mob-sakai/UIEffect.git",
                Repository = "https://github.com/mob-sakai/UIEffect",
                Category = "UI",
            },
            new CuratedPackage
            {
                Id = "com.coffee.softmask-for-ugui",
                DisplayName = "Soft Mask for uGUI",
                Description = "Smooth masking component for Unity UI.",
                GitUrl = "https://github.com/mob-sakai/SoftMaskForUGUI.git",
                Repository = "https://github.com/mob-sakai/SoftMaskForUGUI",
                Category = "UI",
            },
            new CuratedPackage
            {
                Id = "com.demigiant.dotween",
                DisplayName = "DOTween (HOTween v2)",
                Description = "Fast, efficient, fully type-safe tween engine for Unity.",
                GitUrl = "https://github.com/Demigiant/dotween.git",
                Repository = "https://github.com/Demigiant/dotween",
                Category = "Animation",
            },
            new CuratedPackage
            {
                Id = "com.dbrizov.naughtyattributes",
                DisplayName = "NaughtyAttributes",
                Description = "Attribute extensions for Unity Inspector.",
                GitUrl = "https://github.com/dbrizov/NaughtyAttributes.git?path=/Assets/NaughtyAttributes",
                Repository = "https://github.com/dbrizov/NaughtyAttributes",
                Category = "Editor",
            },
            new CuratedPackage
            {
                Id = "com.unity.nuget.newtonsoft-json",
                DisplayName = "Newtonsoft Json",
                Description = "Json.NET wrapped as a Unity package.",
                GitUrl = "3.2.1",
                Repository = "https://github.com/jilleJr/Newtonsoft.Json-for-Unity",
                Category = "Serialization",
            },
            new CuratedPackage
            {
                Id = "com.yasirkula.unityingamedebugconsole",
                DisplayName = "In-Game Debug Console",
                Description = "Runtime debug console window for Unity.",
                GitUrl = "https://github.com/yasirkula/UnityIngameDebugConsole.git",
                Repository = "https://github.com/yasirkula/UnityIngameDebugConsole",
                Category = "Debug",
            },

            // ---- Dependency Injection ----
            new CuratedPackage
            {
                Id = "jp.hadashikick.vcontainer",
                DisplayName = "VContainer",
                Description = "Fast, lightweight DI container for Unity. Recommended modern DI.",
                GitUrl = "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer",
                Repository = "https://github.com/hadashiA/VContainer",
                Category = "Dependency Injection",
            },
            new CuratedPackage
            {
                Id = "com.svermeulen.extenject",
                DisplayName = "Extenject (Zenject)",
                Description = "Maintained fork of Zenject - a mature DI framework for Unity.",
                GitUrl = "https://github.com/Mathijs-Bakker/Extenject.git?path=UnityProject/Assets/Plugins/Zenject",
                Repository = "https://github.com/Mathijs-Bakker/Extenject",
                Category = "Dependency Injection",
            },

            // ---- Animation ----
            new CuratedPackage
            {
                Id = "com.kyrylokuzyk.primetween",
                DisplayName = "PrimeTween",
                Description = "Modern, allocation-free, type-safe tween library - a strong alternative to DOTween.",
                GitUrl = "https://github.com/KyryloKuzyk/PrimeTween.git?path=Assets/PrimeTween",
                Repository = "https://github.com/KyryloKuzyk/PrimeTween",
                Category = "Animation",
            },

            // ---- Serialization ----
            new CuratedPackage
            {
                Id = "com.cysharp.memorypack",
                DisplayName = "MemoryPack",
                Description = "Zero-encoding, extremely fast binary serializer for C# and Unity.",
                GitUrl = "https://github.com/Cysharp/MemoryPack.git?path=src/MemoryPack.Unity/Assets/Scripts/MemoryPack",
                Repository = "https://github.com/Cysharp/MemoryPack",
                Category = "Serialization",
            },
            new CuratedPackage
            {
                Id = "com.sirenix.odinserializer",
                DisplayName = "OdinSerializer",
                Description = "Open-source serializer behind Odin Inspector. Polymorphism, dictionaries, cycles, etc.",
                GitUrl = "https://github.com/TeamSirenix/odin-serializer.git",
                Repository = "https://github.com/TeamSirenix/odin-serializer",
                Category = "Serialization",
            },

            // ---- Logging ----
            new CuratedPackage
            {
                Id = "com.cysharp.zlogger",
                DisplayName = "ZLogger",
                Description = "Zero-allocation structured logger built on Microsoft.Extensions.Logging.",
                GitUrl = "https://github.com/Cysharp/ZLogger.git?path=src/ZLogger.Unity/Assets/Scripts/ZLogger",
                Repository = "https://github.com/Cysharp/ZLogger",
                Category = "Logging",
            },

            // ---- Networking ----
            new CuratedPackage
            {
                Id = "com.mirrornetworking.mirror",
                DisplayName = "Mirror",
                Description = "Open-source high-level multiplayer networking framework for Unity.",
                GitUrl = "https://github.com/MirrorNetworking/Mirror.git?path=/Assets/Mirror",
                Repository = "https://github.com/MirrorNetworking/Mirror",
                Category = "Networking",
            },

            // ---- Debug (more) ----
            new CuratedPackage
            {
                Id = "com.tayx.graphy",
                DisplayName = "Graphy",
                Description = "FPS / RAM / Audio / Advanced device stats overlay.",
                GitUrl = "https://github.com/Tayx94/graphy.git",
                Repository = "https://github.com/Tayx94/graphy",
                Category = "Debug",
            },
            new CuratedPackage
            {
                Id = "com.yasirkula.assetusagedetector",
                DisplayName = "Asset Usage Detector",
                Description = "Find references to assets, scripts, and scene objects in the project.",
                GitUrl = "https://github.com/yasirkula/UnityAssetUsageDetector.git",
                Repository = "https://github.com/yasirkula/UnityAssetUsageDetector",
                Category = "Debug",
            },
            new CuratedPackage
            {
                Id = "com.yasirkula.runtimeinspector",
                DisplayName = "Runtime Inspector & Hierarchy",
                Description = "Inspect and edit GameObjects at runtime.",
                GitUrl = "https://github.com/yasirkula/UnityRuntimeInspector.git",
                Repository = "https://github.com/yasirkula/UnityRuntimeInspector",
                Category = "Debug",
            },

            // ---- UI (more) ----
            new CuratedPackage
            {
                Id = "com.coffee.ui-particle",
                DisplayName = "Particle Effect for uGUI",
                Description = "Render particle effects inside the uGUI canvas with masking and sorting support.",
                GitUrl = "https://github.com/mob-sakai/ParticleEffectForUGUI.git",
                Repository = "https://github.com/mob-sakai/ParticleEffectForUGUI",
                Category = "UI",
            },
            new CuratedPackage
            {
                Id = "com.coffee.unmask-for-ugui",
                DisplayName = "Unmask for uGUI",
                Description = "Reverse mask for Unity UI (cutouts for tutorials, spotlights, etc.).",
                GitUrl = "https://github.com/mob-sakai/UnmaskForUGUI.git",
                Repository = "https://github.com/mob-sakai/UnmaskForUGUI",
                Category = "UI",
            },

            // ---- Node Editors / Visual Tools ----
            new CuratedPackage
            {
                Id = "com.github.siccity.xnode",
                DisplayName = "xNode",
                Description = "Lightweight node editor framework for building visual tools in the Unity editor.",
                GitUrl = "https://github.com/Siccity/xNode.git",
                Repository = "https://github.com/Siccity/xNode",
                Category = "Editor",
            },
        };
    }
}

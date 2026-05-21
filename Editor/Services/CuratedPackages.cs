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
        };
    }
}

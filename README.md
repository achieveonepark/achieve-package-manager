# Achieve Package Manager

🇰🇷 한국어 (현재) · [🇺🇸 English](./README.en.md)

Unity용 UI Toolkit 기반 패키지 매니저. OpenUPM 패키지, 큐레이션된 GitHub 유틸리티, 임의의 Git URL을 클릭 한 번으로 `Packages/manifest.json`에 바로 주입합니다.

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)
![License](https://img.shields.io/badge/license-MIT-blue)

## 주요 기능

- **UI Toolkit 윈도우** - 사이드바 카테고리, 검색, 상태바를 갖춘 깔끔한 UI
- **Installed** - 매니페스트의 모든 패키지를 소스 자동 감지(Registry / Git / Local / Built-in)와 함께 표시
- **OpenUPM** - OpenUPM 레지스트리 전체를 검색·열람하고, 스코프 자동 등록과 함께 설치
- **Featured (Git)** - 큐레이션된 유틸리티(UniTask, NuGetForUnity, VContainer, DOTween 등) 원클릭 설치
- **Custom Git URL** - `https://`, `git@`, `.git` URL을 붙여넣고 바로 설치
- 빌트인이 아닌 모든 패키지를 매니페스트에서 **Remove** 가능

## 설치

Unity 메뉴 `Window > Package Manager > + > Add package from git URL` 에서 다음 URL 입력:

```
https://github.com/achieveonepark/achieve-package-manager.git
```

또는 `Packages/manifest.json`에 직접 추가:

```json
{
  "dependencies": {
    "com.achieve.achieve-package-manager": "https://github.com/achieveonepark/achieve-package-manager.git"
  }
}
```

## 사용법

Unity 메뉴에서 윈도우 열기:

```
Tools > Achieve Package Manager
```

### Installed 탭

`Packages/manifest.json`에 있는 패키지와 그 의존성을 전부 보여줍니다. 각 행은 이름·버전·자동 감지된 소스를 표시하고, 빌트인이 아닌 항목엔 **Remove** 버튼이 붙어 있어요.

### OpenUPM 탭

OpenUPM 전체 패키지 목록(`https://package.openupm.com/-/all`)을 가져옵니다. 검색은 이름·ID·설명에 대해 필터링되고, **Install**을 누르면:

1. `package.openupm.com` 스코프 레지스트리를 추가(또는 병합)
2. 패키지 스코프를 자동 등록 (예: `com.cysharp.unitask` → `com.cysharp`)
3. `dependencies`에 패키지와 최신 버전을 기록
4. `Client.Resolve()` 호출

### Featured 탭

엄선된 Git 패키지가 카테고리별로 묶여 있습니다. 카드 한 번 클릭이면 매니페스트에 의존성이 작성됩니다. 각 카드에는 GitHub 링크도 함께 표시돼요.

현재 큐레이션 목록:

| 카테고리 | 패키지 |
|----------|--------|
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

### Custom Git URL 탭

임의의 Git URL을 붙여넣으면 됩니다. 패키지 이름은 URL에서 자동 추정(`com.git.<repo>`)되며 수동으로 덮어쓸 수도 있어요. 아래쪽엔 설치된 Git 패키지 리스트가 함께 보입니다.

## 동작 원리

- 매니페스트 읽기·쓰기는 `Editor/Dto/PackageCenter.cs`를 거치며, 알지 못하는 최상위 필드를 보존하고 JSON을 일관된 포맷으로 직렬화합니다.
- OpenUPM 탐색은 `UnityWebRequest`로 npm 호환 엔드포인트(`package.openupm.com`)를 호출합니다.
- 큐레이션 엔트리는 `Editor/Services/CuratedPackages.cs`에 있습니다. Featured 목록을 늘리거나 수정하려면 여기에 항목을 추가하세요.
- 모든 쓰기 작업 후엔 `Client.List(true)` + `Client.Resolve()`로 패키지 리스트를 새로 고칩니다.

## 요구사항

- Unity 2022.3 이상
- `com.unity.nuget.newtonsoft-json` (의존성으로 자동 추가)
- 네트워크 액세스 (OpenUPM 탐색 및 Git 설치용)

## 라이선스

MIT

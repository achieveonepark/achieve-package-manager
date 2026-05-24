# Changelog

Achieve Package Manager의 주요 변경 사항을 정리합니다.
형식은 [Keep a Changelog](https://keepachangelog.com/ko/1.1.0/)를 따르고, 버전은 [Semantic Versioning](https://semver.org/lang/ko/)을 따릅니다.

📖 문서: [https://somiri.dev/achieve-package-manager/](https://somiri.dev/achieve-package-manager/)

---

## 🎉 1.0.0 - 2026.05.24

UI Toolkit으로 패키지 매니저를 새로 설계한 첫 정식 릴리스.

### Added

- **UI Toolkit 윈도우** — `Tools > Achieve Package Manager`에서 열리는 단일 윈도우. 툴바 + 사이드바(카테고리) + 컨텐츠 + 상태바 레이아웃.
- **Installed 탭** — 매니페스트의 모든 패키지를 표시하고, 소스(Registry / Git / Local / Embedded / Built-in)를 자동 감지. 빌트인이 아닌 항목은 한 번에 제거 가능.
- **OpenUPM 탭** — OpenUPM 레지스트리 전체 카탈로그(`/-/all`)를 세션 단위 메모리 캐시로 1회만 다운로드. 검색·필터링 후 클릭 한 번에 설치 — 스코프 레지스트리 자동 추가/병합 + 패키지 스코프 자동 등록 + 최신 버전 기록.
- **Featured 탭** — 큐레이션된 Git 패키지를 카테고리별 카드로 표시, 클릭 한 번에 manifest 주입:
  - **Async**: UniTask, R3
  - **Animation**: DOTween, PrimeTween
  - **Performance**: ZString
  - **Messaging**: MessagePipe
  - **Serialization**: Newtonsoft Json, MemoryPack, OdinSerializer
  - **Logging**: ZLogger
  - **Dependency Injection**: VContainer, Extenject (Zenject)
  - **Networking**: Mirror
  - **UI**: UI Effect, Soft Mask, Particle Effect for uGUI, Unmask for uGUI
  - **Editor**: NaughtyAttributes, xNode
  - **Tools**: NuGet for Unity
  - **Debug**: In-Game Debug Console, Graphy, Asset Usage Detector, Runtime Inspector
- **Custom Git URL 탭** — 임의 Git URL(`https://`, `git@`, `?path=`, `#branch/tag` 모두 지원) 입력 후 1클릭 설치. 이름 자동 추정(`com.git.<repo>`) + 수동 오버라이드.
- **미해결(Unresolved) 항목 표시** — manifest에는 있지만 resolve 실패한 항목을 경고 배너 + Remove 버튼으로 노출. 도구 안에서 깨진 매니페스트를 복구할 수 있음.
- **UPM 이름 검증** — manifest 쓰기 전 `PackageCenter.IsValidPackageName`으로 검증. 잘못된 이름이 매니페스트 전체 resolve를 망가뜨리는 사고 차단.
- **검색 / 새로고침 / Open manifest.json** — 툴바 액션. 검색은 활성 탭의 리스트에 적용.
- **한국어/영어 가이드 사이트** — VitePress 기반, GitHub Actions로 GitHub Pages 자동 배포. `Documentation~/` 디렉터리로 Unity 임포트 대상에서 제외.

### Changed

- 기존 IMGUI(`OnGUI`) 기반 "Manifest Data Viewer"를 UI Toolkit 기반 윈도우로 전면 재작성.
- 메뉴 항목 이름을 `Tools > Manifest Data Viewer` → `Tools > Achieve Package Manager`로 변경.

### Fixed

- 매니페스트 직렬화 시 `dependencies`만 디스크에 쓰여 `scopedRegistries` 등 다른 최상위 필드가 손실되던 문제 수정. 알지 못하는 필드는 그대로 보존하도록 변경.
- `ToolbarSearchField` 사용 시 누락된 `UnityEditor.UIElements` using 디렉티브로 인한 `CS0246` 컴파일 에러 수정.
- 컨텐츠가 길어지면 툴바/상태바가 `flexShrink` 기본값(1) 때문에 찌부러지던 레이아웃 버그 수정. 툴바·상태바·사이드바에 `flexShrink = 0`, 컨텐츠 영역에 `overflow: Hidden` 적용.
- OpenUPM `/-/all`에 npm 포맷으로 섞여 들어오던 `@scope/name` 형식 이름이 manifest에 기록되어 전체 resolve가 막히던 사고 차단. 목록 단계에서 사전 필터링 + 쓰기 직전 검증.

### Dependencies

- `com.unity.test-framework` 1.0.16 (유지)
- `com.unity.nuget.newtonsoft-json` 3.2.1 (신규)
- `com.unity.serialization` 3.1.1 (신규)

### Requirements

- Unity **2022.3** 이상

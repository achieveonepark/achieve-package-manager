# 문제 해결

## `Project has invalid dependencies: <name> is invalid.`

원인: `manifest.json`에 UPM 패키지 이름 규칙을 위반하는 항목이 들어가 있어요. 한 줄만 깨져도 Unity는 **전체 resolve를 거부**하기 때문에, 그 뒤로는 무엇을 설치해도 같은 에러가 납니다.

**복구:**

1. **도구로 복구 (권장)** — 패키지 매니저가 그래도 로드된다면, `Tools > Achieve Package Manager > Installed` 탭에 노란 경고 배너와 함께 미해결 항목이 표시됩니다. 그 옆 `Remove` 버튼으로 즉시 제거하세요.
2. **수동 복구** — 도구가 안 뜨면 `<Project>/Packages/manifest.json`을 직접 열어 문제 줄을 삭제하고 저장하세요. 콤마 정리도 잊지 마세요.

설치 시점부터 잘못된 이름은 도구가 막아주므로 재발하지 않습니다.

## `error CS0246: 'ToolbarSearchField' could not be found`

`UnityEditor.UIElements` using이 빠졌을 때 발생합니다. 현재 코드엔 이미 들어가 있으므로, 구버전을 캐싱한 상태라면 패키지를 업데이트하세요.

## 툴바가 위쪽에 찌부러져 보임

`flexShrink` 기본값(1) 때문에 컨텐츠가 길어지면 툴바·상태바가 같이 눌렸던 버그예요. 1.0.0+에서 `flexShrink = 0`으로 고정해서 해결했습니다. 구버전을 쓰고 있다면 업데이트하세요.

## 패키지가 최신으로 안 바뀜

Git 의존성은 Unity가 자동으로 갱신하지 않아요.

* Package Manager에서 해당 패키지 선택 → **Update** 버튼
* 그래도 안 되면 `Library/PackageCache/com.achieve.achieve-package-manager@*` 폴더 삭제 후 Unity 재시작
* 또는 manifest에서 한 번 제거 → 다시 추가

## OpenUPM 목록이 안 뜨거나 비어 있음

* 네트워크가 막혀 있는지 확인 (`https://package.openupm.com/-/all` 접근 가능 여부)
* 회사망/방화벽 환경이라면 OpenUPM 도메인 허용 필요
* `↻ Refresh`로 재요청

응답이 수 MB라 처음 로딩에 몇 초 걸릴 수 있습니다.

## 매니페스트가 깔끔하게 정렬됨/포맷이 바뀜

도구가 매니페스트를 다시 쓸 때 dependencies와 scopes를 알파벳순으로 정렬해서 저장합니다. 이는 의도된 동작이며, manifest의 의미에는 영향이 없어요.

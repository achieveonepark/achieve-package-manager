---
sidebar_position: 3
---

# 설치

## 1. Unity Package Manager에서 Git URL로 추가 (권장)

Unity 메뉴 `Window > Package Manager` → 좌측 상단 **+** → **Add package from git URL...** 선택 후 다음 입력:

```
https://github.com/achieveonepark/achieve-package-manager.git
```

특정 버전을 고정하려면 태그를 붙입니다:

```
https://github.com/achieveonepark/achieve-package-manager.git#1.0.0
```

## 2. `manifest.json`에 직접 추가

`<Project>/Packages/manifest.json`을 열고 `dependencies`에 추가합니다.

```json
{
  "dependencies": {
    "com.achieve.achieve-package-manager": "https://github.com/achieveonepark/achieve-package-manager.git"
  }
}
```

저장하면 Unity가 자동으로 가져옵니다.

## 요구사항

* Unity **2022.3** 이상
* `com.unity.nuget.newtonsoft-json` (패키지 의존성으로 자동 설치)
* `com.unity.serialization` (패키지 의존성으로 자동 설치)
* 네트워크 액세스 (OpenUPM 카탈로그 조회, Git 다운로드용)

## 업데이트

이 패키지는 Git 의존성으로 설치되므로, Package Manager에서 **Update**를 누르거나 `Library/PackageCache`의 해당 폴더를 지운 뒤 다시 임포트하면 최신 커밋으로 갱신됩니다. 태그를 고정한 경우엔 manifest의 태그 부분만 새 버전으로 바꿔주세요.

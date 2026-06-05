# Custom Git URL 탭

큐레이션에 없는 패키지도 Git URL만 있으면 1클릭으로 설치할 수 있어요.

## 지원하는 URL 형식

UPM이 인식하는 모든 Git URL을 지원합니다.

* `https://github.com/user/repo.git`
* `git@github.com:user/repo.git` (SSH 키 설정 필요)
* `https://gitlab.com/user/repo.git`
* `https://bitbucket.org/user/repo.git`
* `?path=` 옵션으로 하위 폴더 지정  
  예: `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`
* `#branch` 또는 `#tag`로 버전 고정  
  예: `https://github.com/Cysharp/UniTask.git#2.5.0`

## 입력 필드

| 필드 | 설명 |
|------|------|
| Git URL | 필수. 위 형식의 URL |
| Package name (optional) | 선택. 입력 시 manifest의 키로 그대로 사용 |

## 패키지 이름 자동 추정

`Package name`을 비워두면, URL에서 마지막 세그먼트를 추출해 다음 규칙으로 만듭니다.

```
https://github.com/Cysharp/UniTask.git
→ com.git.unitask
```

이름은 항상 소문자로, `com.git.<repo>` 형식으로 생성돼요.

> 추정 이름이 깔끔하지 않거나 패키지의 실제 `package.json`이 다른 이름을 쓰는 경우엔, **수동으로 정확한 이름을 입력**하는 것이 안전합니다. 그래야 Unity가 같은 패키지의 중복 추가를 막을 수 있어요.

## 검증

도구는 manifest에 쓰기 직전 패키지 이름을 검증합니다.

* `a-z`, `0-9`, `-`, `_`, `.` 만 허용
* 첫 글자는 영문 소문자 또는 숫자
* 길이 ≤ 214

규칙 위반 시 manifest에 기록하지 않고 `Failed to install ...` 노티피케이션과 함께 Console에 에러를 남깁니다. (잘못된 이름 한 줄이 매니페스트 전체 resolve를 막을 수 있기 때문입니다.)

## 설치된 Git 패키지 리스트

입력 카드 아래쪽엔 현재 프로젝트에 설치된 **Git 소스 패키지 목록**이 별도 섹션으로 표시됩니다. Installed 탭과 동일한 행 포맷이며 `Remove` 버튼도 동일하게 동작합니다.

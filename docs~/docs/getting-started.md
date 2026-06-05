---
sidebar_position: 2
---

# 시작하기

## 윈도우 열기

Unity 메뉴에서:

```
Tools > Achieve Package Manager
```

## 화면 구성

```
┌─────────────────────────────────────────────────────────┐
│ Achieve Package Manager   ↻ Refresh  [검색]  Open manifest │  ← 툴바
├──────────┬──────────────────────────────────────────────┤
│ CATEGORIES                                              │
│ ▸ Installed     │  (선택된 탭의 컨텐츠)                 │
│ ▸ OpenUPM       │                                       │
│ ▸ Featured (Git)│                                       │
│ ▸ Custom Git URL│                                       │
├──────────┴──────────────────────────────────────────────┤
│ N packages installed                                    │  ← 상태바
└─────────────────────────────────────────────────────────┘
```

* **툴바**: 새로고침, 검색, 매니페스트 파일 위치 열기
* **사이드바**: 4개의 카테고리 탭
* **상태바**: 설치된 패키지 수 / 새로고침 상태

## 4개 탭 한눈에

| 탭 | 용도 |
|----|------|
| [Installed](guides/installed.md) | 현재 매니페스트에 있는 패키지 / 미해결 항목 보기·삭제 |
| [OpenUPM](guides/openupm.md) | OpenUPM 레지스트리 전체 탐색·설치 |
| [Featured (Git)](guides/featured.md) | 큐레이션된 Git 패키지 1클릭 설치 |
| [Custom Git URL](guides/git-url.md) | 임의 Git URL 입력으로 설치 |

## 검색 동작

툴바의 검색은 **현재 활성 탭의 리스트**에 적용됩니다.

* Installed: 이름·ID로 필터
* OpenUPM: 이름·ID·설명으로 필터
* Featured: 이름·ID·설명·카테고리로 필터
* Custom Git URL: 설치된 Git 패키지 리스트에 적용

## 새로고침

`↻ Refresh` 버튼은:
1. Unity 패키지 리스트(`Client.List`)를 다시 가져옴
2. OpenUPM 카탈로그를 강제 재다운로드

OpenUPM 카탈로그는 세션 내에서 자동 캐시되니, 평소엔 누를 일이 거의 없어요.

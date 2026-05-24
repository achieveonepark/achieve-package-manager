import { defineConfig } from 'vitepress'

const GITHUB_URL = 'https://github.com/achieveonepark/achieve-package-manager'

export default defineConfig({
  base: '/achieve-package-manager/',
  cleanUrls: true,
  lastUpdated: true,
  metaChunk: true,

  title: 'Achieve Package Manager',
  description: 'Unity UI Toolkit 기반 패키지 매니저 - OpenUPM, 큐레이션 Git 패키지, Custom Git URL을 한 곳에서.',

  head: [
    ['meta', { name: 'theme-color', content: '#5176ff' }],
  ],

  themeConfig: {
    socialLinks: [
      { icon: 'github', link: GITHUB_URL },
    ],
    search: {
      provider: 'local',
      options: {
        locales: {
          root: {
            translations: {
              button: { buttonText: '검색', buttonAriaLabel: '검색' },
              modal: {
                displayDetails: '상세 보기',
                resetButtonTitle: '검색어 지우기',
                backButtonTitle: '닫기',
                noResultsText: '검색 결과가 없습니다',
                footer: {
                  selectText: '선택',
                  navigateText: '이동',
                  closeText: '닫기',
                },
              },
            },
          },
        },
      },
    },
  },

  locales: {
    root: {
      label: '한국어',
      lang: 'ko-KR',
      themeConfig: {
        nav: [
          { text: '시작하기', link: '/getting-started' },
          { text: '가이드', link: '/guides/installed' },
          { text: '레퍼런스', link: '/reference/architecture' },
        ],
        sidebar: [
          {
            text: '시작',
            items: [
              { text: '소개', link: '/' },
              { text: '설치', link: '/installation' },
              { text: '시작하기', link: '/getting-started' },
            ],
          },
          {
            text: '가이드',
            items: [
              { text: 'Installed 탭', link: '/guides/installed' },
              { text: 'OpenUPM 탭', link: '/guides/openupm' },
              { text: 'Featured 탭', link: '/guides/featured' },
              { text: 'Custom Git URL 탭', link: '/guides/git-url' },
              { text: '문제 해결', link: '/guides/troubleshooting' },
            ],
          },
          {
            text: '레퍼런스',
            items: [
              { text: '아키텍처', link: '/reference/architecture' },
              { text: '큐레이션 목록 확장', link: '/reference/extending-curated-list' },
            ],
          },
        ],
        outline: { label: '이 페이지' },
        docFooter: { prev: '이전', next: '다음' },
        lastUpdatedText: '최근 업데이트',
        darkModeSwitchLabel: '테마',
        lightModeSwitchTitle: '라이트 모드',
        darkModeSwitchTitle: '다크 모드',
        sidebarMenuLabel: '메뉴',
        returnToTopLabel: '맨 위로',
        editLink: {
          pattern: `${GITHUB_URL}/edit/main/Documentation~/:path`,
          text: 'GitHub에서 이 페이지 수정',
        },
      },
    },

    en: {
      label: 'English',
      lang: 'en-US',
      link: '/en/',
      themeConfig: {
        nav: [
          { text: 'Get Started', link: '/en/getting-started' },
          { text: 'Guides', link: '/en/guides/installed' },
          { text: 'Reference', link: '/en/reference/architecture' },
        ],
        sidebar: [
          {
            text: 'Start',
            items: [
              { text: 'Introduction', link: '/en/' },
              { text: 'Installation', link: '/en/installation' },
              { text: 'Getting Started', link: '/en/getting-started' },
            ],
          },
          {
            text: 'Guides',
            items: [
              { text: 'Installed Tab', link: '/en/guides/installed' },
              { text: 'OpenUPM Tab', link: '/en/guides/openupm' },
              { text: 'Featured Tab', link: '/en/guides/featured' },
              { text: 'Custom Git URL Tab', link: '/en/guides/git-url' },
              { text: 'Troubleshooting', link: '/en/guides/troubleshooting' },
            ],
          },
          {
            text: 'Reference',
            items: [
              { text: 'Architecture', link: '/en/reference/architecture' },
              { text: 'Extending Curated List', link: '/en/reference/extending-curated-list' },
            ],
          },
        ],
        editLink: {
          pattern: `${GITHUB_URL}/edit/main/Documentation~/:path`,
          text: 'Edit this page on GitHub',
        },
      },
    },
  },
})

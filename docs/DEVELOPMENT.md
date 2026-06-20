# Just Alarm 빌드 · 개발하기

이 문서는 Just Alarm 프로젝트의 빌드 및 개발 환경 설정 방법을 안내합니다.

## 개발 환경 요구사항

* **운영체제**: Windows 10 이상 
* **개발 도구**: .NET 10 SDK 필요
* **IDE / 에디터**: [Visual Studio Code Insiders](https://visualstudio.com) 필요

 ## 빌드 방법

터미널(PowerShell)을 열고 아래 명령어를 입력하여 릴리즈 모드로 컴파일합니다.

```powershell
dotnet publish -c Release
```

### 실행 파일 경로
빌드가 성공적으로 완료되면 아래 경로에서 최종 실행 파일을 확인할 수 있습니다.

**`JustAlarm\bin\Release\net10.0-windows\win-x64\publish\JustAlarm.exe`**

## 설치파일(Installer) 제작 방법

배포용 인스톨러(Setup 패키지)를 생성하려면 아래 단계를 따르세요.

1. [Inno Setup](https://jrsoftware.org/isdl.php/Inno-Setup-Downloads) 프로그램을 컴퓨터에 다운로드 및 설치합니다.

2. 프로젝트 루트 폴더에 있는 `JustAlarm-Setup.iss` 파일을 엽니다.

3. Inno Setup에서 **Compile** 버튼 혹은 **Shift + F9**을 눌러 스크립트를 실행합니다.

 ## 프로젝트 구조

```
 JustAlarm/
├── JustAlarm.csproj # 프로젝트 설정 파일
├── App.xaml # 애플리케이션 진입점 UI
├── App.xaml.cs # 애플리케이션 초기화 로직
├── MainWindow.xaml # 주 창 UI
├── MainWindow.xaml.cs # 주 창 로직
├── icon.ico # 앱 아이콘
└── AssemblyInfo.cs # 어셈블리 메타정보
├── JustAlarm-Setup.iss # Inno Setup 설치파일 생성 스크립트
├── README.md # 프로젝트 설명서
└── .gitignore # Git 무시 파일 목록
```

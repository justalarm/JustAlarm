# JustAlarm 빌드, 개발하기

이 문서는 Just Alarm을 로컬에서 실행하거나 수정할 때 필요한 기본 정보를 정리합니다.

## 빌드하기

**.NET 10 SDK** 필요

_PowerShell_
```powershell
dotnet publish -c Release
```

빌드 후 `JustAlarm\bin\Release\net10.0-windows\win-x64\publish\JustAlarm.exe` 실행

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

## 설치파일 만들기

[Inno Setup](https://jrsoftware.org/isdl.php/Inno-Setup-Downloads) 설치 후 JustAlarm-Setup.iss 컴파일

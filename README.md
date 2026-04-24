SmokeTestSuite
    An automated smoke testing framework built with Selenium WebDriver and NUnit, targeting .NET 10. Designed to provide rapid, high-confidence validation of critical application paths after deployments or releases.

Architecture
  - UML Diagram (use draw.io to view): Class Structure and component relationships.
  - Data Flow Diagram (on Page 2 fo the UML Diagram and Data Flow Diagram document, use draw.io to view): shows end-to-end data flow through the test pipeline.

Tech Stack
    Dependency                          Version                    Purpose
     .NET                                 10.0                       Target framework
     NUnit                                4.5.1                      Test framework
     NUnit3TestAdapter                    6.1.0                      VS / CLI test runner integration
     NUnit.Analyzers                      4.12.0                     Static analysis for NUnit usage
     Selenium.WebDriver                   4.41.0                     Browser automation
     Selenium.Support                     4.41.0                     WebDriver helper utilities
     DotNetSeleniumExtras.WaitHelpers     3.11.0                     Explicit wait conditions
     ExtentReports                        5.0.4                      HTML test reporting
     Microsoft.Extensions.Configuration   10.0.5                     JSON & environment variable config
     coverlet.collector                   8.0.1                      Code coverage collection
     Microsoft.NET.Test.Sdk               18.3.0                     MSBuild test infrastructure

Prerequisites
    .NET 10 SDK
    A compatible WebDriver binary in your PATH (e.g. ChromeDriver, GeckoDriver) matching your installed browser version
    Git

Getting Started
    1. Clone the repository
    bashgit clone https://github.com/<your-org>/SmokeTestSuite.git
    cd SmokeTestSuite
    2. Configure the test environment
    Copy or edit appsettings.json to point at your target environment:
    json{
      "BaseUrl": "https://your-app-url.com",
      "Browser": "Chrome",
      "ImplicitWaitSeconds": 10
    }
    Environment variables override appsettings.json values at runtime, making it easy to inject configuration in CI without modifying source files.
    3. Restore dependencies
    bashdotnet restore
    4. Build
    bashdotnet build --configuration Release
    5. Run the smoke tests
    bashdotnet test --configuration Release

Configuration
    Configuration is loaded in priority order:
    
    Environment variables (highest priority — ideal for CI/CD)
    appsettings.json (committed base config)
    
    appsettings.json is automatically copied to the output directory on every build.

Test Reports
    This suite uses ExtentReports to generate a rich HTML report after each test run. The report is written to the output directory and includes:
    
    Pass / fail / skip summary
    Per-test execution timeline
    Step-level logging and screenshots on failure


CI/CD
    A GitHub Actions workflow is included at .github/workflows/smoke-tests.yml. It triggers automatically on push and pull request events, running the full suite against the configured target environment.
    To configure secrets (e.g. BASE_URL, credentials), add them in your repository under Settings → Secrets and variables → Actions.

Project Structure
    SmokeTestSuite/
    ├── .github/
    │   └── workflows/
    │       └── smoke-tests.yml       # GitHub Actions CI workflow
    ├── docs/
    │   ├── uml-diagram.png           # UML class diagram
    │   └── data-flow-diagram.png     # Data flow diagram
    ├── appsettings.json              # Base environment configuration
    ├── SmokeTestSuite.csproj         # Project file
    └── SmokeTestSuite.slnx           # Solution file

License
    This project is licensed under the MIT License

# ENSEK API Automation Test Suite

This project provides automated API tests for the ENSEK energy order system using .NET 8, NUnit, and Reqnroll (SpecFlow-compatible BDD).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- Internet access to restore NuGet packages

## Project Structure

- `ENSEK_Test/` - Test project (feature files, step definitions, test runners)
- `EnseK_API_Automation/` - API client and supporting code

## Setup

1. **Clone the repository** and open the solution in Visual Stu# ENSEK Automation Candidate Test

This project is a test automation or development assignment based on the ENSEK Automation Candidate Test platform.

## 🚀 Getting Started

Follow the instructions below to get the project up and running in your local development environment.

### Prerequisites

- Visual Studio 2022
- .NET 6.0 SDK or higher
- Git
- (Optional) SQL Server if the project includes a database

### 🔧 Setup Instructions

1. **Clone the repository**

   Open a terminal and run:

   ```bash
   git clone
2. **Restore NuGet packages**:  
   Visual Studio will restore packages automatically, or run:

   
#### Run tests by category and generate TRX report

To run only tests with the `feature` category and generate a TRX report:
   ```bash
   dotnet test --filter "Category=feature" --logger "trx;LogFileName=TestResults.trx"
   ```

To run only tests with Nunit  test calss test method  and generate a TRX report:	
   ```bash
   dotnet test --filter "FullyQualifiedName~ENSEK_Test.UpdateOrderTest.VerifyUserCanUpdateOrder" --logger "trx;LogFileName=TestResults.trx
   ```

To Generate HTML report from TRX file, you can use ReportUnit or any other tool that supports TRX to HTML conversion.
   ```bash
   reportunit TestResults.trx TestResults.html
```
```bash
   livingdoc test-assembly --test-results ./TestResults/test_results.trx --output ./livingdoc.html
   ```
Structure
```bash
EnseK_API_Automation/
├── APIClient.cs
├── Endpoints.cs
├── IEnsekClientAPI.cs
├── Auth/
│   └── ApiAuthenticator.cs
├── Config/
│   ├── ApiCredentials.cs
│   ├── ApiEnvironment.cs
│   └── EnvironmentConfig.cs
├── Helpers/
│   ├── CommonHelper.cs
│   └── OrderHelper.cs
├── Models/
│   ├── ExpectedOrder.cs
│   ├── Request/
│   │   ├── LoginResource.cs
│   │   └── OrderResource.cs
│   └── Response/
│       ├── BuyProductResponse.cs
│       ├── EnergyInventory.cs
│       ├── Order.cs
│       ├── OrderList.cs
│       └── TokenResponse.cs
```
```bash
ENSEK_Test/
├── ENSEK_Test.csproj
├── reqnroll.json
├── reporter/
│   └── ExtedndReporter.cs
├── features/
│   ├── BuyProduct.feature
│   ├── DeleteOrder.feature
│   ├── GetOrder.feature
│   ├── Reset.feature
│   ├── UpdateOrder.feature
│   ├── BuyProduct.feature.cs
│   ├── DeleteOrder.feature.cs
│   ├── GetOrder.feature.cs
│   ├── Reset.feature.cs
│   └── UpdateOrder.feature.cs
├── steps/
│   ├── BuyProductSteps.cs
│   ├── CommonSteps.cs
│   ├── DeleteOrderSteps.cs
│   ├── GetOrderSteps.cs
│   ├── GlobalHooks.cs
│   ├── ResetSteps.cs
│   └── UpdateOrderSteps.cs
├── BuyProductTests.cs
├── DeleteOrderTest.cs
├── GetOrdersTest.cs
├── ResetTest.cs
└── UpdateOrderTest.cs
```



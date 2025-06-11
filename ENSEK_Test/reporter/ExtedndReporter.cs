
//using AventStack.ExtentReports;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;




//namespace ENSEK_Test.reporter
//{

//    public class ExtedndReporter
//    {

//        private static ExtentReports _extent;
//        private static ExtentReports _htmlReporter;
//        private static ExtentTest _currentTest;

//        // Initialize the reporter and configure the output HTML file path
//        public static void Init(string reportPath = "TestReport.html")
//        {
//            _htmlReporter = new extenhtml(reportPath);
//            _htmlReporter.Config.DocumentTitle = "ENSEK API Test Report";
//            _htmlReporter.Config.ReportName = "ENSEK API Automation Results";
//            _htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

//            _extent = new ExtentReports();
//            _extent.AttachReporter(_htmlReporter);

//            // You can add system info if needed:
//            _extent.AddSystemInfo("Environment", Environment.GetEnvironmentVariable("ENSEK_ENV") ?? "QA");
//            _extent.AddSystemInfo("User", Environment.UserName);
//        }

//        // Create a new test case in the report
//        public static void CreateTest(string testName)
//        {
//            _currentTest = _extent.CreateTest(testName);
//        }

//        // Log pass status
//        public static void Pass(string message)
//        {
//            _currentTest?.Pass(message);
//        }

//        // Log fail status
//        public static void Fail(string message)
//        {
//            _currentTest?.Fail(message);
//        }

//        // Log info message
//        public static void Info(string message)
//        {
//            _currentTest?.Info(message);
//        }

//        // Flush the report (write to disk)
//        public static void Flush()
//        {
//            _extent?.Flush();
//        }
//    }

//}

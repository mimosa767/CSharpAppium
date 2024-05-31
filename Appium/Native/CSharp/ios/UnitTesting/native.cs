using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Support.UI;
using Reportium.Client;
using Reportium.Model;
using Reportium.Test;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

[assembly: Parallelize(Workers = 3, Scope = ExecutionScope.MethodLevel)]
namespace IOS
{
    [TestClass]
    public class IOSTest
    {
        public TestContext TestContext { get; set; }
        IOSDriver<IOSElement> driver = null;
        ReportiumClient reportiumClient = null;

        [TestInitialize]
        public void BeforeTest()
        {
            AppiumOptions capabilities = new AppiumOptions();
            // 1. Replace <<cloud name>> with your perfecto cloud name (e.g. demo is the cloudName of demo.perfectomobile.com).
            String cloudName = "demo";

            // 2. Replace <<security token>> with your perfecto security token.
            String securityToken = "eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI4YmI4YmZmZS1kMzBjLTQ2MjctYmMxMS0zNTYyMmY1ZDkyMGYifQ.eyJpYXQiOjE3MDI0NzI1OTgsImp0aSI6ImE0NmNhYWI4LTI3MmYtNGU1MS1hYzU5LTdhZGI4NzAwNWYzNiIsImlzcyI6Imh0dHBzOi8vYXV0aC5wZXJmZWN0b21vYmlsZS5jb20vYXV0aC9yZWFsbXMvZGVtby1wZXJmZWN0b21vYmlsZS1jb20iLCJhdWQiOiJodHRwczovL2F1dGgucGVyZmVjdG9tb2JpbGUuY29tL2F1dGgvcmVhbG1zL2RlbW8tcGVyZmVjdG9tb2JpbGUtY29tIiwic3ViIjoiNjEwM2FhZjktOTdkNC00YjgwLThmZTYtZDNhYmRlNTJhM2JiIiwidHlwIjoiT2ZmbGluZSIsImF6cCI6Im9mZmxpbmUtdG9rZW4tZ2VuZXJhdG9yIiwibm9uY2UiOiJmZmVmNWIxMC01OTI4LTQzNWUtOGIzZC1hNDcxNGQwODA0N2IiLCJzZXNzaW9uX3N0YXRlIjoiMDQ5NjdhZTMtNzQ0Yi00ZDA5LWFiZTgtNDhlNDFiMWIzNjM0Iiwic2NvcGUiOiJvcGVuaWQgb2ZmbGluZV9hY2Nlc3MifQ.Xmzn0wU0tt9zV-vuzSUYrSCQ-B-4v6J7m3VLTiriaBg";
            capabilities.AddAdditionalCapability("securityToken", securityToken);

            // 3. Set device capabilities.
            capabilities.PlatformName = "iOS";
            capabilities.AddAdditionalCapability("deviceName", "00008120-001E71991EB8201E");

            // 4. Set Perfecto Media repository path of App under test.
            capabilities.AddAdditionalCapability("app", "PUBLIC:ExpenseTracker/Native/InvoiceApp1.0.ipa");


            // 5. Set the unique identifier of your app
            capabilities.AddAdditionalCapability("appPackage", "io.perfecto.expense.tracker");

            // Set other capabilities.
            capabilities.AddAdditionalCapability("enableAppiumBehavior", true);
            capabilities.AddAdditionalCapability("autoLaunch", true); // Whether to install and launch the app automatically.
            capabilities.AddAdditionalCapability("iOSResign", true); // Resign with developer certificate
            // capabilities.AddAdditionalCapability("fullReset", false); // Reset app state by uninstalling app.
            capabilities.AddAdditionalCapability("takesScreenshot", false);
            capabilities.AddAdditionalCapability("screenshotOnError", true);
            capabilities.AddAdditionalCapability("openDeviceTimeout", 5);
            // capabilities.AddAdditionalCapability("model", "iPhone.*");

            // Initialize the Appium driver
            driver = new IOSDriver<IOSElement>(
                    new Uri(string.Format("https://{0}.perfectomobile.com/nexperience/perfectomobile/wd/hub/fast", cloudName)), capabilities);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            PerfectoExecutionContext perfectoExecutionContext;
            if (System.Environment.GetEnvironmentVariable("JOB_NAME") != null)
            {
                perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                        .WithProject(new Project("My Project", "1.0"))
                        .WithJob(new Job(System.Environment.GetEnvironmentVariable("JOB_NAME"),
                                int.Parse(System.Environment.GetEnvironmentVariable("BUILD_NUMBER"))))
                        .WithWebDriver(driver).Build();
            }
            else
            {
                perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                        .WithProject(new Project("My Project", "1.0"))
                        .WithWebDriver(driver).Build();
            }
            reportiumClient = PerfectoClientFactory.CreatePerfectoReportiumClient(perfectoExecutionContext);

            reportiumClient.TestStart(TestContext.TestName, new Reportium.Test.TestContext("native", "ios"));
        }


        [TestMethod]
        [Description("Native C# iOS Sample1")]
        public void CSharpIOSNativeTest1()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                reportiumClient.StepStart("Enter email");
                IOSElement email = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_email"))));
                email.SendKeys("test@perfecto.com");

                reportiumClient.StepStart("Enter password");
                IOSElement password = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_password"))));
                password.SendKeys("test123");
                driver.HideKeyboard();

                reportiumClient.StepStart("Click login");
                driver.HideKeyboard();
                IOSElement login = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_login_btn"))));
                login.Click();

                reportiumClient.StepStart("Add expense");
                IOSElement add = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("list_add_btn"))));
                add.Click();

                reportiumClient.StepStart("Select head");
                IOSElement head = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("edit_head"))));
                head.Click();

                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//*[@value='- Select -']")))[0].SendKeys("Flight");

                reportiumClient.StepStart("Enter amount");
                IOSElement amount = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("edit_amount"))));
                amount.SendKeys("100");

                reportiumClient.StepStart("Save expense");
                IOSElement save = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("add_save_btn"))));
                save.Click();

                reportiumClient.StepStart("Verify alert");
                String expectedText = "Please enter valid category";
                Boolean res = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name='" + expectedText + "']"))).Displayed;
                reportiumClient.ReportiumAssert("Alert text validation.", res);
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateSuccess());
            }
            catch (Exception e)
            {
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateFailure(e));
            }

            ////Close connection and ends the test
            driver.Quit();
            Console.WriteLine("C# IOS execution completed");
        }

        [TestMethod]
        [Description("Native C# iOS Sample2")]
        public void CSharpIOSNativeTest2()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

                reportiumClient.StepStart("Enter email");
                IOSElement email = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_email"))));
                email.SendKeys("test@perfecto.com");

                reportiumClient.StepStart("Enter password");
                IOSElement password = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_password"))));
                password.SendKeys("test123");
                driver.HideKeyboard();

                reportiumClient.StepStart("Click login");
                IOSElement login = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("login_login_btn"))));
                login.Click();

                reportiumClient.StepStart("Add expense");
                IOSElement add = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("list_add_btn"))));
                add.Click();

                reportiumClient.StepStart("Select head");
                IOSElement head = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("edit_head"))));
                head.Click();

                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//*[@value='- Select -']")))[0].SendKeys("Flight");

                reportiumClient.StepStart("Enter amount");
                IOSElement amount = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("edit_amount"))));
                amount.SendKeys("100");

                reportiumClient.StepStart("Save expense");
                IOSElement save = (IOSElement)wait.Until(ExpectedConditions.ElementIsVisible((By.Name("add_save_btn"))));
                save.Click();

                reportiumClient.StepStart("Verify alert");
                String expectedText = "Please enter valid category";
                Boolean res = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@name='" + expectedText + "']"))).Displayed;
                reportiumClient.ReportiumAssert("Alert text validation.", res);
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateSuccess());
            }
            catch (Exception e)
            {
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateFailure(e));
            }

            ////Close connection and ends the test
            driver.Quit();
            Console.WriteLine("C# IOS execution completed");
        }

    }
}

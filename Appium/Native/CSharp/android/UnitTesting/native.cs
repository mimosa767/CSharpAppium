using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using Reportium.Client;
using Reportium.Model;

[assembly: Parallelize(Workers = 3, Scope = ExecutionScope.MethodLevel)]
namespace Android
{
    [TestClass]
    public class AndroidTest
    {
        public TestContext TestContext { get; set; }
        AndroidDriver<AndroidElement> driver = null;
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
            capabilities.PlatformName = "Android";
            // capabilities.AddAdditionalCapability("model", "Galaxy S.*|LG.*");
            capabilities.AddAdditionalCapability("deviceName", "1C241FDEE002D2");

            // 4. Set Perfecto Media repository path of App under test.
            capabilities.AddAdditionalCapability("app", "PUBLIC:ExpenseAppVer1.0.apk");

            // 5. Set the unique identifier of your app
            capabilities.AddAdditionalCapability("appPackage", "io.perfecto.expense.tracker");

            // Set other capabilities.
            capabilities.AddAdditionalCapability("enableAppiumBehavior", true); // Mandatory
            capabilities.AddAdditionalCapability("autoLaunch", true); // Whether to install and launch the app automatically.
            capabilities.AddAdditionalCapability("autoInstrument", true); // instruments the app before installation
            // capabilities.AddAdditionalCapability("fullReset", false); // Reset app state by uninstalling app.
            capabilities.AddAdditionalCapability("takesScreenshot", false);
            capabilities.AddAdditionalCapability("screenshotOnError", true);
            capabilities.AddAdditionalCapability("openDeviceTimeout", 5);
           // capabilities.AddAdditionalCapability("model", "Galaxy S.*");

            // Initialize the Appium driver
            driver = new AndroidDriver<AndroidElement>(
                   new Uri(string.Format("https://{0}.perfectomobile.com/nexperience/perfectomobile/wd/hub/fast", cloudName)), capabilities);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            PerfectoExecutionContext perfectoExecutionContext;
            if (System.Environment.GetEnvironmentVariable("jobName") != null)
            {
                perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                        .WithProject(new Project("My Project", "1.0"))
                        .WithJob(new Job(System.Environment.GetEnvironmentVariable("jobName"),
                                int.Parse(System.Environment.GetEnvironmentVariable("jobNumber"))))
                        .WithWebDriver(driver).Build();
            }
            else
            {
                perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                        .WithProject(new Project("My Project", "1.0"))
                        .WithWebDriver(driver).Build();
            }
            reportiumClient = PerfectoClientFactory.CreatePerfectoReportiumClient(perfectoExecutionContext);

            reportiumClient.TestStart(TestContext.TestName, new Reportium.Test.TestContext("native", "android"));
        }

        [TestMethod]
        [Description("Native C# Android Sample1")]
        public void PerfectoCSharpAndroidNativeTest1()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                reportiumClient.StepStart("Enter email");
                IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_email")));
                email.SendKeys("test@perfecto.com");

                reportiumClient.StepStart("Enter password");
                IWebElement password = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_password")));
                password.SendKeys("test123");

                reportiumClient.StepStart("Click login");
                driver.HideKeyboard();
                IWebElement login = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_login_btn")));
                login.Click();

                reportiumClient.StepStart("Add expense");
                IWebElement add = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/list_add_btn")));
                add.Click();

                reportiumClient.StepStart("Select head");
                IWebElement head = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/input_layout_head")));
                head.Click();

                IWebElement flight = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@text='Flight']")));
                flight.Click();

                reportiumClient.StepStart("Enter amount");
                IWebElement amount = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/add_amount")));
                amount.SendKeys("100");

                reportiumClient.StepStart("Save expense");
                IWebElement save = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/layout_buttons")));
                save.Click();

                reportiumClient.StepStart("Verify alert");
                String expectedText = "Select Currency";
                bool res = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@text='" + expectedText + "']"))).Displayed;
                reportiumClient.ReportiumAssert("Alert text validation.", res);

                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateSuccess());
            }
            catch (Exception e)
            {
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateFailure(e));
            }

            //Close connection and ends the test
            driver.Quit();
            Console.WriteLine("C# Android execution completed");
        }

        [TestMethod]
        [Description("Native C# Android Sample2")]
        public void PerfectoCSharpAndroidNativeTest2()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                reportiumClient.StepStart("Enter email");
                IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_email")));
                email.SendKeys("test@perfecto.com");

                reportiumClient.StepStart("Enter password");
                IWebElement password = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_password")));
                password.SendKeys("test123");

                reportiumClient.StepStart("Click login");
                IWebElement login = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/login_login_btn")));
                login.Click();

                reportiumClient.StepStart("Add expense");
                IWebElement add = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/list_add_btn")));
                add.Click();

                reportiumClient.StepStart("Select head");
                IWebElement head = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/input_layout_head")));
                head.Click();

                IWebElement flight = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@text='Flight']")));
                flight.Click();

                reportiumClient.StepStart("Enter amount");
                IWebElement amount = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/add_amount")));
                amount.SendKeys("100");

                reportiumClient.StepStart("Save expense");
                IWebElement save = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("io.perfecto.expense.tracker:id/layout_buttons")));
                save.Click();

                reportiumClient.StepStart("Verify alert");
                String expectedText = "Select Currency";
                bool res = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@text='" + expectedText + "']"))).Displayed;
                reportiumClient.ReportiumAssert("Alert text validation.", res);

                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateSuccess());
            }
            catch (Exception e)
            {
                reportiumClient.TestStop(Reportium.Test.Result.TestResultFactory.CreateFailure(e));
            }

            //Close connection and ends the test
            driver.Quit();
            Console.WriteLine("C# Android execution completed");
        }
    }
}

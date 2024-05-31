package web;

import java.net.URL;
import java.util.concurrent.TimeUnit;
import org.openqa.selenium.Keys;
import org.openqa.selenium.By;
import org.openqa.selenium.Platform;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import com.perfecto.reportium.client.ReportiumClient;
import com.perfecto.reportium.client.ReportiumClientFactory;
import com.perfecto.reportium.model.Job;
import com.perfecto.reportium.model.PerfectoExecutionContext;
import com.perfecto.reportium.model.Project;
import com.perfecto.reportium.test.TestContext;
import com.perfecto.reportium.test.result.TestResultFactory;
import org.openqa.selenium.Point;

public class Sample {

	public static void main(String[] args) throws Exception {


		// System.setProperty("http.proxyHost", "127.0.0.1");
		// System.setProperty("http.proxyPort", "8081");
		// System.setProperty("https.proxyHost", "127.0.0.1");
		// System.setProperty("https.proxyPort", "8081");
		DesiredCapabilities capabilities = new DesiredCapabilities("", "", Platform.ANY);

		// 1. Replace <<cloud name>> with your perfecto cloud name (e.g. demo is the cloudName of demo.perfectomobile.com).
		String cloudName = "<<cloud name>>";

		// 2. Replace <<security token>> with your perfecto security token.
		String securityToken = "<<security token>>";
		capabilities.setCapability("securityToken", securityToken);

		// 3. Set web capabilities.
		capabilities.setCapability("platformName", "Windows");
		capabilities.setCapability("platformVersion", "10");
		capabilities.setCapability("browserName", "Chrome");
		capabilities.setCapability("browserVersion", "latest");
		capabilities.setCapability("location", "US East");
		capabilities.setCapability("resolution", "1024x768");
        	
		// Set other capabilities.
		capabilities.setCapability("takesScreenshot", false);
		capabilities.setCapability("screenshotOnError", true);

		// Initialize the  driver
		RemoteWebDriver driver = new RemoteWebDriver(
				new URL("https://" + cloudName.replace(".perfectomobile.com", "")
				+ ".perfectomobile.com/nexperience/perfectomobile/wd/hub"),
				capabilities);

		// Setting implicit wait
		driver.manage().timeouts().implicitlyWait(5, TimeUnit.SECONDS);

		PerfectoExecutionContext perfectoExecutionContext;
		if (System.getProperty("jobName") != null) {
			perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
					.withProject(new Project("My Project", "1.0"))
					.withJob(new Job(System.getProperty("jobName"),
							Integer.parseInt(System.getProperty("jobNumber"))))
					.withWebDriver(driver).build();
		} else {
			perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
					.withProject(new Project("My Project", "1.0"))
					.withWebDriver(driver).build();
		}
		ReportiumClient reportiumClient = new ReportiumClientFactory()
				.createPerfectoReportiumClient(perfectoExecutionContext);

		reportiumClient.testStart("Selenium Java Web Sample", new TestContext("selenium", "web"));

		try {
			WebDriverWait wait = new WebDriverWait(driver, 30);
			String search = "perfectomobile";
			reportiumClient.stepStart("Navigate to Google");
			driver.get("https://www.google.com");
			reportiumClient.stepEnd();

			reportiumClient.stepStart("Search for " + search);
			WebElement searchbox = wait.until(ExpectedConditions.elementToBeClickable(driver.findElement(By.xpath("//*[@name='q']"))));
			searchbox.sendKeys(search);
			searchbox.sendKeys(Keys.RETURN);
			reportiumClient.stepEnd();

			reportiumClient.stepStart("Verify Title");
			String expectedText = "perfectomobile - Google Search";
			// Adding assertions to the Execution Report. This method will not fail the test
			reportiumClient.reportiumAssert(expectedText, driver.getTitle().contains(expectedText));
			reportiumClient.stepEnd();

			reportiumClient.testStop(TestResultFactory.createSuccess());
		} catch (Exception e) {
			reportiumClient.testStop(TestResultFactory.createFailure(e));
		}

		// Prints the Smart Reporting URL
		String reportURL = reportiumClient.getReportUrl();
		System.out.println("Report url - " + reportURL);

		// Quits the driver
		driver.quit();
	}
}

<<<<<<< HEAD
Commands:

Allure
Open cmd on {solutionPath.bin.debug.net6}
To clean results>:
allure generate allure-results --clean -o allure-report
to run browser>:
allure serve allure-results 

Api test:
dotnet test --filter "Category=Level_1"



Mobile:
Open any app in your mobile once it with adb and run>:
adb shell dumpsys window | find "mCurrentFocus"
s
Install uiautomator2
adb uninstall io.appium.uiautomator2.server
adb uninstall io.appium.uiautomator2.server.test

=======
Automation API for Web and Mobile Testing
Overview
This Automation API is designed to facilitate automated testing for web and mobile applications using Selenium for web testing and Appium for mobile testing. The API is designed to be infrastructure agnostic, making it easy to adapt to various systems.

Features
Web Testing: Leverage Selenium for automated web testing.
Mobile Testing: Utilize Appium for mobile application testing.
Parallel Execution: Execute tests in parallel for load testing and faster execution.
Easy Customization: Adapt the API to work with different systems easily.
Prerequisites
Ensure the following software is installed before using the Automation API:

Selenium WebDriver
Appium
[Your Preferred Testing Framework] (e.g., NUnit, MSTest, JUnit)
Getting Started:
git clone https://github.com


Customization
The API is designed for easy customization. Modify the test scripts, configuration files, and helper classes to adapt the API to your specific requirements and testing infrastructure.

Contributing
Contributions are welcome! Feel free to submit issues, feature requests, or pull requests to enhance the Automation API.
>>>>>>> 0715927a1426d88c92e8d607c469d2bf95435c39


Alphav vantage seconed Token 
9NLEOCDH0YUKCLTX
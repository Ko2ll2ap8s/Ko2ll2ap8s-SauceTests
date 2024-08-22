using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SauceDemoTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            Assert.IsTrue(driver.Url.Contains("inventory.html"));

            driver.Quit();
        }

        [TestMethod]
        public void TestAddItemToCart()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            // Авторизация
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            // Добавление товара в корзину
            driver.FindElement(By.ClassName("inventory_item")).FindElement(By.ClassName("btn_inventory")).Click();

            // Проверка, что товар добавлен в корзину
            Assert.IsTrue(driver.FindElement(By.ClassName("shopping_cart_badge")).Text.Equals("1"));

            driver.Quit();
        }

        [TestMethod]
        public void TestCheckout()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");

            // Авторизация
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            // Добавление товара в корзину
            driver.FindElement(By.ClassName("inventory_item")).FindElement(By.ClassName("btn_inventory")).Click();

            // Ввод данных и завершение заказа
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            driver.FindElement(By.Id("checkout")).Click();

            // Ââîä äàííûõ è çàâåðøåíèå çàêàçà
            driver.FindElement(By.Id("first-name")).SendKeys("John");
            driver.FindElement(By.Id("last-name")).SendKeys("Doe");
            driver.FindElement(By.Id("postal-code")).SendKeys("12345");
            driver.FindElement(By.Id("continue")).Click();
            driver.FindElement(By.Id("finish")).Click();

            // Явное ожидание появления элемента с текстом благодарности
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement completeHeader = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("complete-header")));

            // Диагностика текста
            string completeHeaderText = completeHeader.Text;
            Console.WriteLine("Actual complete-header text: " + completeHeaderText);

            // Проверка текста на странице
            Assert.IsTrue(completeHeaderText.Equals("Thank you for your order!"));

            driver.Quit();
        }

    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using Xunit.Abstractions;


namespace Portfel.TestySelenium
{
    public class RejestracjaTest
    {
        IWebDriver driver = new ChromeDriver();

        //czeka .... sekund na za³¹dowanie siê strony
        //driver.Manage().Timeouts().PageLoad = System.TimeSpan.FromSeconds(15);

        //Chrome driver nie wisi w procesach
        //   driver.Quit();
        //driver close zamyka przegladarke ale nie ubija drivera
        //  driver.Close();

        [Fact]
        //za pomoc¹ [Fact] testujemy jeden przypadek, za pomoc¹ [Theories] testujemy wiele przypadków
        public void Test1()
        {
            //driver.Manage().Window.Position = new System.Drawing.Point(8, 30);
            //driver.Manage().Window.Size = new System.Drawing.Size(1290, 730);

            driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
            driver.Manage().Window.Maximize();
            //Znajdz element o id = rejestracja
            IWebElement zarejestrujSie = driver.FindElement(By.Id("rejestracja"));
            // Kliknij na zarejestruj siê
            zarejestrujSie.Click();

            //uzupe³nianie danych do rejestracji
            IWebElement poleImie = driver.FindElement(By.Name("Imie"));
            poleImie.SendKeys("SeleniumImie");
            IWebElement poleEmail = driver.FindElement(By.Name("Email"));
            poleEmail.SendKeys("selenium9@po.pl");
            IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
            poleHaslo.SendKeys("Selenium2022");
            IWebElement polePowtorzHaslo = driver.FindElement(By.Id("powtorzHaslo"));
            polePowtorzHaslo.SendKeys("Selenium2022");

            //    IJavaScriptExecutor js = (IJavaScriptExecutor)driver; 
            //      js.ExecuteScript(String.Format("window.scrollTo({0}, {1})", 1000, 1000));
            //czeka .... sekund na znalezienie elementu
            //   driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(9);

            // driver.FindElement(By.XPath("xpath")).SendKeys(Keys.PageDown);
            //var element = driver.FindElement(By.Id("re3"));
            //Actions actions = new Actions(driver);
            //actions.MoveToElement(element);
            //actions.Perform();

            //Actions akcja = new Actions(driver);
            IWebElement rejestracja = driver.FindElement(By.Id("re3"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("arguments[0].click();", rejestracja);

            string rejestracjaPomyslnaURL = "https://localhost:7272/Uzytkownik/RejestracjaPomyslna";
            //  Assert.Same(rejestracjaPomyslnaURL, driver.Url);
            //  Assert.assertTrue(URL.startsWith("https://accounts.google.com"));
            Assert.True(driver.Url == rejestracjaPomyslnaURL, $"Strona, która siê pojawi³a to {driver.Url}");
            driver.Quit();
        }

        [Fact]
        public void LogowanieTest()
        {
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
            driver.Manage().Window.Maximize();
            IWebElement zalogujSie = driver.FindElement(By.Id("navLogowanie"));
            zalogujSie.Click();
            IWebElement poleEmail = driver.FindElement(By.Name("Email"));
            poleEmail.SendKeys("selenium9@po.pl");
            IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
            poleHaslo.SendKeys("Selenium2022");
            IWebElement zalogujSieKlik = driver.FindElement(By.Id("logowanie"));
            zalogujSieKlik.Click();
            string logowaniePomyslneURL = "https://localhost:7272/Portfel/MojePortfele";
            Assert.True(driver.Url == logowaniePomyslneURL, $"Strona, która siê pojawi³a to {driver.Url}");
            driver.Quit();
        }

        [Fact]
        public void DodawaniePortfelaTest()
        {
            LogowanieTest();
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272/Portfel/MojePortfele"));
            driver.Manage().Window.Maximize();
            //  IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[@text()='Dodaj nowy portfel'"));
            //IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[.='Dodaj nowy portfel']"));
            IWebElement dodajNowyPortfel = driver.FindElement(By.Id("dodajPortfel"));
            dodajNowyPortfel.Click();
            IWebElement poleNowyPortfel = driver.FindElement(By.Name("Nazwa"));
            poleNowyPortfel.SendKeys("SeleniumPortfel");
            IWebElement dodajPortfelKlik = driver.FindElement(By.Id("dodajPortfel"));
            dodajPortfelKlik.Click();
            var listaPortfeli = driver.FindElements(By.Id("nazwaPortfela"));
            Assert.Equal(listaPortfeli.Count, 4);
            driver.Quit();
        }

        //[Fact]
        //public void DodawanieWplatiWyplat()
        //{
        //    LogowanieTest();
        //    driver.Navigate().GoToUrl(new Uri("https://localhost:7272/Portfel/MojePortfele"));
        //    driver.Manage().Window.Maximize();
        //    IList<IWebElement> listaPortfeli = driver.FindElements(By.XPath("//*[@id='about']/div/table/thead/tr/th[1]"));
        //    Console.WriteLine("Liczba wierszy to" + listaPortfeli.Count());
    
            
        }
    }

    public class DodawanieAkcjiTest
    {
        IWebDriver driver = new ChromeDriver();
        private readonly ITestOutputHelper _testOutputHelper;

        public DodawanieAkcjiTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void Logowanie()
        {
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
            driver.Manage().Window.Maximize();
            IWebElement zalogujSie = driver.FindElement(By.Id("navLogowanie"));
            zalogujSie.Click();
            IWebElement poleEmail = driver.FindElement(By.Name("Email"));
            poleEmail.SendKeys("selenium9@po.pl");
            IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
            poleHaslo.SendKeys("Selenium2022");
            IWebElement zalogujSieKlik = driver.FindElement(By.Id("logowanie"));
            zalogujSieKlik.Click();
    }
    [Fact]
    public void WplacKwoteTest()
        {
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
            driver.Manage().Window.Maximize();
            IWebElement zalogujSie = driver.FindElement(By.Id("navLogowanie"));
            zalogujSie.Click();
            IWebElement poleEmail = driver.FindElement(By.Name("Email"));
            poleEmail.SendKeys("selenium9@po.pl");
            IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
            poleHaslo.SendKeys("Selenium2022");
            IWebElement zalogujSieKlik = driver.FindElement(By.Id("logowanie"));
            zalogujSieKlik.Click();
          IList<IWebElement> listaKolumn = driver.FindElements(By.XPath(".//*[@id='about']/div/table/thead/tr/th"));
          IList<IWebElement> listaWierszy = driver.FindElements(By.XPath(".//*[@id='about']/div/table/tbody/tr/td[1]"));

        // output.WriteLine("Liczba wierszy to ");
        _testOutputHelper.WriteLine("Liczba kolumn to: " + listaKolumn.Count());
        _testOutputHelper.WriteLine("Liczba wierszy to: " + listaWierszy.Count());
        //Console.WriteLine("Liczba wierszy to" );

        //  IWebElement tablica = driver.FindElement(By.TagName("table"));
        //wyszukuje pierwszy wiersz w tablicy
        //   IWebElement wierszPierwszy = tablica.FindElement(By.XPath(".//*[@id='about']/div/table/tbody/tr[1]"));
        //   IWebElement tablicaWierszTekst = tablica.FindElement(By.XPath(".//*[@id='about']/div/table/tbody/tr[1]/td[1]"));

        // string wierszTekst = wierszPierwszy.Text;;
        //  _testOutputHelper.WriteLine("Pierwszy wiersz wartoœci: " + wierszTekst);

       // int rowNumber = 1;
        string value = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[1]")).Text;
        _testOutputHelper.WriteLine("Pierwsza komórka pierwszego wiersza ma wartoœæ: " + value);
        string value2 = driver.FindElement(By.XPath(".//table/tbody/tr[1]")).Text;
        _testOutputHelper.WriteLine("Pierwsz wiersz ma wartoœci: " + value2);

        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement wplac = pierwszyWiersz.FindElement(By.Id("wplac"));
        wplac.Click();
        IWebElement poleWplata = driver.FindElement(By.TagName("input"));
        poleWplata.SendKeys("100,00");
        IWebElement przyciksWplac =
            driver.FindElement(By.XPath("//*[@id='about']/div/div[2]/div/div/div/div/form/div[2]/input"));
        przyciksWplac.Click();
        //je¿eli stan konta == 200,00 test powinien przejœæ
        string wartoscKonta = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        Assert.True(wartoscKonta=="200,00", $"Wartoœæ konta wynosi: {wartoscKonta}");
        }

    [Fact]
    public void KupAktywoTest()
    {

        Logowanie();
        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement kup = pierwszyWiersz.FindElement(By.Id("kup"));
        kup.Click();

        IWebElement wybierzAktywo = driver.FindElement(By.Name("AktywoId"));
        wybierzAktywo.Click();

        IWebElement kliknijAktywo = wybierzAktywo.FindElement(By.CssSelector("option[value='3']"));
        kliknijAktywo.Click();

        IWebElement cena = driver.FindElement(By.Name("Cena"));
        cena.SendKeys("1,00");
        IWebElement ilosc = driver.FindElement(By.Name("Ilosc"));
        ilosc.SendKeys("1");
        IWebElement komentarz = driver.FindElement(By.Name("Komentarz"));
        komentarz.SendKeys("Brak");

        IWebElement przyciksKupAktywo = driver.FindElement(By.Id("kupAktywo"));
        IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
        ex.ExecuteScript("arguments[0].click();", przyciksKupAktywo);

        _testOutputHelper.WriteLine("TEST");
    }

    [Fact]
        public void SzczegolyPortfelaTest()
        {
            Logowanie();
            IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
            IWebElement szczegolyPortfela = pierwszyWiersz.FindElement(By.Id("szczegoly"));
            szczegolyPortfela.Click();

            IList<IWebElement> listaWierszy = driver.FindElements(By.XPath(".//*[@id='about']/div/table/tbody/tr/td[1]"));
            Assert.True(listaWierszy.Count() == 1, $"Lista aktyw wynosi: {listaWierszy.Count()}");
    }
}
    //public class Tests2
        //{
        //    IWebDriver driver = new ChromeDriver();
        //    [Fact]
        //    public void LogowanieTest()
        //    {
        //        driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
        //        driver.Manage().Window.Maximize();
        //        IWebElement zalogujSie = driver.FindElement(By.Id("navLogowanie"));
        //        zalogujSie.Click();
        //        IWebElement poleEmail = driver.FindElement(By.Name("Email"));
        //        poleEmail.SendKeys("selenium9@po.pl");
        //        IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
        //        poleHaslo.SendKeys("Selenium2022");
        //        IWebElement zalogujSieKlik = driver.FindElement(By.Id("logowanie"));
        //        zalogujSieKlik.Click();
        //        string logowaniePomyslneURL = "https://localhost:7272/Portfel/MojePortfele";
        //        Assert.True(driver.Url == logowaniePomyslneURL, $"Strona, która siê pojawi³a to {driver.Url}");
        //        driver.Quit();
        //    }

        //    [Fact]
        //    public void DodawaniePortfelaTest()
        //    {
        //        LogowanieTest();
        //        driver.Navigate().GoToUrl(new Uri("https://localhost:7272/Portfel/MojePortfele"));
        //        driver.Manage().Window.Maximize();
        //      //  IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[@text()='Dodaj nowy portfel'"));
        //      //IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[.='Dodaj nowy portfel']"));
        //        IWebElement dodajNowyPortfel = driver.FindElement(By.Id("dodajPortfel"));
        //          dodajNowyPortfel.Click();
        //          IWebElement poleNowyPortfel = driver.FindElement(By.Name("Nazwa"));
        //          poleNowyPortfel.SendKeys("SeleniumPortfel");
        //          IWebElement dodajPortfelKlik = driver.FindElement(By.Id("dodajPortfel"));
        //          dodajPortfelKlik.Click();
        //          var listaPortfeli = driver.FindElements(By.Id("nazwaPortfela"));
        //          Assert.Equal(listaPortfeli.Count, 4);
        //          driver.Quit();
        //    }
        //}
    
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V106.CSS;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using Xunit.Abstractions;


namespace Portfel.TestySelenium
{
    public class RejestracjaLogowanieTest
    {

        IWebDriver driver = new ChromeDriver();
        private readonly ITestOutputHelper _testOutputHelper;

        public RejestracjaLogowanieTest(ITestOutputHelper testOutputHelper)
        {
            //pozwala na wyswietlanie na consoli
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        //za pomoc¹ [Fact] testujemy jeden przypadek, za pomoc¹ [Theories] testujemy wiele przypadków
        public void RejestracjaTest()
        {
            
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272"));
            driver.Manage().Window.Maximize();
            //Znajdz element o id = rejestracja
            IWebElement zarejestrujSie = driver.FindElement(By.Id("rejestracja"));
            //Kliknij na zarejestruj siê
            zarejestrujSie.Click();
            //uzupe³nianie danych do rejestracji
            IWebElement poleImie = driver.FindElement(By.Name("Imie"));
            poleImie.SendKeys("SeleniumImie");
            IWebElement poleEmail = driver.FindElement(By.Name("Email"));
            poleEmail.SendKeys("selenium19@po.pl");
            IWebElement poleHaslo = driver.FindElement(By.Name("Haslo"));
            poleHaslo.SendKeys("Selenium2022");
            IWebElement polePowtorzHaslo = driver.FindElement(By.Id("powtorzHaslo"));
            polePowtorzHaslo.SendKeys("Selenium2022");

            //Znajdz element o id= re3
            IWebElement rejestracja = driver.FindElement(By.Id("re3"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("arguments[0].click();", rejestracja);

            string rejestracjaPomyslnaURL = "https://localhost:7272/Uzytkownik/Create";
            //Test przejdzie je¿eli znajdziemy siê na konkretnej stronie
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
          //  driver.Quit();
        }

        [Fact]
        public void DodawaniePortfelaTest()
        {
            LogowanieTest();
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272/Portfel/MojePortfele"));
            driver.Manage().Window.Maximize();
            //IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[@text()='Dodaj nowy portfel'"));
            //IWebElement dodajNowyPortfel = driver.FindElement(By.XPath("//button[.='Dodaj nowy portfel']"));
            IWebElement dodajNowyPortfel = driver.FindElement(By.Id("dodajPortfel"));
            dodajNowyPortfel.Click();
            IWebElement poleNowyPortfel = driver.FindElement(By.Name("Nazwa"));
            var nowaNazwaPortfela = "SeleniumPortfel20";
            poleNowyPortfel.SendKeys(nowaNazwaPortfela);
            IWebElement dodajPortfelKlik = driver.FindElement(By.Id("dodajPortfel"));
            dodajPortfelKlik.Click();
            
            //lista wszystkich portfeli
            IList<IWebElement> listaKolumn = driver.FindElements(By.XPath(".//*[@id='about']/div/table/thead/tr/th"));
            IList<IWebElement> listaWierszy = driver.FindElements(By.XPath(".//*[@id='about']/div/table/tbody/tr/td[1]"));

            // output.WriteLine("Liczba wierszy to ");
            _testOutputHelper.WriteLine("Liczba kolumn to: " + listaKolumn.Count());
            _testOutputHelper.WriteLine("Liczba wierszy to: " + listaWierszy.Count());
            string value = driver.FindElement(By.XPath(".//table/tbody/tr["+ listaWierszy.Count() + "]/td[1]")).Text;
            _testOutputHelper.WriteLine("Pierwsza komórka "+ listaWierszy.Count() + " wiersza ma wartoœæ: " + value);
            var ostatniaNazwaPortfela = value;
 
            Assert.Equal(nowaNazwaPortfela,ostatniaNazwaPortfela);
            ////Chrome driver nie wisi w procesach
            driver.Quit();
        }

        [Fact]
        public void DodawaniePortfelaGdyNazwaPortfelaJuzIstniejeTest()
        {
            LogowanieTest();
            driver.Navigate().GoToUrl(new Uri("https://localhost:7272/Portfel/MojePortfele"));
            driver.Manage().Window.Maximize();

            IWebElement dodajNowyPortfel = driver.FindElement(By.Id("dodajPortfel"));
            dodajNowyPortfel.Click();
            IWebElement poleNowyPortfel = driver.FindElement(By.Name("Nazwa"));
            var nowaNazwaPortfela = "SeleniumPortfel20";
            poleNowyPortfel.SendKeys(nowaNazwaPortfela);
            IWebElement dodajPortfelKlik = driver.FindElement(By.Id("dodajPortfel"));
            dodajPortfelKlik.Click();

            var portfelJuzIstnieje = driver.FindElement(By.Id("portfelIstnieje")).Text;
            _testOutputHelper.WriteLine(portfelJuzIstnieje);

            Assert.Equal("Podaj inn¹ nazwê portfela, taka ju¿ istnieje.", portfelJuzIstnieje);
            //Chrome driver nie wisi w procesach
            driver.Quit();
        }

    }
    }

    public class DodawanieAkcjiTest
    {
        IWebDriver driver = new ChromeDriver();
        private readonly ITestOutputHelper _testOutputHelper;

        public DodawanieAkcjiTest(ITestOutputHelper testOutputHelper)
        {
            //pozwala na wyswietlanie na consoli
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

        _testOutputHelper.WriteLine("Liczba kolumn to: " + listaKolumn.Count());
        _testOutputHelper.WriteLine("Liczba wierszy to: " + listaWierszy.Count());
      
        string value = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[1]")).Text;
        _testOutputHelper.WriteLine("Pierwsza komórka pierwszego wiersza ma wartoœæ: " + value);
        string value2 = driver.FindElement(By.XPath(".//table/tbody/tr[1]")).Text;
        _testOutputHelper.WriteLine("Pierwsz wiersz ma wartoœci: " + value2);

        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement wplac = pierwszyWiersz.FindElement(By.Id("wplac"));
        var stanKonta = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        wplac.Click();
        IWebElement poleWplata = driver.FindElement(By.TagName("input"));
        var wplata = "100,00";
        poleWplata.SendKeys(wplata);
        IWebElement przyciksWplac =
            driver.FindElement(By.XPath("//*[@id='about']/div/div[2]/div/div/div/div/form/div[2]/input"));
        przyciksWplac.Click();
        var nowyStanKonta = decimal.Parse(stanKonta) + decimal.Parse(wplata);
                            
        var wartoscKonta = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
            //je¿eli stan konta == stan konta + wp³ata - test powinien przejœæ
        Assert.True(wartoscKonta == nowyStanKonta.ToString(), $"Wartoœæ konta wynosi: {wartoscKonta}");
        //Chrome driver nie wisi w procesach
        driver.Quit();
    }
    [Fact]
    public void WyplacKwoteTest()
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

        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement wyplac = pierwszyWiersz.FindElement(By.Id("wyplac"));
        var stanKonta = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        wyplac.Click();
        IWebElement poleWyplata = driver.FindElement(By.TagName("input"));
        var wyplata = "10,00";
        poleWyplata.SendKeys(wyplata);
        IWebElement przyciksWyplac =
            driver.FindElement(By.XPath("//*[@id='about']/div/div[2]/div/div/div/div/form/div[2]/input"));
        przyciksWyplac.Click();
        var nowyStanKonta = decimal.Parse(stanKonta) - decimal.Parse(wyplata);

        var wartoscKonta = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        _testOutputHelper.WriteLine("Wartoœæ konta po wyp³acie: " + wartoscKonta);
        //je¿eli stan konta == stan konta + wp³ata - test powinien przejœæ
        Assert.True(wartoscKonta == nowyStanKonta.ToString(), $"Wartoœæ konta wynosi: {wartoscKonta}");
        //Chrome driver nie wisi w procesach
        driver.Quit();
    }

    [Fact]
    public void KupAktywoTest()
    {

        Logowanie();
        var stanPrzedZakupem = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement kup = pierwszyWiersz.FindElement(By.Id("kup"));
        kup.Click();

        IWebElement wybierzAktywo = driver.FindElement(By.Name("AktywoId"));
        wybierzAktywo.Click();

        IWebElement kliknijAktywo = wybierzAktywo.FindElement(By.CssSelector("option[value='3']"));
        kliknijAktywo.Click();

        IWebElement cena = driver.FindElement(By.Name("Cena"));
        var cenaAktywa = "1,00";
        cena.SendKeys(cenaAktywa);
        IWebElement ilosc = driver.FindElement(By.Name("Ilosc"));
        var iloscAktywa = "1";
        ilosc.SendKeys(iloscAktywa);
        IWebElement komentarz = driver.FindElement(By.Name("Komentarz"));
        komentarz.SendKeys("Brak");
        var kosztZakupu = decimal.Parse(cenaAktywa) * decimal.Parse(iloscAktywa);
        IWebElement przyciksKupAktywo = driver.FindElement(By.Id("kupAktywo"));
        IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
        ex.ExecuteScript("arguments[0].click();", przyciksKupAktywo);

        var stanKontaPoZakupie = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        var nowyStanKonta = decimal.Parse(stanPrzedZakupem) - kosztZakupu;
        Assert.True(stanKontaPoZakupie == nowyStanKonta.ToString(), $"Wartoœæ konta wynosi: {stanKontaPoZakupie}, a test wykazuje: {nowyStanKonta}");
        //Chrome driver nie wisi w procesach
        driver.Quit();

    }
    [Fact]
    public void SprzedajAktywoTest()
    {

        Logowanie();
        var stanPrzedSprzedaza = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
        IWebElement sprzedaj = pierwszyWiersz.FindElement(By.Id("sprzedaj"));
        sprzedaj.Click();

        IWebElement wybierzAktywo = driver.FindElement(By.Name("AktywoId"));
        wybierzAktywo.Click();

        IWebElement kliknijAktywo = wybierzAktywo.FindElement(By.CssSelector("option[value='3']"));
        kliknijAktywo.Click();

        IWebElement cena = driver.FindElement(By.Name("Cena"));
        var cenaAktywa = "1,00";
        cena.SendKeys(cenaAktywa);
        IWebElement ilosc = driver.FindElement(By.Name("Ilosc"));
        var iloscAktywa = "1";
        ilosc.SendKeys(iloscAktywa);
        IWebElement komentarz = driver.FindElement(By.Name("Komentarz"));
        komentarz.SendKeys("Brak");
        var kosztSprzedazy = decimal.Parse(cenaAktywa) * decimal.Parse(iloscAktywa);
        IWebElement przyciskSprzedajAktywo = driver.FindElement(By.Id("sprzedajAktywo"));
        IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
        ex.ExecuteScript("arguments[0].click();", przyciskSprzedajAktywo);

        var stanKontaPoSprzedazy = driver.FindElement(By.XPath(".//table/tbody/tr[1]/td[2]")).Text;
        var nowyStanKonta = decimal.Parse(stanPrzedSprzedaza) + kosztSprzedazy;
        Assert.True(stanKontaPoSprzedazy == nowyStanKonta.ToString(), $"Wartoœæ konta wynosi: {stanKontaPoSprzedazy}, a test wykazuje: {nowyStanKonta}");
        //Chrome driver nie wisi w procesach
        driver.Quit();

    }

    [Fact]
        public void SzczegolyPortfelaTest()
        {
            Logowanie();
            IWebElement pierwszyWiersz = driver.FindElement(By.XPath(".//table/tbody/tr[1]"));
            IWebElement szczegolyPortfela = pierwszyWiersz.FindElement(By.Id("szczegoly"));
            szczegolyPortfela.Click();
            var listaAktyw = "3";
            IList<IWebElement> listaWierszy = driver.FindElements(By.XPath(".//*[@id='about']/div/table/tbody/tr/td[1]"));
            Assert.True(listaWierszy.Count() == decimal.Parse(listaAktyw), $"Lista aktyw wynosi: {listaWierszy.Count()}");
            //Chrome driver nie wisi w procesach
            driver.Quit();
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
    
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace RedArbor
{
    //Crear clase cliente para rellenar el formulario del TC 4
    class Client
    {
        //Atributos
        private string name = "Jordi";
        private string email = "jordivallejo.job@gmail.com";
        private string phone = "698502649";
        private string message = "Este mensaje ha sido generado mediante un test automatizado";

        //Constructor por defecto
        public Client()
        {
        }

        //Getters
        public string getName()
        {
            return this.name;
        }
        public string getEmail()
        {
            return this.email;
        }
        public string getPhone()
        {
            return this.phone;
        }
        public string getMessage()
        {
            return this.message;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Instanciar el objeto ChromeDriver y maximizar la ventana
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            try
            {
                //Declarar variables
                string url = "https://www.milanuncios.com";
                string textToSearch = "Stores amarillos";
                string expectedUbication = "Esparreguera";
                Client client = new Client();

                //Navegar a la página web y aceptar sus cookies
                driver.Url = url;
                IWebElement cookiesBtn = driver.FindElementByXPath("//button[span = 'Aceptar y cerrar']");
                cookiesBtn.Click();

                /*
                TC 1.
                Insertar "Stores amarillos"
                Clicar en el boton "Buscar"
                */

                IWebElement mainSearcher = driver.FindElementByXPath("//input[@placeholder = 'Qué buscas']");
                IWebElement mainSearcherBtn = driver.FindElementByXPath("//button[span = 'Buscar']");
                mainSearcher.SendKeys(textToSearch);
                ClickAndWait(mainSearcherBtn);

                /*
                TC 2.
                Filtrar por "sólo particulares" y "Barcelona"
                Clicar en el boton "Buscar"
                */

                IWebElement provinceSelect = driver.FindElementById("protmp");
                IWebElement sellerSelect = driver.FindElementById("vendedor");
                IWebElement searcherBtn = driver.FindElementById("vamos");
                SelectElement provinceOption = new SelectElement(provinceSelect);
                SelectElement sellerOption = new SelectElement(sellerSelect);
                provinceOption.SelectByValue("barcelona");
                sellerOption.SelectByValue("part");
                ClickAndWait(searcherBtn);

                /*
                TC 3.
                Seleccionar "STORES AMARILLOS"
                Verificar ubicación "Esparreguera"
                */

                IWebElement announcementLink = driver.FindElementByXPath("//a[@href='/cortinas/stores-amarillos-383296021.htm']");
                ClickAndWait(announcementLink);

                //Cambiar el focus para operar en la nueva tab
                driver.SwitchTo().Window(driver.WindowHandles[1]);

                IWebElement ubication = driver.FindElementByXPath("//div[text() = '- Cortinas en Esparreguera/esparraguera (BARCELONA)']");

                //Verificar que la ubicación coincida con lo esperado, en caso afirmativo continuamos con el TC 4.            
                if (ubication.Text.Contains(expectedUbication))
                {
                    /*
                    TC 4.
                    Clicar en el botón "Mensaje"
                    Rellenar el formulario y enviarlo"
                    */

                    IWebElement messageBtn = driver.FindElementByXPath("//a[span = 'Mensaje']");
                    ClickAndWait(messageBtn);

                    //Cambiar el focus para operar en el nuevo frame
                    driver.SwitchTo().Frame(driver.FindElementById("ifrw"));

                    IWebElement nameInput = driver.FindElementById("nombre");
                    IWebElement emailInput = driver.FindElementById("email");
                    IWebElement phoneInput = driver.FindElementById("tfno");
                    IWebElement messageTextarea = driver.FindElementById("mensaje");
                    IWebElement tosCheckbox = driver.FindElementById("legal-checkbox");
                    IWebElement sendingBtn = driver.FindElementById("btnsubmit");
                    nameInput.SendKeys(client.getName());
                    emailInput.SendKeys(client.getEmail());
                    phoneInput.SendKeys(client.getPhone());
                    messageTextarea.SendKeys(client.getMessage());
                    tosCheckbox.Click();
                    ClickAndWait(sendingBtn);
                }
                //Checker de que hemos llegado al final del codigo try
                Console.WriteLine("Prueba finalizada con exito");
            }
            //Cerrar Chrome independientemente de si hemos completado con éxito o no la prueba 
            finally
            {
                driver.Quit();
            }
        }
        //Clicar y dejar un tiempo para que cargue páginas nuevas, iframes, submit... 
        public static void ClickAndWait(IWebElement element)
        {
            element.Click();
            Thread.Sleep(3000);
        }
    }
}
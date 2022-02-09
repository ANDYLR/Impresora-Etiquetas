using System;

namespace Impresora_Etiquetas.Controllers
{
    public class AppController
    {
        public static AppController instance;
        public static AppController GetInstance()
        {
            if (instance == null)
            {
                instance = new AppController();
            }
            return instance;
        }
        public string gNombreImpresora { get; set; }
        

    }
}
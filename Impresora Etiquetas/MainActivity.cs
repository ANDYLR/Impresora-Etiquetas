using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using System.Threading;
using System.Threading.Tasks;
using Java.Util;
using Impresora_Etiquetas.Controllers;
using System.Text;

namespace Impresora_Etiquetas
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private AppController app;
        string nombreImpresora = string.Empty;
        private LinkOS.Plugin.Abstractions.IConnection conexion = null;
        private BluetoothManager _manager;

        private Button btnImpresion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = AppController.GetInstance();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            btnImpresion = FindViewById<Button>(Resource.Id.btnImpresion);
            btnImpresion.Click += BtnImpresion_Click;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }


        private async void BtnImpresion_Click(object sender, EventArgs e)
        {
            string macAdress = "";
            btnImpresion.Enabled = false;


            Android.Bluetooth.BluetoothAdapter btAdapter;
            btAdapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter;

            try
            {
                _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
                if (_manager.Adapter.IsEnabled == false)
                {
                    _manager.Adapter.Enable();
                    Thread.Sleep(2000);
                }

            }
            catch (Exception ex)
            {

                Toast.MakeText(this, "Error al activar Bluetooth", ToastLength.Short).Show();
            }

            // Get a set of currently paired devices
            var pairedDevices = btAdapter.BondedDevices;

            // If there are paired devices, add each one to the ArrayAdapter
            if (pairedDevices.Count > 0)
            {

                foreach (var device in pairedDevices)
                {
                    Toast.MakeText(this, "Impresora - " + device.Name + "\n" + device.Address, ToastLength.Short).Show();
                    macAdress = device.Address;
                    nombreImpresora = device.Name;



                    try
                    {
                        await Task.Delay(100);
                        BluetoothSocket mySocket;
                        await Task.Delay(100);
                        UUID my_uuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
                        mySocket = device.CreateInsecureRfcommSocketToServiceRecord(my_uuid);
                        await Task.Run(() =>
                        {
                            mySocket.Connect();
                        });

                        if (mySocket.IsConnected)
                        {
                            app.gNombreImpresora = device.Name.ToString();
                            mySocket.Close();
                            await Task.Delay(100);

                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(100);
                        Toast.MakeText(this, "Favor de encender la impresora", ToastLength.Short).Show();
                        Console.WriteLine(ex.Message);
                        btnImpresion.Enabled = true;
                        return;
                    }
                }
            }
            else
            {
                Toast.MakeText(this, "No hay dispositivos conectados", ToastLength.Long).Show();
                btnImpresion.Enabled = true;
                return;
            }

            await Impresion(macAdress);

            btnImpresion.Enabled = true;
        }


        private async Task Impresion(string macAdress)
        {

            int longitud = 1200;
            int distancia = 0;
            string codigoQR = "SOLUCIONES MOVILES INTEGRALES - ICONN";
            string codigoBarras1 = "ICCON";
            string codigoBarras2 = "SOLUCIONES MOVILES";
            int loc = 19040120;
            int paq = 12;
            string tam= "946 ML";
            string descripcion = "P7 ALTO KILOMETRAJE SAE 25 W50";
            int pedido = 944835;
            int no = 106;
            string nombre = "PETROMAX 6056 T1 VIA PONIENTE ES";

            string zplEtiqueta =

                "^FX LAS SIGUIENTES DOS LINEAS IMPRIMEN UN CODIGO QR, MODIFICA TAMAÑO CON ULTIMO DIGITO EJEMPLO : BQN,2,10 <--MODIFICAR EL 10"
                  + "^FT" + distancia.ToString() + "," + (longitud-20).ToString() + "^BQN,2,10" 
                +"^FH\\^FDLA," + codigoQR.ToString() + "^FS"

                + "^FX LAS SIGUIENTES DOS LINEAS IMPRIMEN UN CODIGO DE BARRAS, MODIFICA LO ALTO DEL CODIGO EJEMPLO: BY4,3,95 <-- MODIFICA EL 95, " +
                    "LO ANCHO LO HACE EN AUTOMATICO DEPENDIENDO LOS DATOS QUE SE INGRESEN"
                + "^BY4,3,95^FT" + distancia.ToString() + "," + (longitud - 363).ToString() + "^BCB,,N,N"
                + "^FD>:" + codigoBarras1.ToString() + "^FS"

                + "^FX LAS SIGUIENTES LINEAS IMPRIMEN TEXTO, MODIFICA EL TAMAÑO DE LETRA EJEMPLO: A0B,30,28 (30 <-ALTO,28 <-ANCHO)"
                + "^FT" + (30 + distancia).ToString() + "," + (longitud - 758).ToString() + "^A0B,30,28^FH\\^FDLoc^FS"
                + "^FT" + (70 + distancia).ToString() + "," + (longitud - 758).ToString() + "^A0B,33,31^FH\\^FD"+ loc.ToString()+"^FS"
                + "^FT" + (30 + distancia).ToString() + "," + (longitud - 911).ToString() + "^A0B,30,28^FH\\^FDPaq^FS"
                + "^FT" + (73 + distancia).ToString() + "," + (longitud - 911).ToString() + "^A0B,30,28^FH\\^FD" + paq.ToString() + "^FS"
                + "^FT" + (30 + distancia).ToString() + "," + (longitud - 967).ToString() + "^A0B,30,28^FH\\^FDTam^FS"
                + "^FT" + (73 + distancia).ToString() + "," + (longitud - 967).ToString() + "^A0B,30,28^FH\\^FD" + tam.ToString() + "^FS"
                + "^FT" + (37 + distancia).ToString() + "," + (longitud - 1100).ToString() + "^A0B,37,33^FH\\^FDP^FS"
                + "^FT" + (37 + distancia).ToString() + "," + (longitud - 1135).ToString() + "^A0B,37,33^FH\\^FD3^FS"
                + "^FX LINEA RECTA"
                + "^FO" + (105 + distancia).ToString() + "," + (longitud - 1070).ToString() + "^GB0,725,4^FS"
                + "^FX TEXTO"
                + "^FT" + (150 + distancia).ToString() + "," + (longitud - 363).ToString() + "^A0B,41,38^FH\\^FD" + descripcion.ToString() + "^FS"
                + "^FT" + (133 + distancia).ToString() + "," + (longitud - 1105).ToString() + "^A0B,98,139^FH\\^FD3^FS"
                + "^FX LINEA RECTA"
                + "^FO" + (165 + distancia).ToString() + "," + (longitud - 1062).ToString() + "^GB0,725,4^FS"
                + "^FX TEXTO"
                + "^FT" + (204 + distancia).ToString() + "," + (longitud - 365).ToString() + "^A0B,37,33^FH\\^FDPedido^FS"
                + "^FT" + (246 + distancia).ToString() + "," + (longitud - 365).ToString() + "^A0B,37,33^FH\\^FD" + pedido.ToString() + "^FS"
                + "^FT" + (204 + distancia).ToString() + "," + (longitud - 490).ToString() + "^A0B,37,33^FH\\^FDNo^FS"
                + "^FT" + (246 + distancia).ToString() + "," + (longitud - 490).ToString() + "^A0B,37,33^FH\\^FD" + no.ToString() + "^FS"
                + "^FT" + (204 + distancia).ToString() + "," + (longitud - 579).ToString() + "^A0B,37,33^FH\\^FDNombre^FS"
                + "^FT" + (246 + distancia).ToString() + "," + (longitud - 579).ToString() + "^A0B,37,33^FH\\^FD" + nombre.ToString() + "^FS"
                + "^FT" + (204 + distancia).ToString() + "," + (longitud - 745).ToString() + "^A0B,33,31^FH\\^FDFecha: ^FS"
                + "^FT" + (204 + distancia).ToString() + "," + (longitud - 840).ToString() + "^A0B,33,31^FH\\^FD5/1^FS"
                + "^FX LINEA RECTA"
                + "^FO" + (253 + distancia).ToString() + "," + (longitud - 1070).ToString() + "^GB0,725,4^FS"

                + "^FX LAS SIGUIENTES DOS LINEAS IMPRIMEN UN CODIGO DE BARRAS"
                + "^BY4,3,97^FT" + (368 + distancia).ToString() + "," + (longitud - 90).ToString() + "^BCB,,N,N"
                + "^FD>:" + codigoBarras2.ToString() + "^FS"

                + "^FX TEXTO"
                + "^FT" + (300 + distancia).ToString() + "," + (longitud - 1080).ToString() + "^A0B,30,28^FH\\^FDCant^FS"
                + "^FT" + (340 + distancia).ToString() + "," + (longitud - 1080).ToString() + "^A0B,30,28^FH\\^FD001/001^FS";
  

            string zpl = "^XA^MNN^LL" + longitud.ToString() + "^JUS" + zplEtiqueta+ "^XZ";


            try
            {

                string setLanguageZpl = "! U1 setvar \"device.languages\" \"zpl\"\r\n\r\n! U1 getvar \"device.languages\"\r\n\r\n";
                string setLanguageCPCL = "! U1 setvar \"device.languages\" \"line_print\"\r\n\r\n! U1 getvar \"device.languages\"\r\n\r\n";

                await Task.Delay(500);
                Zebra.Sdk.Comm.BluetoothConnectionInsecure connec = new Zebra.Sdk.Comm.BluetoothConnectionInsecure(macAdress);
                await Task.Delay(500);
                await Task.Delay(500);
                connec.Open();
                byte[] respuesta = connec.SendAndWaitForResponse(Encoding.ASCII.GetBytes(setLanguageZpl), 500, 500, "");
                string s = Encoding.ASCII.GetString(respuesta);
                await Task.Delay(500);
                
                //ENVIA CODIGO ZPL A IMPRIMIR
                connec.Write(Encoding.ASCII.GetBytes(zpl));
                await Task.Delay(1000);

                byte[] respuesta2 = connec.SendAndWaitForResponse(Encoding.ASCII.GetBytes(setLanguageCPCL), 500, 500, "");
                string s2 = Encoding.ASCII.GetString(respuesta2);
                await Task.Delay(1000);
                connec.Close();
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Toast.MakeText(this, "Fallo al imprimir", ToastLength.Short).Show();
            }

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


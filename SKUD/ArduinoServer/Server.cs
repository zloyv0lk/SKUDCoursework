using System;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using ArduinoServer.Properties;


namespace ArduinoServer { 
    public static class Server {
        public static void Main()
        {
            try
            {
                var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                FileInfo fileInf = new FileInfo(appDir + "/TestFile.txt");
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                }

                SerialPort port;
                int timeout = 100000;
                port = new SerialPort("COM5", 9600);
                port.Open();
                port.ReadTimeout = timeout;

                if (port.IsOpen)
                {
                    while (true)
                    {
                        // Если данные поступили пытаемся их прочитать
                        try
                        {
                            string returnedMesssage = port.ReadExisting();
                            Thread.Sleep(200);

                            if (returnedMesssage.Contains("**Card Detected:**"))
                            {
                                StreamWriter file = new StreamWriter(appDir + "/TestFile.txt");
                                //записать в него
                                file.Write(returnedMesssage);
                                //закрыть для сохранения данных
                                file.Close();

                            }


                        }
                        catch (TimeoutException ex)
                        {
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
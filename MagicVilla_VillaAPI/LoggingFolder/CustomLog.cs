using MagicVilla_VillaAPI.Logging;

namespace MagicVilla_VillaAPI.LoggingFolder
{

    public class CustomLog: ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("Error - " + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
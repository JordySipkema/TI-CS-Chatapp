using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TI_CS_Chatapp
{
    public class data
    {
        public int Id { get; set; }
        public int SSN { get; set; }
        public string Message { get; set; }

    }

    static class Settings
    {
        public static string ServerIP { get; set; }
        public static string Nickname { get; set; }


        public static void loadSettingsFromFile()
        {
            

        }

        public static void saveSettingsToFile()
        {
            // Iets als dit; WIP
            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                Id = 1,
                SSN = 2,
                Message = "A Message"
            });
            string json = JsonConvert.SerializeObject(_data.ToArray());

            //write string to file
            System.IO.File.WriteAllText(@"settings.txt", json);

        }



    }
}

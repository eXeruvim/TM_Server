using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace Tm_Server.Scripts
{
    class getCompDetails
    {
        public string cpuGetInformation(String value)
        {
            ManagementObjectSearcher searcher;
            string terms = null;

            switch (value)
            {
                case "name":
                    searcher = new ManagementObjectSearcher($"select {value} from Win32_Processor");
               
                    foreach (var item in searcher.Get())
                    {
                        terms = item["name"].ToString();
                    }

                    break;

                case "MaxClockSpeed":
                    searcher = new ManagementObjectSearcher($"select {value} from Win32_Processor");

                    foreach (var item in searcher.Get())
                    {
                        terms = (Convert.ToDouble(item["MaxClockSpeed"]) / 1000).ToString("0.00");
                    }

                    break;
                
                default:
                    return $"Invalid argument or code fault, argument is {value}";
            }

            return terms;
        }

        private string gpuGetInformation()
        {
            ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("select name from Win32_VideoController");
            string Procname = null;

            foreach (ManagementObject item in mosProcessor.Get())
            {
                if (item["Name"] != null)
                {
                    Procname = item["Name"].ToString();
                }
            }

            return Procname;

        }

        public float getFreeMemory()
        {
            ObjectQuery wql = new 
                ObjectQuery("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            float answer = 0.0f;
            
            foreach (ManagementObject item in results)
            {
                answer = (float)(Convert.ToDouble(item["FreePhysicalMemory"]) / (1024 * 1024));     // Free Physical Memoryy GB
            }

            return answer;
        }

        public float getTotalMemory()
        {

            ObjectQuery wql = new 
                ObjectQuery("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            float answer = 0.0f;

            foreach (ManagementObject item in results)
            {
                answer = (float)(Convert.ToDouble(item["TotalVisibleMemorySize"]) / (1024 * 1024)); // Total visible memory GB
            }

            return answer;
        }

        public object[,] fillTable()
        {
            object[,] computerInfo = new object[2, 4];

            computerInfo[0, 0] = "Имя устройства";
            computerInfo[1, 0] = SystemInformation.ComputerName;

            computerInfo[0, 1] = "Процессор";
            computerInfo[1, 1] = cpuGetInformation("name");

            computerInfo[0, 2] = "Оперативная память";
            computerInfo[1, 2] = (getTotalMemory()).ToString("0.00") + " GB";

            computerInfo[0, 3] = "Графический процессор";
            computerInfo[1, 3] = gpuGetInformation();

            return computerInfo;
        }
    }
}

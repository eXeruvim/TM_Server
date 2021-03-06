using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace Tm_Server.Scripts
{
    public class HDD
    {

        public int Index { get; set; }
        public string DeviceId { get; set; }
        public string InstanceName { get; set; }
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
        public Dictionary<int, Smart> Attributes = new Dictionary<int, Smart>() {
            {0x00, new Smart("Invalid")},
            {0x01, new Smart("Raw read error rate")},
            {0x02, new Smart("Throughput performance")},
            {0x03, new Smart("Spinup time")},
            {0x04, new Smart("Start/Stop count")},
            {0x05, new Smart("Reallocated sector count")},
            {0x06, new Smart("Read channel margin")},
            {0x07, new Smart("Seek error rate")},
            {0x08, new Smart("Seek time performance")},
            {0x09, new Smart("Power-on hours count")},
            {0x0A, new Smart("Spinup retry count")},
            {0x0B, new Smart("Calibration retry count")},
            {0x0C, new Smart("Power cycle count")},
            {0x0D, new Smart("Soft read error rate")},
            {0x16, new Smart("Current Helium Level (HGST He8 only!)")},
            {0xAA, new Smart("Available Reserved Space (Intel SSD)")},
            {0xAB, new Smart("SmartProgram Fail Count (Kingston SSD)")},
            {0xAC, new Smart("Erase Fail Count (Kingston SSD)")},
            {0xAD, new Smart("Wear Leveling Count (SSD)")},
            {0xAE, new Smart("Unexpected power loss count (SSD)")},
            {0xAF, new Smart("Power Loss Protection Failure")},
            {0xB0, new Smart("Erase Fail Count (chip)")},
            {0xB1, new Smart("Wear Range Delta (SSD)")},
            {0xB3, new Smart("Used Reserved Block Count (Samsung)")},
            {0xB4, new Smart("Unused Reserved Block Count (HP)")},
            {0xB5, new Smart("SmartProgram Fail Count Total / Non-4K Aligned Access Count")},
            {0xB6, new Smart("Erase Fail Count (Samsung)")},
            {0xB7, new Smart("SATA Downshift Error Count / Runtime Bad Block (Western Digital, Samsung or Seagate)")},
            {0xB8, new Smart("End-to-End error / IOEDC (HP)")},
            {0xB9, new Smart("Head Stability (Western Digital)")},
            {0xBA, new Smart("Induced Op-Vibration Detection (Western Digital)")},
            {0xBB, new Smart("Reported Uncorrectable Errors (Hardware ECC)")},
            {0xBC, new Smart("Command Timeout")},
            {0xBD, new Smart("High Fly Writes (Seagate, Western Digital)")},
            {0xBE, new Smart("Airflow Temperature (WDC) / Airflow Temperature Celsius (HP) / Temperature diff. from 100")},
            {0xBF, new Smart("G-sense error rate")},
            {0xC0, new Smart("Power-off retract count / Emergency Retract Cycle Count (Fujitsu) / Unsafe Shutdown Count")},
            {0xC1, new Smart("Load Cycle Count / Load/Unload cycle count (Fujitsu)")},
            {0xC2, new Smart("HDD temperature")},
            {0xC3, new Smart("Hardware ECC recovered")},
            {0xC4, new Smart("Reallocation count")},
            {0xC5, new Smart("Current pending sector count")},
            {0xC6, new Smart("Offline scan uncorrectable count")},
            {0xC7, new Smart("UDMA CRC error rate")},
            {0xC8, new Smart("Multi-Zone Error Rate / Write error rate (Fujitsu)")},
            {0xC9, new Smart("Soft read error rate / TA Counter Detected")},
            {0xCA, new Smart("Data Address Mark errors / TA Counter Increased")},
            {0xCB, new Smart("Run out cancel")},
            {0xCC, new Smart("Soft ECC correction")},
            {0xCD, new Smart("Thermal asperity rate (TAR)")},
            {0xCE, new Smart("Flying height")},
            {0xCF, new Smart("Spin high current")},
            {0xD0, new Smart("Spin buzz")},
            {0xD1, new Smart("Offline seek performance")},
            {0xD2, new Smart("Vibration During Write (Maxtor)")},
            {0xD3, new Smart("Vibration During Write")},
            {0xD4, new Smart("Shock During Write")},
            {0xDC, new Smart("Disk shift")},
            {0xDD, new Smart("G-sense error rate")},
            {0xDE, new Smart("Loaded hours")},
            {0xDF, new Smart("Load/unload retry count")},
            {0xE0, new Smart("Load friction")},
            {0xE1, new Smart("Load/Unload cycle count")},
            {0xE2, new Smart("Load-in time")},
            {0xE3, new Smart("Torque amplification count")},
            {0xE4, new Smart("Power-off retract cycle")},
            {0xE6, new Smart("GMR head amplitude / Drive Life Protection Status")},
            {0xE7, new Smart("Temperature (HDD) / Life Left (SSD)")},
            {0xE8, new Smart("Endurance Remaining (HDD) / Available Reserved Space (Intel SSD)")},
            {0xE9, new Smart("Power-On Hours (HDD) / Media Wearout Indicator (Intel SSD)")},
            {0xEA, new Smart("Average erase count AND Maximum Erase Count")},
            {0xEB, new Smart("Good Block Count AND System (Free) Block Count")},
            {0xF0, new Smart("Head flying hours / Transfer Error Rate (Fujitsu)")},
            {0xF1, new Smart("Total LBAs Written")},
            {0xF2, new Smart("Total LBAs Read")},
            {0xF3, new Smart("Total LBAs Written Expanded")},
            {0xF4, new Smart("Total LBAs Read Expanded")},
            {0xF9, new Smart("NAND_Writes_1GiB")},
            {0xFA, new Smart("Read error retry rate")},
            {0xFB, new Smart("Minimum Spares Remaining")},
            {0xFC, new Smart("Newly Added Bad Flash Block")},
            {0xFE, new Smart("Free Fall Protection")},

            /* slot in any new codes you find in here */
        };

    }

    public class Smart
    {
        public bool HasData
        {
            get
            {
                if (Current == 0 && Worst == 0 && Threshold == 0 && Data == 0)
                    return false;
                return true;
            }
        }
        public string Attribute { get; set; }
        public int Current { get; set; }
        public int Worst { get; set; }
        public int Threshold { get; set; }
        public int Data { get; set; }
        public bool IsOK { get; set; }

        public Smart()
        {

        }

        public Smart(string attributeName)
        {
            this.Attribute = attributeName;
        }
    }

    public class SmartProgram
    {
        public List<HDD> SmartFunction()
        {
            try
            {
                // retrieve list of drives on computer (this will return both HDD's and CDROM's and Virtual CDROM's)                    
                var dicDrives = new List<HDD>();

                var wdSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                // extract model and interface information
                foreach (ManagementObject drive in wdSearcher.Get())
                {
                    var hdd = new HDD();
                    hdd.Model = drive["Model"].ToString().Trim();
                    hdd.Type = drive["InterfaceType"].ToString().Trim();
                    hdd.DeviceId = drive["DeviceID"].ToString().Trim();
                    hdd.InstanceName = drive["PNPDeviceID"].ToString().Trim();
                    dicDrives.Add(hdd);
                }

                var pmsearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                // retrieve hdd serial number
                foreach (ManagementObject drive in pmsearcher.Get())
                {
                    var tag = drive["Tag"].ToString().Trim();

                    var dicDrive = dicDrives.FirstOrDefault(d => d.DeviceId.Equals(tag, StringComparison.InvariantCultureIgnoreCase));

                    if (dicDrive != null)
                        dicDrive.Serial = drive["SerialNumber"] == null ? "None" : drive["SerialNumber"].ToString().Trim();
                }

                // get wmi access to hdd 
                var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive");
                searcher.Scope = new ManagementScope(@"\root\wmi");

                // check if SMART reports the drive is failing
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictStatus");
                foreach (ManagementObject drive in searcher.Get())
                {
                    var instanceName = drive["InstanceName"].ToString().Trim();

                    var dicDrive = dicDrives.FirstOrDefault(d => instanceName.StartsWith(d.InstanceName, StringComparison.InvariantCultureIgnoreCase));

                    if (dicDrive != null)
                        dicDrive.IsOK = (bool)drive.Properties["PredictFailure"].Value == false;
                }

                // retrive attribute flags, value worste and vendor data information
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData");
                foreach (ManagementObject data in searcher.Get())
                {
                    var instanceName = data["InstanceName"].ToString().Trim();

                    var dicDrive = dicDrives.FirstOrDefault(d => instanceName.StartsWith(d.InstanceName, StringComparison.InvariantCultureIgnoreCase));

                    if (dicDrive == null)
                        continue;

                    Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {
                            int id = bytes[i * 12 + 2];

                            int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                                                            //bool advisory = (flags & 0x1) == 0x0;
                            bool failureImminent = (flags & 0x1) == 0x1;
                            //bool onlineDataCollection = (flags & 0x2) == 0x2;

                            int value = bytes[i * 12 + 5];
                            int worst = bytes[i * 12 + 6];
                            int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                            if (id == 0) continue;

                            var attr = dicDrive.Attributes[id];
                            attr.Current = value;
                            attr.Worst = worst;
                            attr.Data = vendordata;
                            attr.IsOK = failureImminent == false;
                        }
                        catch
                        {
                            // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                        }
                    }
                }

                // retreive threshold values foreach attribute
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictThresholds");

                foreach (ManagementObject data in searcher.Get())
                {
                    var instanceName = data["InstanceName"].ToString().Trim();

                    var dicDrive = dicDrives.FirstOrDefault(d => instanceName.StartsWith(d.InstanceName, StringComparison.InvariantCultureIgnoreCase));

                    if (dicDrive == null)
                        continue;

                    Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {

                            int id = bytes[i * 12 + 2];
                            int thresh = bytes[i * 12 + 3];
                            if (id == 0) continue;

                            var attr = dicDrive.Attributes[id];
                            attr.Threshold = thresh;
                        }
                        catch
                        {
                            // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                        }
                    }
                }

                return dicDrives;
            }

            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
                return null;
            }

        }

    }

}

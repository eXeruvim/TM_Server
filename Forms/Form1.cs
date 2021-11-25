using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tm_Server.Scripts;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Tm_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary <string, Data> answer;
        public bool dataGettingChecker = false;
        getCompDetails getComputerDetails = new getCompDetails();
        IFirebaseConfig config = new FirebaseConfig
        {
            //AuthSecret = "ing1e878IfUyLRkOKfBMQp2hUUStYjONU82dIrMc",
            AuthSecret = "ZqzbLab21zm5cRdktIHny4Cpmv1khMAIkc34SZXb",
            //BasePath = "https://store-computer-data-default-rtdb.europe-west1.firebasedatabase.app/",
            BasePath = "https://store-for-andrey-default-rtdb.europe-west1.firebasedatabase.app/",
        };

        IFirebaseClient client;

        public bool makeConnection()
        {
            client = new FireSharp.FirebaseClient(config);
            if (client == null)
            {
                MessageBox.Show("Connection to server not established!\nFatal error!");
                pictureBox3.Image = TaskManager_01.Properties.Resources.Basic_red_dot;
                return false;
            }

            return true;
        }

        public void btn_msg(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            getComputerData(btn.Name.ToString());
        }

        private async void getTablesName()
        {
            try
            {
                FirebaseResponse response = await client.GetTaskAsync("Information/");
                answer = response.ResultAs<Dictionary<string, Data>>();

                dataGettingChecker = true;
                pictureBox3.BackColor = Color.Transparent;
                pictureBox3.Image = TaskManager_01.Properties.Resources.green;

                List<Button> buttons = panel2.Controls.OfType<Button>().ToList();
                
                foreach (Button btn in buttons)
                {
                    btn.Click -= new EventHandler(this.btn_msg); //It's unnecessary
                    panel2.Controls.Remove(btn);
                    btn.Dispose();
                }

                foreach (var info in answer)
                {
                    Button computerBtn = new Button();
                    {
                        computerBtn.Name = string.Format("{0}", info.Key.ToString());
                        computerBtn.Text = string.Format("{0}", info.Key.ToString());
                        computerBtn.Font = new Font("Microsoft Sans Serif", 8);
                        computerBtn.Size = new Size(248, 40);
                        computerBtn.TextAlign = ContentAlignment.MiddleCenter;
                        computerBtn.ForeColor = Color.LightGray;

                        computerBtn.TabStop = false;
                        computerBtn.FlatStyle = FlatStyle.Flat;
                        computerBtn.FlatAppearance.BorderSize = 0;
                        computerBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);

                        computerBtn.Dock = DockStyle.Top;
                        computerBtn.FlatStyle = FlatStyle.Flat;


                        computerBtn.Click += btn_msg;
                        panel2.Controls.Add(computerBtn);
                    }
                }
            }
            
            catch
            {
                dataGettingChecker = false;
                pictureBox3.Image = TaskManager_01.Properties.Resources.Basic_red_dot;
                pictureBox3.BackColor = Color.Transparent;
            }
        
        }

        private void getComputerData(string cName)
        {
            if (answer.ContainsKey(cName))
            {
                var data = answer[cName];
                //MessageBox.Show(cName);
                label2.Text = data.DeviceName.ToString();
                label10.Text = data.UserName.ToString();
                label11.Text = data.CpuName.ToString();
                //label12.Text = data.WindowsXVer.ToString();
                label13.Text = data.Ram.ToString();
                label14.Text = data.GpuName.ToString();
                //label15.Text = data.DiskInSystem.ToString();
                label9.Text = data.DateAndTime.ToString();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
//            SideMenuPanel.BackColor = Color.FromArgb(11, 7, 17);
  //          ButtonPanelSide.BackColor = Color.FromArgb(35, 32, 39);
    //        AboutPanel.BackColor = Color.FromArgb(23, 21, 32);

            bool check = makeConnection();

            if (check)
            {
                getTablesName();
                timer.Start();
            }

            customizeDesign(); 
        }

        // Menu 
        private void customizeDesign()
        {
            ButtonPanelSide.Visible = false;
        }
        
        private void hideSubMenu()
        {
            if (ButtonPanelSide.Visible == true)
            {
                ButtonPanelSide.Visible = false;
            }
        }

        private void clearScreen()
        {
            if (label9.Text != "None")
            {
                label2.Text = "None";
                label10.Text = "None";
                label11.Text = "None";
                label12.Text = "None";
                label13.Text = "None";
                label14.Text = "None";
                label15.Text = "None";
                label9.Text = "None";
            }

        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            } 
            
            else {
                subMenu.Visible = false;
            }
        }

        private void MediaBtn_Click(object sender, EventArgs e)
        {
            showSubMenu(ButtonPanelSide);
        }

        private void AboutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            clearScreen();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            getTablesName();
            if (dataGettingChecker == false)
            {
                timer.Interval = 200000;
                MessageBox.Show("Can't connect to the server.");

            }
            else
            {
                timer.Interval = 10000;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    internal class Data
    {
        public string DeviceName { get; set; }
        public string CpuName { get; set; }
        public string Ram { get; set; }
        public string GpuName { get; set; }
        public string DateAndTime { get; set; }
        //public string CpuUsage { get; set; }
        public string UserName { get; set; }
    }
}

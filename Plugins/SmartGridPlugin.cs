using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MissionPlanner.Plugin;
using MissionPlanner;
using System.Windows.Forms;

    public class PluginSmartGrid : Plugin
    {
        string _Name = "Plugin SmartGrid"; 
        string _Version = "0.1";
        string _Author = "Ethan Carrés";

        public override string Name { get { return _Name; } }
        public override string Version { get { return _Version; } }
        public override string Author { get { return _Author; } }

        ToolStripMenuItem but = new ToolStripMenuItem("SmartGrid by Hemav");

        public override bool Init() { loopratehz = 0.1f; return true; }

        public override bool Loaded() 
        {
            bool hit = false;
            ToolStripItemCollection col = Host.FPMenuMap.Items;
            int index = col.Count;
            foreach (ToolStripItem item in col)
            {
                if (item.Text.Equals("Auto WP"))
                {
                    index = col.IndexOf(item);
                    ((ToolStripMenuItem)item).DropDownItems.Add(but);
                    hit = true;
                    break;
                }
            }

            if (hit == false)
                col.Add(but);

             but.Click+=but_Click;

            return true; 
        }

        void but_Click(object sender, EventArgs e)
        {
            SetupUI(0);
        }

        public bool SetupUI(int gui = 0) 
        {
            if (gui == 0)
            {
                PluginTest.SmartPluginConfigurador form = new PluginTest.SmartPluginConfigurador();
                form.Show();
            }

            return true; 
        }

        public override bool Loop()
        { 
            Console.WriteLine("Plugin Loop - {0}", NextRun);

            Console.WriteLine("Currrent Pos {0} {1} {2}", Host.cs.lat, Host.cs.lng, Host.cs.altasl);

            return true; 
        }


        public override bool Exit() { return true; }
    }


using System;
using System.ComponentModel;
using System.Windows.Forms;
using cleanCore;
using WhiteMagic.Internals;

namespace TeleportBook
{
    public partial class Form1 : Form
    {
        public BindingList<BookItem> Items = new BindingList<BookItem>();
        private readonly Detour _ctmDetour;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Items;
            _ctmDetour = Helper.Magic.Detours.Create(WoWLocalPlayer.ClickToMoveFunction, new WoWLocalPlayer.ClickToMoveDelegate(HandleClickToMove),
                                                     "CTMTeleport");
        }

        private int HandleClickToMove(IntPtr thisPointer, int clickType, ref ulong interactGuid, ref Location clickLocation, float precision)
        {
            if (Teleporter.Destination != null)
                return 0;

            if (clickType == 4)
            {
                Teleporter.SetDestination(clickLocation);
                return 0;
            }

            return (int)_ctmDetour.CallOriginal(thisPointer, clickType, interactGuid, clickLocation, precision);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Manager.LocalPlayer == null)
                return;

            var loc = Manager.LocalPlayer.Location;
            Items.Add(new BookItem { Name = "Player", X = loc.X, Y = loc.Y, Z = loc.Z });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Manager.LocalPlayer == null || Manager.LocalPlayer.TargetGuid == 0 || Manager.LocalPlayer.Target == null)
                return;

            var loc = Manager.LocalPlayer.Target.Location;
            Items.Add(new BookItem{Name = Manager.LocalPlayer.Target.Name, X = loc.X, Y = loc.Y, Z = loc.Z});
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            if (Teleporter.Destination != null)
                return;

            var row = dataGridView1.SelectedRows[0];
            if (row.DataBoundItem != null)
            {
                var bookitem = (BookItem) row.DataBoundItem;
                Teleporter.SetDestination(new Location(bookitem.X, bookitem.Y, bookitem.Z));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                _ctmDetour.Apply();
            }
            else
                _ctmDetour.Remove();
        }
    }

    public class BookItem
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

}

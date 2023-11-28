using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logistica
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //this.AllowTransparency = true;
            /*BackColor = Color.Azure;
            TransparencyKey = Color.Azure;*/
            WindowUtils.EnableAcrylic(this, Color.Transparent);
            //base.OnHandleCreated(e);
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
        }

        private void a(object sender, PaintEventArgs e)
        {
            ((Label)sender).BackColor = Color.Black;
        }
    }
}

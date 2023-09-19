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
    public partial class Form1 : Form
    {
        public Form1() 
        {
            InitializeComponent();
            tabellaIniziale.Font = new Font("Arial", 10);
            aggiornaRigheTabella(tabellaIniziale, 1);
        }

        private void textBoxRighe_TextChanged(object sender, EventArgs e)
        {
            aggiornaRigheTabella(tabellaIniziale, Int32.Parse(((NumericUpDown)sender).Text));
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            aggiornaColonneTabella(tabellaIniziale, Int32.Parse(((NumericUpDown)sender).Text));
        }

        void aggiornaRigheTabella(DataGridView tabella, int numero)
        {
            tabella.RowCount = numero + 2;
        }
        void aggiornaColonneTabella(DataGridView tabella, int numero)
        {
            tabella.ColumnCount = numero + 2;
        }
    }
}

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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            aggiornaRigheTabella(tabellaIniziale, Int32.Parse(((NumericUpDown)sender).Value.ToString()));
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            aggiornaColonneTabella(tabellaIniziale, Int32.Parse(((NumericUpDown)sender).Value.ToString()));
        }

        void aggiornaRigheTabella(DataGridView tabella, int numero)
        {
            if (presenzaTesto(tabella) && numero < tabella.RowCount)
            {
                    string message = "Ridimensionare la tabella potrebbe cancellare parzialmente o totalmente i dati contenuti. Continuare?";
                    string caption = "La tabella non è vuota";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
            }
            tabella.RowCount = numero +1;
            generaTesto(tabella);
        }

        void aggiornaColonneTabella(DataGridView tabella, int numero)
        {
            if (presenzaTesto(tabella) && numero < tabella.ColumnCount)
            {
                string message = "Ridimensionare la tabella potrebbe cancellare parzialmente o totalmente i dati contenuti. Continuare?";
                string caption = "La tabella non è vuota";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            tabella.ColumnCount = numero +1;
            generaTesto(tabella);
        }

        void generaTesto(DataGridView tabella)
        {
            for (int righe = 0; righe < tabella.Rows.Count; righe++)
            {
                tabella.RowHeadersWidth = 200;
                if (righe == tabella.RowCount - 1)
                {
                    tabella.Rows[righe].HeaderCell.Value = "Totale";
                }
                else
                {
                    tabella.Rows[righe].HeaderCell.Value = "Unità di produzione " + (righe + 1);
                }
            }
            for (int colonne = 0; colonne < tabella.Columns.Count; colonne++)
            {
                tabella.Columns[colonne].Width = 110;
                if(colonne == tabella.ColumnCount - 1)
                {
                    tabella.Columns[colonne].HeaderCell.Value = "Totale";
                }
                else
                {
                    tabella.Columns[colonne].HeaderCell.Value = "Destinazione " + (colonne + 1);
                }
            }
        }

        private bool presenzaTesto(DataGridView tabella)
        {
            for (int righe = 0; righe < tabella.Rows.Count; righe++)
            {
                for (int colonne = 0; colonne < tabella.Columns.Count; colonne++)
                {
                    if(tabella.Rows[righe].Cells[colonne].Value != null && (tabella.Rows[righe].Cells[colonne].Value.ToString() != "" || tabella.Rows[righe].Cells[colonne].Value.ToString() != null))
                    {
                        Console.Out.WriteLine(tabella.Rows[righe].Cells[colonne].Value + " non è vuoto");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

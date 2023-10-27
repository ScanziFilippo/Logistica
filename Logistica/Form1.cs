using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
            this.AllowTransparency = false;
            /*BackColor = Color.Azure;
            TransparencyKey = Color.Azure;*/
            tabellaIniziale.Font = new Font("Arial", 10);
            tabellaIniziale.ForeColor = Color.Black;
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
                    result = MessageBox.Show(message, caption, buttons);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        numericUpDown1.Value = tabella.RowCount-1;
                        return;
                    }
            }
            int xVecchie = tabella.ColumnCount;
            int yVecchie = tabella.RowCount;
            tabella.RowCount = numero +1;
            generaTesto(tabella);
            pulisciCellaTotale(tabella, yVecchie, xVecchie);
            tabella.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
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
                    numericUpDown2.Value = tabella.ColumnCount-1;
                    return;
                }
            }
            int xVecchie = tabella.ColumnCount;
            int yVecchie = tabella.RowCount;
            tabella.ColumnCount = numero +1;
            generaTesto(tabella);
            pulisciCellaTotale(tabella, yVecchie, xVecchie);
            tabella.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
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
                tabella.Columns[colonne].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (colonne == tabella.ColumnCount - 1)
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
                    if(righe == tabella.RowCount - 1 && colonne == tabella.ColumnCount - 1)
                    {
                        Console.Out.WriteLine("Vuota");
                        return false;
                    }
                    if(tabella.Rows[righe].Cells[colonne].Value != "" &&tabella.Rows[righe].Cells[colonne].Value != null && (tabella.Rows[righe].Cells[colonne].Value.ToString() != "" || tabella.Rows[righe].Cells[colonne].Value.ToString() != null))
                    {
                        Console.Out.WriteLine("non vuota perchè " + righe + " " + colonne);
                        return true;
                    }
                }
            }
            return false;
        }

        private void tabellaIniziale_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            calcolaTotale((DataGridView)sender);
        }

        private void calcolaTotale(DataGridView tabella)
        {
            if(tabella.ColumnCount > 0 && tabella.RowCount > 0)
            {
                //Console.Out.WriteLine("sc");
                int sommaOrizzontale = 0;
                for (int colonne = 0; colonne < tabella.ColumnCount - 1; colonne++)
                {
                    if (tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value != "" && tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value != null && tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value.ToString().All(char.IsDigit))
                    {
                        sommaOrizzontale += Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value.ToString());
                    }
                }
                int sommaVerticale = 0;
                for (int righe = 0; righe < tabella.RowCount - 1; righe++)
                {
                    if (tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value != "" && tabella.Rows[righe].Cells[tabella.ColumnCount-1].Value != null && tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value.ToString().All(char.IsDigit))
                    {
                        sommaVerticale += Int32.Parse(tabella.Rows[righe].Cells[tabella.ColumnCount-1].Value.ToString());
                    }
                }
                if(sommaOrizzontale != sommaVerticale)
                {
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Value = "######";
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Style.ForeColor = Color.Red;
                }
                else
                {
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Value = sommaOrizzontale;
                    //tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Style.Font = new Font("Arial", 10);
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Style.ForeColor = Color.Black;
                }
            }
        }
        private void pulisciCellaTotale(DataGridView tabella, int yVecchie, int xVecchie)
        {
            if ((yVecchie <= tabella.RowCount && xVecchie <= tabella.ColumnCount) && yVecchie > 0 && xVecchie > 0)
            {
                //Console.Out.WriteLine("Cancello " + xVecchie + " " + yVecchie);
                tabella.Rows[yVecchie - 1].Cells[xVecchie - 1].Value = "";
                tabella.Rows[yVecchie - 1].Cells[xVecchie - 1].Style.ForeColor = Color.Black;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generaContenutoCelle(tabellaIniziale);
        }
        private void generaContenutoCelle(DataGridView tabella)
        {
            int minimo = Int32.Parse(numericUpDown3.Value.ToString());
            int massimo = Int32.Parse(numericUpDown4.Value.ToString());
            Random casuale= new Random();
            for (int righe = 0; righe < tabella.Rows.Count-1; righe++)
            {
                for (int colonne = 0; colonne < tabella.Columns.Count-1; colonne++)
                {
                    tabella.Rows[righe].Cells[colonne].Value = casuale.Next(minimo, massimo+1);
                }
            }
        }
        private void generaTotali()
        {

        }
    }
}

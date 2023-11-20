﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            aggiornaRigheTabella(tabellaIniziale, 3);
            aggiornaColonneTabella(tabellaIniziale, 3);
            
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
            //tabella.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
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
            //tabella.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
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
                
                
                if(sommaOrizzontale(tabella) != sommaVerticale(tabella))
                {
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Value = "######";
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Style.ForeColor = Color.Red;
                }
                else
                {
                    tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 1].Value = sommaOrizzontale(tabella);
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
        private void generaTotali(DataGridView tabella)
        {
            int minimo = Int32.Parse(numericUpDown6.Value.ToString());
            int massimo = Int32.Parse(numericUpDown5.Value.ToString());
            //int totale = new Random().Next(minimo, massimo + 1);
            Random casuale = new Random();
            //Console.Out.WriteLine(totale.ToString());
            for (int righe = 0; righe < tabella.Rows.Count - 1; righe++)
            {
                tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value = casuale.Next(minimo, massimo + 1);
            }
            for (int colonne = 0; colonne < tabella.Columns.Count - 1; colonne++)
            {
                tabella.Rows[tabella.RowCount-1].Cells[colonne].Value = casuale.Next(minimo, massimo + 1);
            }
            int differenza = sommaVerticale(tabella) - sommaOrizzontale(tabella);
            //da fare if per sapere quale piu grande e decidere dove mettere il "fittizio"
            if (differenza > 0)
            {
                tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 2].Value = Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[tabella.ColumnCount - 2].Value.ToString()) + sommaVerticale(tabella) - sommaOrizzontale(tabella);
            }
            else if(differenza < 0)
            {
                tabella.Rows[tabella.RowCount - 2].Cells[tabella.ColumnCount - 1].Value = Int32.Parse(tabella.Rows[tabella.RowCount - 2].Cells[tabella.ColumnCount - 1].Value.ToString()) + sommaOrizzontale(tabella) - sommaVerticale(tabella);
            }
            Console.WriteLine("SO: " + sommaOrizzontale(tabella) + " SV: " + sommaVerticale(tabella));

            /*for (int righe = 0; righe < tabella.Rows.Count - 1; righe++)
            {
                int prossimo = casuale.Next(0, totale + 1);
                tabella.Rows[righe].Cells[tabella.ColumnCount-1].Value = prossimo;
                totale -= prossimo;
            }
            /*for (int colonne = 0; colonne < tabella.Columns.Count - 1; colonne++)
            {
                tabella.Rows[righe].Cells[colonne].Value = casuale.Next(minimo, massimo + 1);
            }*/
        }

        private int sommaOrizzontale(DataGridView tabella)
        {
            int sommaOrizzontale = 0;
            for (int colonne = 0; colonne < tabella.ColumnCount - 1; colonne++)
            {
                if (tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value != "" && tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value != null && tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value.ToString().All(char.IsDigit))
                {
                    sommaOrizzontale += Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value.ToString());
                }
            }
            return sommaOrizzontale;
        }
        private int sommaVerticale(DataGridView tabella)
        {
            int sommaVerticale = 0;
            for (int righe = 0; righe < tabella.RowCount - 1; righe++)
            {
                if (tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value != "" && tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value != null && tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value.ToString().All(char.IsDigit))
                {
                    sommaVerticale += Int32.Parse(tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value.ToString());
                }
            }
            return sommaVerticale;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            generaTotali(tabellaIniziale);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            eliminaContenutoCelle(tabellaIniziale);
        }

        private void eliminaContenutoCelle(DataGridView tabella)
        {
            for (int righe = 0; righe < tabella.Rows.Count - 1; righe++)
            {
                for (int colonne = 0; colonne < tabella.Columns.Count - 1; colonne++)
                {
                    tabella.Rows[righe].Cells[colonne].Value = "";
                }
            }
        }

        private void eliminaTotali(DataGridView tabella)
        {
            for (int righe = 0; righe < tabella.Rows.Count - 1; righe++)
            {
                tabella.Rows[righe].Cells[tabella.ColumnCount - 1].Value = "";
            }
            for (int colonne = 0; colonne < tabella.Columns.Count - 1; colonne++)
            {
                tabella.Rows[tabella.RowCount - 1].Cells[colonne].Value = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            eliminaTotali(tabellaIniziale);
        }

        DataGridView duplicaTabella(DataGridView tabella)
        {
            DataGridView tabellaNuova = new DataGridView();
            // Copia le colonne dalla DataGridView originale a quella duplicata
            foreach (DataGridViewColumn col in tabella.Columns)
            {
                tabellaNuova.Columns.Add(col.Clone() as DataGridViewColumn);
            }
            //tabellaNuova.Rows.RemoveAt(tabellaNuova.Columns.Count-1);

            // Copia i dati dalla DataGridView originale a quella duplicata
            foreach (DataGridViewRow row in tabella.Rows)
            {
                int rowIndex = tabellaNuova.Rows.Add(row.Clone() as DataGridViewRow);

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    tabellaNuova.Rows[rowIndex].Cells[i].Value = row.Cells[i].Value;
                }
            }
            tabellaNuova.AllowUserToAddRows = false;
            tabellaNuova.AllowUserToDeleteRows = false;
            //tabellaNuova.Rows.RemoveAt(tabellaNuova.Columns.Count - 1);
            tabellaNuova.Location = tabella.Location;
            tabellaNuova.Margin = tabella.Margin;
            tabellaNuova.Size = tabella.Size;
            tabellaNuova.Dock = DockStyle.Fill;
            tabellaNuova.Font = tabella.Font;
            tabellaNuova.ForeColor = tabella.ForeColor;
            tabellaNuova.GridColor = tabella.GridColor;
            tabellaNuova.BackgroundColor = tabella.BackgroundColor;
            generaTesto(tabellaNuova);
            tabellaNuova.DefaultCellStyle = tabella.DefaultCellStyle;
            tabellaNuova.RowHeadersDefaultCellStyle = tabella.RowHeadersDefaultCellStyle;
            tabellaNuova.ColumnHeadersDefaultCellStyle = tabella.ColumnHeadersDefaultCellStyle;
            tabellaNuova.ColumnHeadersHeight = tabella.ColumnHeadersHeight;

            return tabellaNuova;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            terminale.BackColor = Color.White;
            terminale.Text="";
            if (tabControl1.TabCount > 1)
            {
                tabControl1.TabPages.RemoveAt(1);
                tabControl1.TabPages.RemoveAt(1);
            }
            tabControl1.TabPages.Add(new TabPage("Nord-Ovest"));
            tabControl1.TabPages.Add(new TabPage("Minimi Costi"));
            tabControl1.TabPages[1].Padding = tabControl1.TabPages[0].Padding;
            tabControl1.TabPages[2].Padding = tabControl1.TabPages[0].Padding;
            DataGridView coc = duplicaTabella(tabellaIniziale);
            DataGridView coc2 = duplicaTabella(tabellaIniziale);
            tabControl1.TabPages[1].Controls.Add(coc);
            tabControl1.TabPages[2].Controls.Add(duplicaTabella(tabellaIniziale));
            metodoNordOvest(coc);
            //Thread.Sleep(2000);
            metodoMinimiCosti(coc2);
            tabControl1.SelectedIndex = 0;
        }

        private void metodoNordOvest(DataGridView tabella)
        {
            tabControl1.SelectedIndex = 1;
            scriviSulTerminale("- - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            scriviSulTerminale("Inizializzando metodo Nord-Ovest\n");
            while(tabella.ColumnCount > 1 && tabella.RowCount > 1)
            {
                consegnaDaA(0, 0, tabella);
            }
            scriviSulTerminale("Terminato");
        }
        private void metodoMinimiCosti(DataGridView tabella)
        {
            tabControl1.SelectedIndex = 2;
            scriviSulTerminale("- - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            scriviSulTerminale("Inizializzando metodo Minimi Costi");
            scriviSulTerminale("Terminato");
        }

        private void scriviSulTerminale(String testo)
        {
            terminale.Text += "\t" + testo + "\n";
            terminale.Refresh();
        }

        private void consegnaDaA(int indiceUP, int indiceD, DataGridView tabella)
        {
            if(tabella.RowCount == 1 || tabella.ColumnCount == 1)
            {
                return;
            }
            if(Int32.Parse(tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value.ToString()) > Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString()))
            {
                scriviSulTerminale("Consegno " + tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value + " scorte da " + tabella.Rows[indiceUP].HeaderCell.Value + " a " + tabella.Columns[indiceD].HeaderCell.Value + "\n");
                tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value = Int32.Parse(tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value.ToString()) - Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString());
                tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value = 0;
                mostraMossa(1000, tabella);
                scriviSulTerminale(tabella.Columns[indiceD].HeaderText + " rimossa" + "\n");
                tabella.Columns.RemoveAt(indiceD);
                mostraMossa(1000, tabella);
            }
            else if(Int32.Parse(tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value.ToString()) <= Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString()))
            {
                scriviSulTerminale("Consegno " + tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value + " scorte da " + tabella.Rows[indiceUP].HeaderCell.Value + " a " + tabella.Columns[indiceD].HeaderCell.Value + "\n");
                tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value = - Int32.Parse(tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value.ToString()) + Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString());
                tabella.Rows[indiceUP].Cells[tabella.ColumnCount - 1].Value = 0;
                mostraMossa(1000, tabella);
                if (Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString()) != 0)
                {
                    scriviSulTerminale(tabella.Rows[indiceUP].HeaderCell.Value + " rimossa\n");
                    tabella.Rows.RemoveAt(indiceUP);
                    mostraMossa(1000, tabella);
                }
                else
                {
                    scriviSulTerminale(tabella.Rows[indiceUP].HeaderCell.Value + " rimossa");
                    tabella.Rows.RemoveAt(indiceUP);
                }
                Console.Out.WriteLine(tabella.RowCount + " " + tabella.ColumnCount + " riochiesto " + (tabella.RowCount - 1) + " " + indiceD);
                if(Int32.Parse(tabella.Rows[tabella.RowCount - 1].Cells[indiceD].Value.ToString()) == 0)
                {
                    scriviSulTerminale(tabella.Columns[indiceD].HeaderText + " rimossa\n");
                    tabella.Columns.RemoveAt(indiceD);
                    mostraMossa(1000, tabella);
                }
            }
        }

        private void mostraMossa(int millisecondi, DataGridView tabella)
        {
            Thread.Sleep(millisecondi);
            tabella.Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gebirgszug_Simulation
{
    public partial class N_Eingabe : Form
    {
        public N_Eingabe()
        {
            InitializeComponent();
        }        

        private void Übernehmen_Button_Click(object sender, EventArgs e)
        {
            //Wenn der Übernehmen_Button geklickt wird, überträgt die Form den Wert des Nummernfeldes in die Variable N
            Gebirgszug_Simulation_Allgemein.N = (int)N_Eingabe_Feld.Value;
            //Der Bit N_Eingabe_Wurde_Geöffnet wird noch vor dem Schließen der Form wieder auf false gesetzt
            Gebirgszug_Simulation_Allgemein.N_Eingabe_Wurde_Geöffnet = false;
            this.Close();
        } 
    }
}
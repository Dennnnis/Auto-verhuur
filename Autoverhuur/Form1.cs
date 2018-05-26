using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Autoverhuur
{



    public partial class Form1 : Form
    {
        int dagen = 1;

        Dictionary<string, CAuto> autos = new Dictionary<string, CAuto>();

        public decimal Afronding(decimal bedrag) => Math.Round(bedrag, 2);
     
        public string EuroTekenEnNull(decimal bedrag)
        {
            if (bedrag == 0)
                return "€ 0,00";

            string stringBedrag = bedrag.ToString();

            while (stringBedrag.Length < Math.Round(bedrag).ToString().Length + 3)
            {
                stringBedrag += "0";
            }

            stringBedrag = stringBedrag.Insert(0, "€ ");

            return stringBedrag;
        }

        public class CAuto
        {
            //Weet niet hoe ik deze 2 moet pakken van Form1 zonder inheritance
            public decimal Afronding(decimal bedrag) => Math.Round(bedrag, 2);
            public string EuroTekenEnNull(decimal bedrag)
            {
                if (bedrag == 0)
                    return "€ 0,00";

                string stringBedrag = bedrag.ToString();


                while (stringBedrag.Length < Math.Round(bedrag).ToString().Length + 3)
                {
                    stringBedrag += "0";
                }

                stringBedrag = stringBedrag.Insert(0, "€ ");

                return stringBedrag;
            }

            public decimal kilometerPrijs = 0.0m;
            public decimal dagtarief = 0.0m;
            public decimal kilometerVrij = 0.0m;

            public new string ToString()
            {
                return "Kilometer prijs: " + (EuroTekenEnNull(Afronding(kilometerPrijs))) + "\n" +
                       "Dagtarief: " + (EuroTekenEnNull(Afronding(dagtarief))) + "\n" +
                       "Eerste " + Math.Round(kilometerVrij) + " km vrij";
                
            }
        }

        public Form1()
        {
            InitializeComponent();
            monthCalendar1.MinDate = DateTime.Today;
            
            autos.Add("Personenauto", new CAuto() { kilometerPrijs = 0.20m, dagtarief = 50.0m, kilometerVrij = 100.0m });
            autos.Add("Personenbusje", new CAuto() { kilometerPrijs = 0.30m, dagtarief = 95.0m, kilometerVrij = 100.0m });
            autos.Add("Vrachtwagen", new CAuto() { kilometerPrijs = 0.50m, dagtarief = 120.0m, kilometerVrij = 200.0m });
            autos.Add("Limousine", new CAuto() { kilometerPrijs = 1.50m, dagtarief = 500.0m, kilometerVrij = 0.0m });
            autos.Add("Ferrari", new CAuto() { kilometerPrijs = 2.00m, dagtarief = 1200.0m, kilometerVrij = 0.0m });

            foreach (string name in autos.Keys)
            {
                comboBox1.Items.Add(name);
            }

            comboBox1.SelectedIndex = 0;
        }

        private void Bereken()
        {

            CAuto bAuto = autos[comboBox1.SelectedItem.ToString()];

            decimal geredenKilometers;

            try
            {
                geredenKilometers = Convert.ToDecimal(textBox3.Text);
            }
            catch(Exception)
            {
                geredenKilometers = 0.0m;
            }
            
            
            decimal geredenKilometerPrijs = Math.Max(0,(geredenKilometers - bAuto.kilometerVrij) * bAuto.kilometerPrijs);
            decimal dagtariefPrijs = dagen * bAuto.dagtarief;
            decimal benzinePrijs;

            try
            {
                benzinePrijs = Convert.ToDecimal(textBox4.Text);
            }
            catch(Exception)
            {
                benzinePrijs = 0.0m;
            }

            label7.Text =
                "Berekening: \n" +
                "____________________________\n" +
                EuroTekenEnNull(Afronding(geredenKilometerPrijs)) + " Voor gereden kilometers\n" +
                EuroTekenEnNull(Afronding(dagtariefPrijs)) + " Dagtarief\n" +
                EuroTekenEnNull(Afronding(benzinePrijs)) + " Benzine kosten\n" +
                "____________________________\n" +
                "Prijs totaal: " + EuroTekenEnNull(Afronding(dagtariefPrijs + geredenKilometerPrijs + benzinePrijs));




        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = monthCalendar1.SelectionStart.ToShortDateString();
            textBox2.Text = monthCalendar1.SelectionEnd.ToShortDateString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (autos.ContainsKey(comboBox1.SelectedItem.ToString()))
                label6.Text = autos[comboBox1.SelectedItem.ToString()].ToString();
            Bereken();
        }

        private void TekstDoosVerandered()
        {
            Bereken();
            try
            {
                Convert.ToDateTime(textBox1.Text);
                Convert.ToDateTime(textBox2.Text);
            }
            catch (System.FormatException)
            {
                label10.Visible = true;
                return;
            }
            label10.Visible = false;
            dagen = Math.Max(0,(Convert.ToDateTime(textBox2.Text) - Convert.ToDateTime(textBox1.Text)).Days) + 1;
                label8.Text = (dagen.ToString() + " Dag(en)");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TekstDoosVerandered();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TekstDoosVerandered();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!textBox4.Text.Contains(","))
            {
                textBox4.Text += ",00";
            }
            Bereken();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bereken();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Bereken();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;
using System.Globalization;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections;

namespace projekt1
{
    public partial class Form1 : Form
    {

        int counter = 0;
        ushort AX;
        ushort BX;
        ushort CX;
        ushort DX;
        ushort[] pamiec = new ushort [100000];
        enum Rejestry { AX, BX, CX, DX};
        enum Czesc { Calosc, Gorna, Dolna};

        public Form1()
        {
            InitializeComponent();
            label9.Text = "Możliwe przerwania: \n1: HDD;\n2:PrintScreen;\n3:PodajCzasZegaraRTC;\n4:OdczytPozycjiKursora;\n5:ZmienRozmairKursora;\n6:StworzNowyFolder;\n7:UtworzPlik;\n8:ZmienNazwePliku;\n9:UsunPlik;";

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Metoda(counter);
            counter++;
        }

        private int OdczytajWartosc(rejestrIJegoCzesc argument)
        {
            int zmienna = 0;
            if (argument.Liczba == true)
            {
                zmienna = argument.x;
            }
            else
            {
                switch (argument.nazwaRejestru)
                {
                    case Rejestry.AX:
                        zmienna = AX;
                        break;
                    case Rejestry.BX:
                        zmienna = BX;
                        break;
                    case Rejestry.CX:
                        zmienna = CX;
                        break;
                    case Rejestry.DX:
                        zmienna = DX;
                        break;

                }
                switch (argument.czescRejestru)
                {
                    case Czesc.Calosc:
                        break;
                    case Czesc.Dolna:
                        zmienna = zmienna & 0xFF;
                        break;
                    case Czesc.Gorna:
                        zmienna = (zmienna & 0xFF) << 8;
                        break;
                }
            }

            return zmienna;
        }

        private static rejestrIJegoCzesc CzytajRejestr(string split)
        {
            rejestrIJegoCzesc nowyRejestr = new rejestrIJegoCzesc();
            if (split[0] == '[' && split[split.Length - 1] == ']')
            {
                split = split.Substring(1, split.Length - 2);
                nowyRejestr.Adres = true;
            }
            else nowyRejestr.Adres = false;
            

            switch (split)
            {
                case "AX":
                    nowyRejestr.nazwaRejestru = Rejestry.AX;
                    nowyRejestr.czescRejestru = Czesc.Calosc;
                    break;
                case "AH":
                    nowyRejestr.nazwaRejestru = Rejestry.AX;
                    nowyRejestr.czescRejestru = Czesc.Gorna;
                    break;
                case "AL":
                    nowyRejestr.nazwaRejestru = Rejestry.AX;
                    nowyRejestr.czescRejestru = Czesc.Dolna;
                    break;
                case "BX":
                    nowyRejestr.nazwaRejestru = Rejestry.BX;
                    nowyRejestr.czescRejestru = Czesc.Calosc;
                    break;
                case "BH":
                    nowyRejestr.nazwaRejestru = Rejestry.BX;
                    nowyRejestr.czescRejestru = Czesc.Gorna;
                    break;
                case "BL":
                    nowyRejestr.nazwaRejestru = Rejestry.BX;
                    nowyRejestr.czescRejestru = Czesc.Dolna;
                    break;
                case "CX":
                    nowyRejestr.nazwaRejestru = Rejestry.CX;
                    nowyRejestr.czescRejestru = Czesc.Calosc;
                    break;
                case "CH":
                    nowyRejestr.nazwaRejestru = Rejestry.CX;
                    nowyRejestr.czescRejestru = Czesc.Gorna;
                    break;
                case "CL":
                    nowyRejestr.nazwaRejestru = Rejestry.CX;
                    nowyRejestr.czescRejestru = Czesc.Dolna;
                    break;
                case "DX":
                    nowyRejestr.nazwaRejestru = Rejestry.DX;
                    nowyRejestr.czescRejestru = Czesc.Calosc;
                    break;
                case "DH":
                    nowyRejestr.nazwaRejestru = Rejestry.DX;
                    nowyRejestr.czescRejestru = Czesc.Gorna;
                    break;
                case "DL":
                    nowyRejestr.nazwaRejestru = Rejestry.DX;
                    nowyRejestr.czescRejestru = Czesc.Dolna;
                    break;
                default:
                    nowyRejestr.x = Int32.Parse(split);
                    nowyRejestr.Liczba = true;
                    break;
            }
            return nowyRejestr;
        }

        private class rejestrIJegoCzesc
        {
            public Rejestry nazwaRejestru;
            public Czesc czescRejestru;
            public bool Adres=false;
            public bool Liczba = false;
            public int x;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Metoda(int a)
        {
            
            string[] textBox = richTextBox1.Lines;
            if (textBox.Length != 0 && a<textBox.Length)
            {

                string theText = textBox[a].ToUpper();
                string[] split = theText.Split(new Char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);


                rejestrIJegoCzesc czescLewa = new rejestrIJegoCzesc(); 
                if (split.Length > 1) czescLewa = CzytajRejestr(split[1]);
                rejestrIJegoCzesc czescPrawa = new rejestrIJegoCzesc();
                if (split.Length > 2) czescPrawa = CzytajRejestr(split[2]);
                int zmienna = 0;
                int zmienna2 = 0;

                switch (split[0])
                {
                    case "INT":
                        if (!czescLewa.Liczba)
                        {
                            return;
                        }
                        switch(czescLewa.x)
                        {
                            case 1:
                                HDD();
                                break;
                            case 2:
                                PrintScreen();
                                break;
                            case 3:
                                PodajCzasZegaraRTC();
                                break;
                            case 4:
                                OdczytPozycjiKursora();
                                break;
                            case 5:
                                ZmienRozmairKursora();
                                break;
                            case 6:
                                StworzNowyFolder();
                                break;
                            case 7:
                                UtworzPlik();
                                break;
                            case 8:
                                ZmienNazwePliku();
                                break;
                            case 9:
                                UsunPlik();
                                break;
                        }
                        break;
                    case "ADD":
                        if (split.Length < 3)
                        {
                            return;
                        }
                        zmienna = OdczytajWartosc(czescPrawa);
                        if (czescPrawa.Adres == true)
                        {
                            zmienna = pamiec[zmienna];
                        }

                        zmienna2 = 0;
                        if (czescLewa.Adres == true)
                        {
                            zmienna2 = OdczytajWartosc(czescLewa);
                            pamiec[zmienna2] = (ushort)zmienna;
                        }
                        else
                        {
                            if (czescLewa.Liczba == true)
                            {
                                MessageBox.Show("blad");
                            }
                            else
                            {
                                switch (czescLewa.nazwaRejestru)
                                {
                                    case Rejestry.AX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                AX = (ushort)(AX + ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((AX & 0xFF) + ((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF00);
                                                AX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((AX & 0xFF00) >> 8) + ((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF);
                                                AX |= (ushort)((gora) << 8);
                                                break;


                                        }
                                        break;
                                    case Rejestry.BX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                BX = (ushort)(BX + ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((BX & 0xFF) + ((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF00);
                                                BX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((BX & 0xFF00) >> 8) + ((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF);
                                                BX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.CX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                CX = (ushort)(CX + ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((CX & 0xFF) + ((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF00);
                                                CX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((CX & 0xFF00) >> 8) + ((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF);
                                                CX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.DX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                DX = (ushort)(DX + ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((DX & 0xFF) + ((ushort)zmienna & 0xFF));
                                                DX = (ushort)(DX & 0xFF00);
                                                DX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((DX & 0xFF00) >> 8) + ((ushort)zmienna & 0xFF));
                                                DX = (ushort)(DX & 0xFF);
                                                DX |= (ushort)((gora) << 8);
                                                break;

                                        }

                                        break;
                                }
                            }
                        }

                        break;
                    case "SUB":
                        if (split.Length < 3)
                        {
                            return;
                        }
                        zmienna = OdczytajWartosc(czescPrawa);
                        if (czescPrawa.Adres == true)
                        {
                            zmienna = pamiec[zmienna];
                        }

                        zmienna2 = 0;
                        if (czescLewa.Adres == true)
                        {
                            zmienna2 = OdczytajWartosc(czescLewa);
                            pamiec[zmienna2] = (ushort)zmienna;
                        }
                        else
                        {
                            if (czescLewa.Liczba == true)
                            {
                                MessageBox.Show("blad");
                            }
                            else
                            {
                                switch (czescLewa.nazwaRejestru)
                                {
                                    case Rejestry.AX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                AX = (ushort)(AX - ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((AX & 0xFF) - ((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF00);
                                                AX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((AX & 0xFF00) >> 8) - ((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF);
                                                AX |= (ushort)((gora) << 8);
                                                break;


                                        }
                                        break;
                                    case Rejestry.BX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                BX = (ushort)(BX - ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((BX & 0xFF) - ((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF00);
                                                BX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((BX & 0xFF00) >> 8) - ((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF);
                                                BX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.CX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                CX = (ushort)(CX - ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((CX & 0xFF) - ((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF00);
                                                CX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((CX & 0xFF00) >> 8) - ((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF);
                                                CX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.DX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                DX = (ushort)(DX - ((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((DX & 0xFF) - ((ushort)zmienna & 0xFF));
                                                DX = (ushort)(DX & 0xFF00);
                                                DX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((DX & 0xFF00) >> 8) - ((ushort)zmienna & 0xFF));
                                                DX = (ushort)(DX & 0xFF);
                                                DX |= (ushort)((gora) << 8);
                                                break;

                                        }

                                        break;
                                }
                            }
                        }
                        break;
                    case "MOV":
                        if (split.Length < 3)
                        {
                            return;
                        }
                        zmienna = OdczytajWartosc(czescPrawa);
                        if (czescPrawa.Adres == true)
                        {
                            zmienna = pamiec[zmienna];
                        }

                        zmienna2 = 0;
                        if (czescLewa.Adres == true)
                        {
                            zmienna2 = OdczytajWartosc(czescLewa);
                            pamiec[zmienna2] = (ushort)zmienna;
                        }
                        else
                        {
                            if (czescLewa.Liczba == true)
                            {
                                MessageBox.Show("blad");
                            }
                            else
                            {
                                switch (czescLewa.nazwaRejestru)
                                {
                                    case Rejestry.AX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                AX = (ushort)(((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)(((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF00);
                                                AX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((ushort)zmienna & 0xFF));
                                                AX = (ushort)(AX & 0xFF);
                                                AX |= (ushort)((gora) << 8);
                                                break;


                                        }
                                        break;
                                    case Rejestry.BX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                BX = (ushort)(((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)(((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF00);
                                                BX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((ushort)zmienna & 0xFF));
                                                BX = (ushort)(BX & 0xFF);
                                                BX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.CX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                CX = (ushort)(((ushort)zmienna));
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)(((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF00);
                                                CX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)(((ushort)zmienna & 0xFF));
                                                CX = (ushort)(CX & 0xFF);
                                                CX |= (ushort)((gora) << 8);
                                                break;

                                        }
                                        break;
                                    case Rejestry.DX:
                                        switch (czescLewa.czescRejestru)
                                        {
                                            case Czesc.Calosc:
                                                DX = (ushort)((ushort)zmienna);
                                                break;
                                            case Czesc.Dolna:
                                                ushort dol = (ushort)((ushort)zmienna & 0xFF);
                                                DX = (ushort)(DX & 0xFF00);
                                                DX |= dol;
                                                break;
                                            case Czesc.Gorna:
                                                ushort gora = (ushort)((ushort)zmienna & 0xFF);
                                                DX = (ushort)(DX & 0xFF);
                                                DX |= (ushort)((gora) << 8);
                                                break;

                                        }

                                        break;
                                }
                            }
                        }
                        break;
                }
                label1.Text = "Rejestr AX: " + AX.ToString();
                label2.Text = "Rejestr BX: " + BX.ToString();
                label3.Text = "Rejestr CX: " + CX.ToString();
                label4.Text = "Rejestr DX: " + DX.ToString();

                string binarneAX = Convert.ToString(Convert.ToInt32(AX), 2);
                label5.Text = "binarnie " + binarneAX;

                string binarneBX = Convert.ToString(Convert.ToInt32(BX), 2);
                label6.Text = "binarnie " + binarneBX;

                string binarneCX = Convert.ToString(Convert.ToInt32(CX), 2);
                label7.Text = "binarnie " + binarneCX;

                string binarneDX = Convert.ToString(Convert.ToInt32(DX), 2);
                label8.Text = "binarnie " + binarneDX;
            }
            else
            {
                MessageBox.Show("Koniec");
                counter= -1;

            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] textBox = richTextBox1.Lines;
            for (int i = 0; i < textBox.Length; i++)
            {
                Metoda(i);
            }
        }

        private void PrintScreen() //05h  Obsługuje klawisz Print Screen -1

        {

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            printscreen.Save(@"C:\Users\\Joanna\\Desktop\\printscreen.jpg", ImageFormat.Jpeg);

        }

        private void PodajCzasZegaraRTC() //1Ah 02h Podaje stan zegra RTC-2
        {
            MessageBox.Show("Aktualna godzina " + DateTime.Now.ToString());
        }

        private void OdczytPozycjiKursora() // 10h 03h	Odczytuje pozycję kursora-3
        {

            MessageBox.Show("x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
        }

        private void ZmienRozmairKursora() // 10h 01h Ustawia rozmiar kursora-4
        {
            richTextBox1.Font = new Font("Mistral", 15);
        }

        private void StworzNowyFolder() //DOS INT 21h AH 3C-5
        {
            string folderName = @"C:\Users\\Joanna\\Desktop";
            string pathString = System.IO.Path.Combine(folderName, "SubFolder");
            System.IO.Directory.CreateDirectory(pathString);
        }

        
       private void UtworzPlik()
        {
            string path1 = @"C:\Users\Joanna\Desktop\jakisplik.txt";
            System.IO.File.Create(path1).Dispose();
            

        }
        
        

        private void ZmienNazwePliku() // DOS INT 21h AH 56-6
        {
            string path11 = @"C:\Users\Joanna\Desktop\jakisplik.txt";
            string path2 = @"C:\Users\Joanna\Desktop\jakissobieinnyplik.txt";
            System.IO.File.Move(path11, path2);
        }
        

    

      private void UsunPlik() // DOS INT 21h AH 41 -7
        {
           File.Delete(@"C:\Users\\Joanna\\Desktop\\jakissobieinnyplik.txt");
        }
        

        private void HDD() //BIOS 13h 15h Podaje typ HDD -8
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            ArrayList hdCollection = new ArrayList();
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.Type = wmi_HD["InterfaceType"].ToString();
                hdCollection.Add(hd);
                MessageBox.Show("Model: " + hd.Model+ " Typ: " + hd.Type);


            }

        }
        class HardDrive
        {
            private string model = null;
            private string type = null;
            private string serialNo = null;
            public string Model
            {
                get { return model; }
                set { model = value; }
            }
            public string Type
            {
                get { return type; }
                set { type = value; }
            }
            public string SerialNo
            {
                get { return serialNo; }
                set { serialNo = value; }
            }
        }

        


    }


       
    }


    


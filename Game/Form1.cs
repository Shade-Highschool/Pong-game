using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int pohyb_x = 10; //Pohyb míčku doprava/doleva
        int pohyb_y = 10; //Pohyb míčku dolu/nahoru
        int rychlost_rakety = 15; //Rychlost pohybu naší rakety
        bool key_up, key_down, key_w, key_s; //Proměnné pro pohyb
        int score_player1 = 0; //skóre hráč1
        int score_player2 = 0; //skóre hráč2
        bool menu = true; //menu otevřené true/false
        private void Check(object sender) // Zaškrtne správnou volbu
        {
            toolStripMenuItem4.Checked = false;
            toolStripMenuItem5.Checked = false;
            toolStripMenuItem6.Checked = false;
            toolStripMenuItem7.Checked = false;
           ToolStripMenuItem mI = (ToolStripMenuItem)sender;
           mI.Checked = true;
        }
        private void Umisti() //Rozmístí podle velikosti obrazu
        {
            label1.Location = new Point((ClientSize.Width / 2) - label1.Width * 2, label1.Height);
            label2.Location = new Point((ClientSize.Width / 2) + label2.Width, label2.Height);
            label3.Location = new Point((ClientSize.Width / 2) - label3.Width / 2, (ClientSize.Height / 2) - label2.Height / 2);
            label5.Location = new Point(ClientSize.Width - label5.Width, Wall_top.Height);

            Wall_top.Location = new Point(0, 0);
            Wall_bottom.Location = new Point(0, ClientSize.Height - 1);
            Wall_top.Height = 1;
            Wall_top.Width = ClientSize.Width;
            Wall_bottom.Height = 1;
            Wall_bottom.Width = ClientSize.Width;

            menuStrip1.Location = new Point(0, ClientSize.Height - menuStrip1.Height);

        }
        private void Reset() // Vrátí do úvodní pozice
        {
            Raketa.Location = new Point(Raketa.Width, (ClientSize.Height / 2) - (Raketa.Height / 2));
            Raketa2.Location = new Point(ClientSize.Width - (Raketa2.Width * 2), (ClientSize.Height / 2) - (Raketa2.Height / 2));
            Micek.Location = new Point((ClientSize.Width / 2) - (Micek.Width / 2), (ClientSize.Height / 2) - (Micek.Height / 2));
            
            timer2.Start(); //Handle pohybu
        }
        private void Rozhoz()//Random rozhoz míčku
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 1)
                pohyb_x = -pohyb_x;
            Thread.Sleep(1000);
        }
        private void Score_player1()//Zaskóruje hráč2
        {
            score_player1++;
            label1.Text = score_player1.ToString();
            if (score_player1 == 10)
                WinPlayer1();
            Reset();
            Rozhoz();
        }
        private void WinPlayer1()
        {
            timer1.Stop();
            MessageBox.Show("Vyhrál Player1!");
            toolStripMenuItem1_Click(null, null);
        }
        private void WinPlayer2()
        {
            timer1.Stop();
            MessageBox.Show("Vyhrál Player2!");
            toolStripMenuItem1_Click(null, null);
        }
        private void Score_player2()//Zaskóruje hráč2
        {
            score_player2++;
            label2.Text = score_player2.ToString();
            if (score_player2 == 10)
                WinPlayer2();
            Reset();
            Rozhoz();
        }
        private void PlaySound(string sound)
        {
            SoundPlayer sp = new SoundPlayer(@"sounds\" + sound);
            sp.Play();
        }
        private void timer1_Tick(object sender, EventArgs e) //Pohybuje
        {
            Micek.Left += pohyb_x;
            Micek.Top += pohyb_y; //Pohyb míčku

            if (Micek.Bounds.IntersectsWith(Raketa.Bounds) || Micek.Bounds.IntersectsWith(Raketa2.Bounds)) //Kolize s raketama
            {
                pohyb_x = -pohyb_x; //obrácení směru x
                PlaySound("plop.wav");
            }
            if (Micek.Bounds.IntersectsWith(Wall_bottom.Bounds) || Micek.Bounds.IntersectsWith(Wall_top.Bounds)) //Kolize se stěnama
            {
                pohyb_y = -pohyb_y; //obrácení směru y
                PlaySound("beep.wav");
            }

            if (Micek.Location.X < 0)
            { Score_player2();PlaySound("end.wav");}
            if(Micek.Location.X >ClientSize.Width)
            {  Score_player1();PlaySound("end.wav");}
            
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e) //Stisk klávesy
        {
            if (e.KeyCode == Keys.W)
                key_w = true;
            if (e.KeyCode == Keys.S)
                key_s = true;
            if (e.KeyCode == Keys.Up)
                key_up = true;
            if (e.KeyCode == Keys.Down)
                key_down = true;

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e) //Puštění klávesy
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (menu)
                    menu = false;
                else
                    menu = true;
                ShowMenu(menu); 
            }

            if (e.KeyCode == Keys.W)
                key_w = false;
            if (e.KeyCode == Keys.S)
                key_s = false;
            if (e.KeyCode == Keys.Up)
                key_up = false;
            if (e.KeyCode == Keys.Down)
                key_down = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //New game
        {
            label1.Text = "0";
            label2.Text = "0";
            score_player1 = 0;
            score_player2 = 0;
            Reset();
            menu = false;
            ShowMenu(menu);

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) //Exit
        {
            Application.Exit();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)//Easy
        {
            pohyb_x = 10;
            pohyb_y = 10;
            Raketa.Height = 232;
            Raketa2.Height = 232;
            Check(sender);

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)//Normal
        {
            pohyb_x = 13;
            pohyb_y = 13;
            Raketa.Height = 180;
            Raketa2.Height = 180;
            Check(sender);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)//Hard
        {
            pohyb_x = 16;
            pohyb_y = 16;
            Raketa.Height = 180;
            Raketa2.Height = 180;
            Check(sender);
            
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)//Insane
        {
            pohyb_x = 10;
            pohyb_y = 10;
            Raketa.Height = 80;
            Raketa2.Height = 80;
            Check(sender);

        }
        private void timer2_Tick(object sender, EventArgs e) //Hýbání raketama
         {
             if (key_w && Raketa.Top > 0)
                 Raketa.Top -= rychlost_rakety;
             if (key_up && Raketa2.Top > 0)
                 Raketa2.Top -= rychlost_rakety;

             if (key_s && (Raketa.Top - ClientSize.Height) + Raketa.Height < 0)
                 Raketa.Top += rychlost_rakety;
             if (key_down && (Raketa2.Top - ClientSize.Height) + Raketa.Height < 0)
                 Raketa2.Top += rychlost_rakety;
         }

         private void Form1_Load(object sender, EventArgs e)
         {
             Umisti();
            Reset();
            ShowMenu(menu);
            label3.Visible = false;
         } //Pozicuje a zavolá menu
        private void ShowMenu(bool status) //Zobrazí/vypne menu
        {
            if (status)
            {
                menuStrip1.Visible = true;
                label3.Visible = true;
                timer1.Stop();
                timer2.Stop();
            }
            else
            {
                menuStrip1.Visible = false;
                label3.Visible = false;
                timer1.Start();
                timer2.Start();
            }

        }
         private void Form1_Paint(object sender, PaintEventArgs e)
         {
             Graphics g = e.Graphics;
             Pen pen = new Pen(Brushes.White, 1);
             pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
             g.DrawLine(pen, ClientSize.Width / 2, 0, ClientSize.Width / 2, ClientSize.Height); //Tečkovaná čára uprostřed
         }

         private void ovládáníToolStripMenuItem_Click(object sender, EventArgs e)//Ovládání
         {
             MessageBox.Show("Player1\n W = Nahoru\n S = Dolu\nPlayer2\n Šipka nahoru = Nahoru\n Šipka dolu = dolu","Ovládání");
         }
    }
}

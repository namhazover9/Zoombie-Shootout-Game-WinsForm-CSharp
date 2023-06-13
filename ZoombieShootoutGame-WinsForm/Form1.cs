using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZoombieShootoutGame_WinsForm
{
    public partial class Form1 : Form
    {
        bool goUp, goDown, goLeft, goRight; // movement of player
        string facing = "up"; // this string is called facing and it will be used to guide the bullets
        int playerHealth = 100;
        int playerSpeed = 6;
        int zoombieSpeed = 2;
        int ammo = 10;
        int score = 0;
        bool gameOver = false;
        Random rnd = new Random();

        List<PictureBox> zoombiesList = new List<PictureBox>(); // List to remove zoombies from the game
       
        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if(playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            } else
            {
                gameOver= true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            // Text
            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;

            // Movement of player
            if(goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            if(goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += playerSpeed;
            }
            if(goUp == true && player.Top > 35)
            {
                player.Top -= playerSpeed;
            }
            if(goDown  == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += playerSpeed;
            }

            foreach(Control x in this.Controls)
            {
                // Pick up the ammo
                if(x is PictureBox && (string)x.Tag == "ammo")
                {
                    if(player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }

                // Move the zoombie to player
                if(x is PictureBox && (string)x.Tag == "zoombie")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }
                    if(score >= 30)
                    {
                        zoombieSpeed = 3;
                    }

                    // Move to the left
                    if(x.Left > player.Left)
                    {
                        x.Left -= zoombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    // Move to the right
                    if (x.Left < player.Left)
                    {
                        x.Left+= zoombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }
                    // Move up
                    if (x.Top > player.Top)
                    {
                        x.Top -= zoombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    // Move down
                    if (x.Top < player.Top)
                    {
                        x.Top += zoombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }
                }

                // Bullet intersect wiht Zoombie
                foreach(Control j in this.Controls)
                {
                    if(j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zoombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            // Remove the bullet and zoombie
                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zoombiesList.Remove((PictureBox)x);
                            MakeZoombies();
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // Stop the game
            if(gameOver == true)
            {
                return;
            }

            // Movement of the player
            if(e.KeyCode == Keys.Left)
            {
                goLeft= true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight= true;
                facing= "right";
                player.Image = Properties.Resources.right;
            }
            if(e.KeyCode == Keys.Up)
            {
                goUp= true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }
            if(e.KeyCode == Keys.Down)
            {
                goDown= true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            // Shoot the bullet
            if(e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);

                int random = rnd.Next(1, 11);
                if (ammo == 2 || random == 5)
                {
                    DropAmmo();
                }
            }

            if(e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }
 
        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.Size = new Size(48, 63);
            ammo.SizeMode = PictureBoxSizeMode.StretchImage;
            ammo.Left = rnd.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = rnd.Next(35, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);
            ammo.BringToFront();
            player.BringToFront();

        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZoombies()
        {
            PictureBox zoombie = new PictureBox();
            zoombie.Tag = "zoombie";
            zoombie.Image = Properties.Resources.zdown;
            zoombie.Left = rnd.Next(0, 900);
            zoombie.Top = rnd.Next(0, 800);
            zoombie.Size = new Size(48, 63);
            zoombie.SizeMode = PictureBoxSizeMode.StretchImage;
            zoombiesList.Add(zoombie);
            this.Controls.Add(zoombie);
            player.BringToFront();
        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;

            foreach(PictureBox i in zoombiesList)
            {
                this.Controls.Remove(i);
            }

            zoombiesList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeZoombies();
            }

            goUp = false;
            goDown= false;
            goLeft= false;
            goRight= false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }
    }
}

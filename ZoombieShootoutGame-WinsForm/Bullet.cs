using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ZoombieShootoutGame_WinsForm
{
    internal class Bullet
    {
        public string direction;
        public int bulletLeft;
        public int bulletTop;

        private int speedTimer = 20;
        private int speedBullet = 15;
        private PictureBox bullet = new PictureBox();
        private Timer bulletTimer = new Timer();

        public void MakeBullet(Form form)
        {
            bullet.BackColor = Color.White;
            bullet.Size = new Size(4, 4);
            bullet.Tag = "bullet";
            bullet.Left = bulletLeft;
            bullet.Top = bulletTop;
            bullet.BringToFront();

            form.Controls.Add(bullet);

            bulletTimer.Interval = speedTimer;
            bulletTimer.Tick += new EventHandler(BulletTimerEvent);
            bulletTimer.Start();
        }

        private void BulletTimerEvent(object sender, EventArgs e)
        {
            // movement of the bullet
            if(direction == "left")
            {
                bullet.Left -= speedBullet;
            }
            if (direction == "right")
            {
                bullet.Left += speedBullet;
            }
            if (direction == "up")
            {
                bullet.Top -= speedBullet;
            }
            if (direction == "down")
            {
                bullet.Top += speedBullet;
            }

            // Dispose the bullet and the timer event
            if(bullet.Left < 5 || bullet.Left > 860 || bullet.Top < 5 || bullet.Top > 600)
            {
                bulletTimer.Stop();
                bulletTimer.Dispose();
                bullet.Dispose();
                bulletTimer = null;
                bullet = null;
            }
        }
    }
}

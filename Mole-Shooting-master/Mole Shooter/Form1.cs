//#define My_Debug // this one is for testing the position of mouse when user move the mouse

using System;
using System.Drawing;
using System.Windows.Forms;
using Mole_Shooter.Properties;
using System.Media;

namespace Mole_Shooter
{
    public partial class MoleShooter : Form
    {
        // declare 2 variables that control the update time of the game.
        const int FrameNum = 8; // equivalent with period of 800ms
        const int SplatNum = 3;// equivalent with period of 300ms

        //we need this boolean splat to check if the splash symbol should be display or not.
        //if the mole is shoot : splat = true.
        //if splat = true, we check if splash symbol has been appeared enough in 300ms or not?.
        //if splat = true = splash symbol has been appeard enough in 300ms --> splat = false again
        //the intial value is false
        bool splat = false;

        //this _gameFrame variable will increase every time the timer trigger.
        //the timer interval is 100ms so the timer will trigger every 100ms.
        //means that this variable will increase by 1 every 100ms.
        //when _gameFrame> FrameNum = 8 --> _gameFrame = 0 again.
        int _gameFrame = 0;

        //this _splatTime counter only increase by 1 everytime the timer trigger.
        //when the boolean splat = true.
        //when _splatTime > SplatNum = 3 --> _splatTime = 0 again.
        int _splatTime = 0;

        //these 4 variables will display the score/game result status of gamer.
        //type of _avarageHits is double because it = _hits/_totalShots.
        int _hits = 0;
        int _misses = 0;
        int _totalShots = 0;
        double _averageHits = 0;

#if My_Debug
        int _cursX = 0;
        int _cursY = 0;
#endif
       //Initialize _mole object with properties/methods inherited from CMole class
       //this object will moving during the game
       //this Mole Shooter class contain CMole class
       private CMole _mole;

       //Initialize _splat object with properties/methods inherited from CSplat class
       //this object also need update position during the game
       //this Mole Shooter class contain CSplat class
       private CSplat _splat;

       //Initialize _sign object with properties/methods inherited from CSign class
       //this object is at fixed position during the game
       //this Mole Shooter class contain CSign class
       private CSign _sign;

       //Initialize _scoreframe object with properties/methods inherited from CScoreFrame class
       //this object is at fixed position during the game
       //this Mole Shooter class contain CScoreFrame class
       private CScoreFrame _scoreframe;

       //create a variable at type random in order to create random position of the mole
       Random rnd = new Random();

        //The game program is not executed as sequential but by timer
        public MoleShooter()
        {
            //Don't put anything before InitializeComponent().
            InitializeComponent();

            //need this code to make the form update smoothly
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            //Update the initial position of the mole when the game is loaded
            //_mole position will be changed during the game
            //Outer class ( Mole Shooter) creates the cointained class ( CMole) upon start-up
            //If we did not, the _mole would begin life as a null reference
            _mole = new CMole() { Left = 300, Top = 450 };

            //intialize the initial position of _scoreFrame and _sign
            //these positons will be fixed during playing game
            //Outer class ( Mole Shooter) creates the cointained classes (_scoreframe and _sign) upon start-up
            //If we did not, the _scoreframe and _sign would begin life as a null reference
            _scoreframe = new CScoreFrame() { Left = 10, Top = 10 };
            _sign = new CSign() { Left = 580, Top = 192 };

            //_splat positon will be updated in Hit() method
            //Outer class ( Mole Shooter) creates the contained class ( _splat) upon start-up
            //If we did not, the _splat would begin life as a null reference
            _splat = new CSplat();

            //Create Scope Site
            //the center point is ( b.Height/2, b.Width/2)
            Bitmap b = new Bitmap(Resources.Site);
            this.Cursor = CustomeCursor.CreateCursor(b, b.Height / 2, b.Width / 2);

        }

        //This timerGameLoop_Tick control how the game run
        //Method UpdateMole() is called with condition in timerGameLoop_Tick
        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= FrameNum)
            {
                UpdateMole();
                _gameFrame = 0;
            }

            if(splat)
            {
                if(_splatTime >= SplatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateMole();
                }
                _splatTime++;
            }

            _gameFrame++;
            this.Refresh();
        }

        //UpdateMole() method will update mole position randomly when called
        private void UpdateMole()
        {
            _mole.Update(rnd.Next(Resources.mole.Width, this.Width - Resources.mole.Width),
                         rnd.Next(this.Height / 2, this.Height - Resources.mole.Height * 2)
                         );
        }

        //using OnPaint() method to draw objects on game background 
        //using System.Drawing ( GDI+)
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            //if splat == true then draw object _splat
            if (splat == true)
            {
                _splat.DrawImage(dc);
            }
            //else draw _mole object
            else
            {
                _mole.DrawImage(dc);
            }
            //_splat.DrawImage(dc);

            //always draw _sign and _scoreframe object as they are at fixed position during game
            _sign.DrawImage(dc);
            _scoreframe.DrawImage(dc);

#if My_Debug
            TextFormatFlags flag = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _fontt = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(dc, "X = " + _cursX.ToString() + ":" + "Y=" + _cursY.ToString(),
                _fontt, new Rectangle(0, 0, 120, 20), SystemColors.ControlText, flag);
#endif

            //Put score on screen
            TextFormatFlags flags = TextFormatFlags.Left;
            Font _font = new System.Drawing.Font("Stencil", 14, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, "Shots :" + _totalShots.ToString(), _font, new Rectangle(40, 62, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Misses :" + _hits.ToString(), _font, new Rectangle(40, 82, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Hits :" + _misses.ToString(), _font, new Rectangle(40, 102, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Avg :" + _averageHits.ToString("F0")+ "%", _font, new Rectangle(40, 122, 120, 20), SystemColors.ControlText, flags);
            base.OnPaint(e);
         }

        //MoleShooter_MouseMove method update mouse position when user move the mouse around to catch mole
        private void MoleShooter_MouseMove(object sender, MouseEventArgs e)
        {

#if My_Debug
            _cursX = e.X;
            _cursY = e.Y;
#endif
            this.Refresh();
        }

        //MoleShooter_MouseClick method give condition to distince the control board items
        //and include Hit() and Misses() method
        //when user click - it means that user try to shot the mole
        //if user shot correct , Hit() method is called
        //if user shot incorrect, Misses() method is called
        private void MoleShooter_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.X > 660 && e.X < 704 && e.Y > 242 && e.Y < 256)
            {
                timerGameLoop.Start();
            }

            else if (e.X > 661 && e.X < 696 && e.Y > 259 && e.Y < 271)
            {
                timerGameLoop.Stop();
            }

            else if (e.X > 661 && e.X < 703 && e.Y > 277 && e.Y < 289)
            {
                splat = false;
                timerGameLoop.Stop();
                _hits = 0;
                _misses = 0;
                _totalShots = 0;
                _averageHits = 0;
                _mole.Left = 300;
                _mole.Top = 450;
             }

            else if (e.X > 660 && e.X < 694 && e.Y > 295 && e.Y < 306)
            {
                timerGameLoop.Stop();
                DialogResult result = MessageBox.Show(this, " Do you really want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
           else
            {
                //Hit condition which is called from Hit() method in CMole class
                //(e.X, e.Y) here is the center point position, not mole position
                if(_mole.Hit(e.X,e.Y))
                {
                    splat = true;
                    _splat.Left = _mole.Left - Resources.Splat.Width / 3;
                    _splat.Top = _mole.Top - Resources.Splat.Height / 3;
                    _hits++;
                }
                else { _misses++; }
                
                _totalShots = _hits + _misses;
                _averageHits = (double)_hits / (double)_totalShots * 100.0;
            }
            // Call FireGun() method to create Gun sound
            FireGun();
        }

        public void FireGun()
        {
            SoundPlayer simpleSound = new SoundPlayer(Resources.gun_sound);
            simpleSound.Play();
        }

        private void MoleShooter_Load(object sender, EventArgs e)
        {

        }
    }
}

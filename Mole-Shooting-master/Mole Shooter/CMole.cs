using Mole_Shooter.Properties;
using System.Drawing;

namespace Mole_Shooter
{
    //ensure that CMole cannot act as a base class to others.
    sealed class CMole: CImageBase
    {
        public CMole() 
            : base(Resources.mole)
         {
            // intialize _moleHotSpot rectangle
            _moleHotSpot.X = Left + 20;
            _moleHotSpot.Y = Top - 1;
            _moleHotSpot.Width = 30;
            _moleHotSpot.Height = 40;
         }

        //Create _moleHotSpot method at type rectangle
        private Rectangle _moleHotSpot = new Rectangle();

        //Update _moleHotSpot rectangle when UpdateMole() method change mole (X,Y) position
        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            _moleHotSpot.X = Left + 20;
            _moleHotSpot.Y = Top - 1;
        }

        //Hit method at boolean type, return true or false
        //it will draw a rectangle (X, Y, 1, 1) with (X,Y) is center point coordinator and (1,1) 
        //is the length and the width of the rectangle
        //if _moleHotSpot contains ( X, Y, 1,1) --> it means Hit and return true
        public bool Hit(int X, int Y)
        {
            Rectangle c = new Rectangle(X, Y, 1, 1);
            if(_moleHotSpot.Contains(c))
            {
                return true;
            }
            return false;
        }
    }
}

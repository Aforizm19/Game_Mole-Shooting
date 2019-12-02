using Mole_Shooter.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_Shooter
{
    //this class is inherited fully from CImageBase class and has no private properties class

    sealed class CSplat : CImageBase
    {
        public CSplat() : base(Resources.Splat)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.Base
{
    interface ISaver
    {
        bool SaveInfo(string path);
        object LoadInfo(string path);
    }
}

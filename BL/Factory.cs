using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Factory Method
    /// </summary>
    public class Factory
    {
        public static IBL GetBL()
        {
            return BL.GetInstance();
        }
    }
}

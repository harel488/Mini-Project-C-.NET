using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    ///Currently, the factory method only brings the object to realization by using lists ,
    ///below we will add the realization of the xml files
    /// </summary>
    public class Factory
    {

        public static Idal get_imp_Dal(string type)
        {
            switch (type)
            {
                case "list":
                    return imp_Dal.GetInstance();
                    
               case "xml":
                    return Dal_XML_imp.GetInstance();
                    
                default:
                    return null;
                    
            }
        }
    }
}

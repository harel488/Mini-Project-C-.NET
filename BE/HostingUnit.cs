using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
    /// <summary>
    /// A hosting unit object that belongs to a particular host
    /// the "Owner" is the Host of this unit 
    /// </summary>
    public class HostingUnit
    {
        public string HostingUnitKey { get; set; }
        public Enums.HostUnitType Type { get; set; }
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }

        [XmlIgnore]
        public bool[,] Diary = new bool[12, 31];

        [XmlArray("Diary")]
        public bool[] XmlDiary
        {
            get { return Diary.Flatten(); }
            set { Diary= value.Expand(12); } //5 is the number of roes in the matrix
        }
        public BE.Enums.Area Area { get; set; }
        public string Price { get; set; }
     
        //===========================================================================================

        //ToString
        public override string ToString()
        {
            string UnitStr = "Hosting Unit Key: " + HostingUnitKey + '\n' +
                "Hosting Unit Name: " + HostingUnitName + '\n'+"Hosting Unit Type: "+Type.ToString()+'\n';
            return UnitStr;
        }



   }
    /// <summary>
    /// this class convert the Diary matrix to array
    /// </summary>
    public static class Tools
    {
        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    var test = arr[i, j];
                    arrFlattened[i * rows + j] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    arrExpanded[i, j] = arr[i * rows + j];
                }
            }
            return arrExpanded;
        }
    }
}

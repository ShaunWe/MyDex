using System;
using System.Collections.Generic;

namespace MyDex
{
    class Utility
    {
        public static void KeyToProceed()
        {
            //This is a standard pause line
            Console.WriteLine("Press a key to continue.");
            Console.ReadKey();
        }

        public static string displayArray(string[] inArray)
        {
            //This method builds a formatted string of the array passed to it
            string returnString = "{ ";

            if (inArray.Length < 1)
            {
                return "{No data}";
            }

            for (int i = 0; i < inArray.Length; i++)
            {
                if (i < inArray.Length - 1)
                {
                    returnString = returnString + inArray[i] + ", ";
                }
                else
                {
                    returnString = returnString + inArray[i] + " ";
                }
            }

            returnString += "}";

            return returnString;
        }

        public static string displayList(IList<string> inList)
        {
            //This method builds a formatted string of the list<string> passed to it
            string returnString = "{ ";

            if (inList.Count < 1)
            {
                return "{No data}";
            }

            for (int i = 0; i < inList.Count; i++)
            {
                if (i < inList.Count - 1)
                {
                    returnString += inList[i] + ", ";
                }
                else
                {
                    returnString += inList[i] + " ";
                }
            }

            returnString += "}";

            return returnString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bdtool.Utilities.Binary;

namespace bdtool.Utilities
{
    public static class Comms
    {
        /// <summary>
        /// HashName__Q27GtComms16CGtCommsDatabasePCcN21 from BO4 Prototype for PS2.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int HashName(string name, string path, string fileName)
        {
            // Concatenate name + path
            string temp = name + path;

            // Add "/" if not already present at end
            /*if (!temp.EndsWith('/'))
            {
                temp += "/";
            }*/

            if (temp.Length > 0 && temp[^1] != '/')
            {
                temp += "/";
            }

            // Add filename
            temp += fileName;

            // Calculate hash
            return Hash.CalculateHash(temp);
        }
    }
}

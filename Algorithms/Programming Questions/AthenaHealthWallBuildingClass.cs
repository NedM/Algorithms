using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Programming_Questions
{
    public enum Blocks
    {
        Three = 0,
        FourPointFive = 1
    };

    public class AthenaHealthWallBuildingClass
    {
        ///Need to write algorithm to figure out how many different ways that one could build an N by M wall
        ///using only blocks of the following dimensions: 3x1, 4.5x1. Furthermore, no seam may line up with another 
        ///in the line above or below.
        ///
        ///Given: there are 2 ways to build a 7.5x1 wall [3, 4.5] and [4.5, 3]
        ///       there are 2 ways to build a 7.5x2 wall {[3, 4.5],[4.5, 3]} and {[4.5, 3], [3, 4.5]}
        ///       there are 4 ways to build a 12x3 wall
        ///       there are 7958 ways to build a 27x5 wall
        ///       
        ///How many ways are there to build a 48x10 wall?

        private int width = 0;
        private int height = 0; 

        public AthenaHealthWallBuildingClass(int width, int height)
        {
            //Validate input. 
            //Width must be between 3 and 48 inclusive and must be a multiple of 0.5
            if ((width < 3) || 
                (width > 48) || 
                (width % 0.5 != 0))
            {
                throw new ArgumentOutOfRangeException("width", "Width must be between 3 and 48 inclusive and must be a multiple of 0.5. Width given was " + width);
            }
            //Height must be and integer between 1 and 10 inclusive
            if ((height < 1) ||
                (height > 10))
            {
                throw new ArgumentOutOfRangeException("height", "Height must be and integer between 1 and 10 inclusive. Height given was " + height);
            }

            this.width = width;
            this.height = height;
        }

        public uint CalculateNumWaysToBuildWall()
        {
            uint numWays = 0;
            


            

            return numWays;
        }

        public Dictionary<int, double[]> BuildFirstRowVariants()
        {
            Dictionary<int, double[]> rowVariants = new Dictionary<int, double[]>(); //Hash map to hold first row variants and their joint locations on the x-axis

            return rowVariants;
        }

        /// <summary>
        /// Depth first recursive method to build next row given previous row's joints
        /// </summary>
        /// <param name="previousRowJoints"></param>
        /// <returns></returns>
        public bool BuildNextRow(Dictionary<double, bool> previousRowJoints)
        {
            return true;
        }
    }
}

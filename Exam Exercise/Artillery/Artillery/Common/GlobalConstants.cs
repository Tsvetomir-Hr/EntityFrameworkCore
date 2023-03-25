using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Common
{
    public class GlobalConstants
    {
        //Country
        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 60;

        public const int ArmySizeMin = 50_000;
        public const int ArmySizeMax = 10_000_000;



        //Manufacturer
        public const int ManufarcturerNameMinLength = 4;
        public const int ManufarcturerNameMaxLength = 40;

        public const int ManufarcturerFoundedMinLength = 10;
        public const int ManufarcturerFoundedMaxLength = 100;

        //SHell

        public const int ShellCaliberMinLength = 4;
        public const int ShellCaliberMaxLength = 30;

        public const string ShellWeightMin = "2";
        public const string ShellWeightMax = "1680";

        //Gun
        public const int GunWeightMin = 100;
        public const int GunWeightMax = 1_350_000;

        public const double BarrelLengthMin = 2.00;
        public const double BarrelLengthMax = 35.00;

        public const int GunRangeMin = 1;
        public const int GunRangeMax = 100_000;





    }
}

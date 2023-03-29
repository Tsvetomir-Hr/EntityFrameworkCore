using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Common
{
    public class GlobalConstants
    {
        public const int RegistrationNumberLength = 8;

        public const int VinNumberLength = 17;

        public const int TankCapacityMin = 950;
        public const int TankCapacityMax = 1420;

        public const int CargoCapacityMin = 5000;
        public const int CargoCapacityMax = 29000;

        //client

        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;

        public const int NationalityMinLength = 2;
        public const int NationalityMaxLength = 40;


        //dispatcher
        public const int DespatcherNameMinLength = 2;
        public const int DespatcherNameMaxLength = 40;

    }
}

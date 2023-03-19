using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.Common
{
    public class ValidationConstants
    {
        //Prisoner
        public const int PrisonerFullNameMinLength = 3;
        public const int PrisonerFullNameMaxLength = 20;

        public const int PrisonerAgeMin = 18;
        public const int PrisonerAgeMax = 65;

        public const string PrisonerBailMin = "0";
        public const string PRisonerBailMax = "79228162514264337593543950335";

        public const string PrisonerNicknameRegex = "The\\s[A-Z][a-z]*";
        //Officer
        public const int OfficerFullNameMinLength = 3;
        public const int OfficerFullNameMaxLength = 30;
        public const string OfficerMinSalary = "0";
        public const string OfficerMaxSalary = "79228162514264337593543950335";
        //Department
        public const int DepartmentFullNameMinLength = 3;
        public const int DepartmentFullNameMaxLength = 25;

        //Cell
        public const int CellMinNumber = 1;
        public const int CellMaxNumber = 1000;

        //Mail
        public const string MailAddressRegex = "^([A-Za-z 0-9]+?)(\\sstr\\.)$";
    }
}

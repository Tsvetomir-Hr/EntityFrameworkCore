using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Common
{
    public class ValidationConstants
    {
        //Theatre
        public const int TheatreNameMinLength = 4;
        public const int TheatreNameMaxLength = 30;

        public const int TheatreHallsMinNumber = 1;
        public const int TheatreHallsMaxNumber = 10;

        public const int TheatreDirectorMinLength = 4;
        public const int TheatreDirectorMaxLength = 30;

        //Play
        public const int PlayTtitleMinLength = 4;
        public const int PlayTitleMaxLength = 50;

        public const double PlayRatingMin = 0.00;
        public const double PlayRatingMax = 10.00;
        public const string PlayRatingMinString = "0";
        public const string PlayRatingMaxString = "10.00";
        public const int PlayDescriptionMaxLength = 700;

        public const int PlayScreenWriterMinLength = 4;
        public const int PlayScreenWriterMaxLength = 30;

        //Cast
        public const int CastFullNameMinLegth = 4;
        public const int CastFullNameMaxLegth = 30;

        //Ticket
        public const int TicketPriceMaxInt = 100;
        public const string TicketMinPrice = "1.00";
        public const string TicketMaxPrice = "100.00";

        public const int TicketMinRowNumber = 1;
        public const int TicketMaxRowNumber = 10;








    }
}

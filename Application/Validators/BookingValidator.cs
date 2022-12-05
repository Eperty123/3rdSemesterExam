using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class BookingValidator :AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(b => b.CoachId).GreaterThan(0);
            RuleFor(b => b.ClientId).GreaterThanOrEqualTo(0);
            RuleFor(b => b.CoachId).NotEqual(b => b.ClientId);
            RuleFor(b => b.Date).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}

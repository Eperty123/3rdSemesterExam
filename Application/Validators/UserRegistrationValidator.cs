using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UserRegistrationValidator : AbstractValidator<RegisterUserDTO>
    {
        public UserRegistrationValidator()
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Usertype).NotEmpty();
            RuleFor(u => u.Usertype).Matches("Coach|Client");
        }
    }
}

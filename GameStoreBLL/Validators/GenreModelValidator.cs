using FluentValidation;
using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Validators
{
    public class GenreModelValidator : AbstractValidator<GenreModel>
    {
        public GenreModelValidator()
        {
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage("Name can not be empty.");
        }
    }
}

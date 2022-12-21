using FluentValidation;
using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Validators
{
    public class FilterSearchModelValidator : AbstractValidator<FilterSearchModel>
    {
        public FilterSearchModelValidator()
        {
            RuleFor(model => model.Title)
                .MinimumLength(3)
                .WithMessage("Title should be at least 3 characters long.");
        }
    }
}

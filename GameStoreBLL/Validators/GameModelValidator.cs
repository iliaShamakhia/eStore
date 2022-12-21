using FluentValidation;
using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Validators
{
    public class GameModelValidator : AbstractValidator<GameModel>
    {
        public GameModelValidator()
        {
            RuleFor(model => model.Title)
                .NotEmpty()
                .WithMessage("Title can not be empty.");
            RuleFor(model => model.Description)
                .NotEmpty()
                .WithMessage("Description can not be empty.");
            RuleFor(model => model.ImageUrl)
                .NotEmpty()
                .WithMessage("Description can not be empty.");
            RuleFor(model => model.Price)
                .GreaterThan(0)
                .WithMessage("Price can not be negative."); ;
        }
    }
}

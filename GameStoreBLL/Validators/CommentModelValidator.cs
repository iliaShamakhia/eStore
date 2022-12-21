using FluentValidation;
using GameStoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Validators
{
    public class CommentModelValidator : AbstractValidator<CommentModel>
    {
        public CommentModelValidator()
        {
            RuleFor(model => model.Body)
                .NotEmpty()
                .WithMessage("Comment body can not be empty.");
        }
    }
}

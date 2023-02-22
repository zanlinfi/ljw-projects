using EFCore.Db;
using EntityClass;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FebSystem.Helper
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(IdEFDbContext ctx)
        {
            RuleFor(e => e.UserName).NotEmpty().WithMessage("username can not be empty")
                .Length(3).WithMessage("username at least include 3 chars")
                .MustAsync((name, _) => ctx.Users.AnyAsync(u => u.UserName == name)).WithMessage(req => $"username {req.UserName} not be here");
            RuleFor(e => e.Password).NotEmpty().WithMessage("password can not be empty")
                .Length(6).WithMessage("password at least include 6 chars");
        }
    }
}

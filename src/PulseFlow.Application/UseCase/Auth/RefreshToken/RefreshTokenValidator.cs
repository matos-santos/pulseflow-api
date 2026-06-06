using FluentValidation;

namespace PulseFlow.Application.UseCase.Auth.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenInput>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("O Access token é obrigatório.");
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O Refresh token é obrigatório.");
    }
}

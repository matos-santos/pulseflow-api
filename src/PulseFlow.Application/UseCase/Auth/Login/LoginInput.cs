using MediatR;

namespace PulseFlow.Application.UseCase.Auth.Login;

public record LoginInput(string Email, string Password) : IRequest<LoginOutput>;

using MediatR;

namespace PulseFlow.Application.UseCase.Companies.RegisterCompany;

public record RegisterCompanyInput(
    string Name,
    string Slug,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string? PhoneNumber
    ) : IRequest<RegisterCompanyOutput>
{
}

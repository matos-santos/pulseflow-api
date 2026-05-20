namespace PulseFlow.Application.UseCase.Companies.RegisterCompany;

public record RegisterCompanyOutput(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive
);

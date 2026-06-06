using MediatR;
using PulseFlow.Domain.Repositories;
using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Enums;
using PulseFlow.Domain.Common.Helpers;

namespace PulseFlow.Application.UseCase.Companies.RegisterCompany;

public class RegisterCompanyUseCase(
    ICompanyRepository companyRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<RegisterCompanyInput, RegisterCompanyOutput>
{

    public async Task<RegisterCompanyOutput> Handle(RegisterCompanyInput request, CancellationToken cancellationToken)
    {
        var company = new Company(
            request.Name,
            request.Slug,
            true
            );

        await companyRepository.AddAsync(company, cancellationToken);

        var passwordHash = PasswordHelper.HashPassword(request.Password);

        company.AddUserDefault(
            request.Email,
            request.FirstName,
            request.LastName,
            passwordHash,
            RoleExtensions.ToDbString(Role.ADMIN),
            request.PhoneNumber
            );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterCompanyOutput(
            company.Id,
            company.Name,
            company.Slug,
            company.IsActive
        );
    }
}

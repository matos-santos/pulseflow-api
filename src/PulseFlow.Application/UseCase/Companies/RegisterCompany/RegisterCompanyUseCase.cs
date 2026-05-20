using MediatR;
using PulseFlow.Domain.Repositories;
using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Enums;

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

        company.AddUserDefault(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password,
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

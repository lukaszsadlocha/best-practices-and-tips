using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using MediatR;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    public bool ActiveOnly { get; init; }
}

public class GetCustomersQueryHandler(ICustomerRepository customerRepository)
    : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = request.ActiveOnly 
            ? await customerRepository.GetActiveCustomersAsync()
            : await customerRepository.GetAllAsync();

        return customers.Select(MapToDto);
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            Address = customer.Address,
            City = customer.City,
            Country = customer.Country,
            PostalCode = customer.PostalCode,
            IsActive = customer.IsActive,
            FullName = customer.GetFullName()
        };
    }
}

using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class GetCustomersUseCase(ICustomerRepository customerRepository) : IGetCustomersUseCase
{
    public async Task<IEnumerable<CustomerDto>> ExecuteAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var customers = activeOnly 
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

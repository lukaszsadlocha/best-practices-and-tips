using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class GetCustomerByIdUseCase(ICustomerRepository customerRepository) : IGetCustomerByIdUseCase
{
    public async Task<CustomerDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        
        return customer == null ? null : MapToDto(customer);
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

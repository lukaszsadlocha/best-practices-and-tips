using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class CreateCustomerUseCase(ICustomerRepository customerRepository) : ICreateCustomerUseCase
{
    public async Task<CustomerDto> ExecuteAsync(CreateCustomerDto customer, CancellationToken cancellationToken = default)
    {
        // Check if customer with email already exists
        var existingCustomer = await customerRepository.GetByEmailAsync(customer.Email);
        if (existingCustomer != null)
        {
            throw new InvalidOperationException("Customer with this email already exists.");
        }

        var newCustomer = new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            Address = customer.Address,
            City = customer.City,
            Country = customer.Country,
            PostalCode = customer.PostalCode
        };

        var createdCustomer = await customerRepository.AddAsync(newCustomer);

        return MapToDto(createdCustomer);
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

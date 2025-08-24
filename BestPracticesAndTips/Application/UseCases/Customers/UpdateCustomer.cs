using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class UpdateCustomerUseCase(ICustomerRepository customerRepository) : IUpdateCustomerUseCase
{
    public async Task<CustomerDto> ExecuteAsync(int id, UpdateCustomerDto customerDto, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {id} not found.");
        }

        if (!customer.IsActive)
        {
            throw new InvalidOperationException("Cannot update inactive customer.");
        }

        // Update customer properties (excluding email per business rules)
        customer.FirstName = customerDto.FirstName;
        customer.LastName = customerDto.LastName;
        customer.PhoneNumber = customerDto.PhoneNumber;
        customer.Address = customerDto.Address;
        customer.City = customerDto.City;
        customer.Country = customerDto.Country;
        customer.PostalCode = customerDto.PostalCode;
        customer.UpdatedAt = DateTime.UtcNow;

        var updatedCustomer = await customerRepository.UpdateAsync(customer);

        return MapToDto(updatedCustomer);
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

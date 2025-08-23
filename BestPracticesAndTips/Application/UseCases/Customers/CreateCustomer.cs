using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using MediatR;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public CreateCustomerDto Customer { get; init; } = null!;
}

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Check if customer with email already exists
        var existingCustomer = await customerRepository.GetByEmailAsync(request.Customer.Email);
        if (existingCustomer != null)
        {
            throw new InvalidOperationException("Customer with this email already exists.");
        }

        var customer = new Customer
        {
            FirstName = request.Customer.FirstName,
            LastName = request.Customer.LastName,
            Email = request.Customer.Email,
            PhoneNumber = request.Customer.PhoneNumber,
            DateOfBirth = request.Customer.DateOfBirth,
            Address = request.Customer.Address,
            City = request.Customer.City,
            Country = request.Customer.Country,
            PostalCode = request.Customer.PostalCode
        };

        var createdCustomer = await customerRepository.AddAsync(customer);

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

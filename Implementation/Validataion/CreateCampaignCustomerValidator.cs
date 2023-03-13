using Application.DataTransfer;
using DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Validataion
{
    public class CreateCampaignCustomerValidator : AbstractValidator<CampaignCustomerDto>
    {
        private const int maxCustomersPerDay = 5;
        public CreateCampaignCustomerValidator(PowerPlatformTaskContext context)
        { 
            RuleFor(x => x.CampaignId)
                .NotEmpty().WithMessage("Campaigh is required!")
                .Must(x => context.Campaigns.Any(y => y.Id == x)).WithMessage("Selected campaign doesn't exist");
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required!");
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee is required!")
                .Must(x => context.Employees.Any(y => y.Id == x)).WithMessage("Selected employee doesn't exist")
                .DependentRules(() =>
                {
                    RuleFor(x => x.EmployeeId)
                    .Must(employeeId => context.CampaignCustomers
                        .Count(x => x.EmployeeId == employeeId
                            && x.CreateadAt.Day == DateTime.Now.Day) < maxCustomersPerDay)
                            .WithMessage("You have reached the maximum number of inserts for today in campaign!");
                });


        }
    }
}

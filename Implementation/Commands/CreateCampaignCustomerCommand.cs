using Application.Commands;
using Application.DataTransfer;
using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using ServiceReference1;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using Implementation.Validataion;
using FluentValidation;
using Application;
using Implementation.Repository;
using System.Net.Mail;
using System.Net;
using Application.Exceptions;
using Microsoft.Diagnostics.Tracing.Analysis.GC;

namespace Implementation.Commands
{
    public class CreateCampaignCustomerCommand : ICreateCampaignCustomerCommand
    {
        private readonly CreateCampaignCustomerValidator _validator;
        private readonly IEmailSender _sender;

        public CreateCampaignCustomerCommand(PowerPlatformTaskContext context, CreateCampaignCustomerValidator validator, IEmailSender sender)
        {
            _validator = validator;
            _sender = sender;
        }

        public async void Execute(CampaignCustomerDto request)
        {
            using(var _context = new PowerPlatformTaskContext())
            {
                _validator.ValidateAndThrow(request);

                var client = new ServiceReference1.SOAPDemoSoapClient();
                var task = client.FindPersonAsync(request.CustomerId.ToString());
                var person = await task;

                if(person == null)
                {
                    throw new EntityNotFoundException(request.CustomerId);
                }

                if (!_context.Customers.Any(x => x.Id == request.CustomerId))
                {
                    var customer = new Customer
                    {
                        Id = request.CustomerId,
                        Name = person.Name,
                        SSN = person.SSN,
                        DateOfBirth = person.DOB
                    };

                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }

                var user = _context.Employees.Where(x => x.Id == request.EmployeeId).FirstOrDefault();
                var campaign = _context.Campaigns.Find(request.CampaignId);
                var campaigncustomer = new CampaignCustomer
                {
                    CampaignId = request.CampaignId,
                    CustomerId = request.CustomerId,
                    EmployeeId = request.EmployeeId,
                    CreateadAt = request.CreateadAt
                };

                _context.CampaignCustomers.Add(campaigncustomer);
                _context.SaveChanges();

                var difference = DateTime.Now.Subtract(campaign.StartDateTime);
                if (difference.TotalDays >= 30)
                {
                    _sender.Send(new EmailDto
                    {
                        SendTo = user.Email,
                        Subject = "Report on successful sales",
                        Attachment = CreateCSVFile.GetCSVFile(request.CampaignId),
                        Content = "<h1>Congratulations!</h1>" +
                                  "<h2>These is a list of successful sales made during the first month of the campaign.</h2>"
                    });
                }
            }
        }
    }
}

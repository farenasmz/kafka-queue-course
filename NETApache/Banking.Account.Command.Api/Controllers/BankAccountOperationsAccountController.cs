using Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositFund;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Command.Application.Features.BankAccounts.WithDrawnFund;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Banking.Account.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BankAccountOperationsAccountController : ControllerBase
    {
        private readonly IMediator Mediator;

        public BankAccountOperationsAccountController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("", Name = "OpenAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> OpenAccount(OpenAccountCommand command)
        {
            var id = Guid.NewGuid().ToString();
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("CloseAccount/{id}", Name = "CloseAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> CloseAccount(string id)
        {
            var command = new CloseAccountCommand()
            {
                Id = id
            };

            return await Mediator.Send(command);
        }

        [HttpPut("DepositFund/{id}", Name = "DepositFund")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> DepositFund(string id, [FromBody] DepositFundsCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpPut("WithDrawnFund/{id}", Name = "WithDrawnFund")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> WithDrawnFund(string id, [FromBody] WithdrawFundsCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

    }
}

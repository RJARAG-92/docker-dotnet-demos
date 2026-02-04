using Demo03.Requests.Api.Contracts;
using Demo03.Requests.Application.Contracts.Requests;
using Demo03.Requests.Application.Contracts.Requests.Audit;
using Demo03.Requests.Application.UseCases.ApproveRequest;
using Demo03.Requests.Application.UseCases.CreateRequest;
using Demo03.Requests.Application.UseCases.GetRequestAudit;
using Demo03.Requests.Application.UseCases.GetRequestById;
using Demo03.Requests.Application.UseCases.RejectRequest;
using Demo03.Requests.Application.UseCases.SubmitRequest;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo03.Requests.Api.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public sealed class RequestsController : ControllerBase
    {
        private readonly CreateRequestHandler _create;
        private readonly SubmitRequestHandler _submit;
        private readonly ApproveRequestHandler _approve;
        private readonly RejectRequestHandler _reject;

        public RequestsController(CreateRequestHandler create, SubmitRequestHandler submit, ApproveRequestHandler approve, RejectRequestHandler reject)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _submit = submit ?? throw new ArgumentNullException(nameof(submit));
            _approve = approve ?? throw new ArgumentNullException(nameof(approve));
            _reject = reject ?? throw new ArgumentNullException(nameof(reject));
        }

        [SwaggerOperation(
            Summary = "Crea una nueva solicitud",
            Description = "Registra una nueva solicitud en estado inicial y genera el registro de auditoría."
        )]
        [HttpPost]
        [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] CreateRequestHttpRequest request,
            CancellationToken ct)
        {
            var dto = new CreateRequestDto(
                request.Title,
                request.Description,
                request.CreatedBy);

            var result = await _create.HandleAsync(
                new CreateRequestCommand(dto),
                ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [SwaggerOperation(
            Summary = "Obtiene una solicitud por identificador",
            Description = "Retorna el detalle de una solicitud a partir de su identificador único."
            )]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            Guid id,
            [FromServices] GetRequestByIdHandler handler,
            CancellationToken ct)
        {
            var result = await handler.HandleAsync(new GetRequestByIdQuery(id), ct);
            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Obtiene el historial de auditoría de una solicitud",
            Description = "Retorna los eventos de auditoría asociados a una solicitud."
            )]
        [HttpGet("{id:guid}/audit")]
        [ProducesResponseType(typeof(IEnumerable<RequestAuditDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAudit(
            Guid id,
            [FromServices] GetRequestAuditHandler handler,
            CancellationToken ct)
        {
            var result = await handler.HandleAsync(
                new GetRequestAuditQuery(id), ct);

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Envía una solicitud para aprobación",
            Description = "Cambia el estado de la solicitud a Enviada y registra el evento correspondiente."
            )]
        [HttpPut("{id:guid}/submit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Submit(
            Guid id,
            [FromBody] ChangeStatusHttpRequest request,
            CancellationToken ct)
        {
            await _submit.HandleAsync(
                new SubmitRequestCommand(id, request.Reason, request.ChangedBy),
                ct);

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Aprueba una solicitud",
            Description = "Cambia el estado de la solicitud a Aprobada y registra la auditoría."
            )]
        [HttpPut("{id:guid}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Approve(
            Guid id,
            [FromBody] ChangeStatusHttpRequest request,
            CancellationToken ct)
        {
            await _approve.HandleAsync(
                new ApproveRequestCommand(id, request.Reason, request.ChangedBy),
                ct);

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Rechaza una solicitud",
            Description = "Cambia el estado de la solicitud a Rechazada indicando el motivo."
            )]
        [HttpPut("{id:guid}/reject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Reject(
            Guid id,
            [FromBody] ChangeStatusHttpRequest request,
            CancellationToken ct)
        {
            await _reject.HandleAsync(
                new RejectRequestCommand(id, request.Reason, request.ChangedBy),
                ct);

            return NoContent();
        }
    }
}

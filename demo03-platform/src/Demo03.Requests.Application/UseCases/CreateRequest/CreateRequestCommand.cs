using Demo03.Requests.Application.Contracts.Requests;

namespace Demo03.Requests.Application.UseCases.CreateRequest
{
    public sealed record CreateRequestCommand(CreateRequestDto Data);
}

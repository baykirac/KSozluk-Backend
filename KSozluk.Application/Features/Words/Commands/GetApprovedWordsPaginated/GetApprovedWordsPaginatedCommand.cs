using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using MediatR;

namespace KSozluk.Application.Features.Words.Commands.GetApprovedWordsPaginated
{
    public class GetApprovedWordsPaginatedCommand : CommandBase<GetApprovedWordsPaginatedResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }   
}

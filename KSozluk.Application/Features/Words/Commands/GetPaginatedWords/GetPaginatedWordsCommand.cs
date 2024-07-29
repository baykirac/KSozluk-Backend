using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.GetPaginatedWords
{
    public class GetPaginatedWordsCommand : CommandBase<GetPaginatedWordsResponse>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

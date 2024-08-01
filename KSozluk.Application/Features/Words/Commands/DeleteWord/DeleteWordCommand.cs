using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.DeleteWord
{
    public class DeleteWordCommand : CommandBase<DeleteWordResponse>
    {
        public Guid WordId { get; set; }
    }
}

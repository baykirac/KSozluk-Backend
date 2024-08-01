using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.UpdateOrder
{
    public class UpdateOrderCommand : CommandBase<UpdateOrderResponse>
    {
        public Guid DescriptionId { get; set; }
        public int Order {  get; set; }
    }
}

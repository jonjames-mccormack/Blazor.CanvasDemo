using System;

namespace Blazor.CanvasDemo.Shared.ComplexCanvas
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid PlanGroupId { get; set; }
        public string PlanGroupName { get; set; }
        public Guid? PlanRowId { get; set; }
        public string PlanRowName { get; set; }
        public Guid ProcessId { get; set; }
        public string ProcessName { get; set; }
        public DateTime PlannedPreProductionDate { get; set; }
        public DateTime FirstProductionDate { get; set; }
        public DateTime LastProductionDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public long WorkContentValue { get; set; }
        public long Quantity { get; set; }
        public long Minutes { get; set; }
        public string OrderName { get; set; }
        public string Customer { get; set; }
        public DateTime OrderReceivedDate { get; set; }
        public string LineReference { get; set; }
        public string Style { get; set; }
        public string StyleOption { get; set; }
        public string[] StyleColours { get; set; }
        public string ProductionTemplate { get; set; }
        public string OrderType { get; set; }
        public string OrderTypeColour { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusColour { get; set; }
        public string Destination { get; set; }
        public StripSchedule[] StripSchedule { get; set; }
    }

    public partial class StripSchedule
    {
        public DateTime PlannedDate { get; set; }
        public double Quantity { get; set; }
        public long WorkingTime { get; set; }
    }
}
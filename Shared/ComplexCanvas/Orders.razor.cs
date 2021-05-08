using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Shared.ComplexCanvas
{
    public partial class Orders : ComponentBase
    {
        [Parameter]
        public DateTime StartDate { get; set; }

        [Parameter]
        public DateTime EndDate { get; set; }

        [Parameter]
        public DateTime TodaysDate { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }

        protected Canvas2DContext _context;
        private int _width;
        private int _height;
        private int _panelWidth;
        private int _dayWidth;
        private int _orderHeight;
        private Order[] _orders;
        private Stopwatch _stopwatch;

        protected BECanvasComponent ordersCanvas;
        protected int frameTimeInMilliseconds;

        protected override async Task OnInitializedAsync()
        {
            _stopwatch = new Stopwatch();
            _orders = await Http.GetFromJsonAsync<Order[]>("sample-data/orders.json");
            _context = await ordersCanvas.CreateCanvas2DAsync();

            await RenderOrders();
        }

        protected virtual async Task DrawRowBackgroundAsync(int rowTop)
        {
            await _context.SetFillStyleAsync("#CCFFFF");
            await _context.FillRectAsync(_panelWidth, rowTop + 1, _dayWidth * (TodaysDate - StartDate).TotalDays, _orderHeight);
        }

        protected virtual async Task DrawRowDeliveryDateAsync(Order order, int rowTop)
        {
            var deliveryDateHorizontalOffset = _dayWidth * (order.DeliveryDate - StartDate).TotalDays;
            if (deliveryDateHorizontalOffset < 0)
            {
                deliveryDateHorizontalOffset = 0;
            }

            await _context.SetFillStyleAsync("#CCFFFF");
            await _context.FillRectAsync(_panelWidth + deliveryDateHorizontalOffset, rowTop + 1, _width, _orderHeight);
        }

        protected virtual async Task DrawOrderAsync(Order order, int rowTop, string orderFillColour, string orderBorderColour)
        {
            // Calculate the start/end positions on the row, and the difference to figure the width
            var startHorizontalOffset = _dayWidth * (order.FirstProductionDate - StartDate).TotalDays;
            var targetHorizontalOffset = _dayWidth * (order.LastProductionDate - StartDate).TotalDays;
            var preProdHorizontalOffset = _dayWidth * (order.PlannedPreProductionDate - StartDate).TotalDays;
            var deliveryHorizontalOffset = _dayWidth * (order.ExpectedDeliveryDate - StartDate).TotalDays;

            // Draw PREPRODUCTION TAIL (Connects to FirstProductDate)
            await _context.SetFillStyleAsync("#008000");
            await _context.FillRectAsync(_panelWidth + preProdHorizontalOffset, rowTop + 5, startHorizontalOffset - preProdHorizontalOffset, 3);

            // Draw DELIVERY HEAD (Connected to LastProductionDate)
            await _context.SetFillStyleAsync("#800080");
            await _context.FillRectAsync(_panelWidth + targetHorizontalOffset, rowTop + 5, deliveryHorizontalOffset - targetHorizontalOffset, 3);

            // Draw ORDER STRIP (FirstProductionDate to LastProductionDate)
            await _context.SetFillStyleAsync(orderFillColour);
            await _context.FillRectAsync(_panelWidth + startHorizontalOffset + 0.5, rowTop + 1.5, targetHorizontalOffset - startHorizontalOffset + _dayWidth, 10);
            await _context.SetStrokeStyleAsync(orderBorderColour);
            await _context.StrokeRectAsync(_panelWidth + startHorizontalOffset + 0.5, rowTop + 1.5, targetHorizontalOffset - startHorizontalOffset + _dayWidth, 10);
        }

        protected virtual async Task DrawRowDetailAndTextAsync(Order order, int rowTop, string orderFillColour, string orderBorderColour)
        {
            // Draw dashed line on the bottom of the row
            await _context.BeginPathAsync();
            await _context.SetLineDashAsync(new float[] { 3, 3 });
            await _context.SetStrokeStyleAsync("#C0C0C0");
            await _context.MoveToAsync(0, rowTop + _orderHeight + 0.5);
            await _context.LineToAsync(_width, rowTop + _orderHeight + 0.5);
            await _context.StrokeAsync();
            await _context.SetLineDashAsync(Array.Empty<float>());

            // Draw a line to seaparate the panel from the row         
            await _context.SetStrokeStyleAsync("#CCCCCC");
            await _context.BeginPathAsync();
            await _context.MoveToAsync(_panelWidth + 0.5, rowTop);
            await _context.LineToAsync(_panelWidth + 0.5, rowTop + _orderHeight);
            await _context.StrokeAsync();

            // Clear the panel area of any overrun drawings so we can write the row info
            await _context.ClearRectAsync(0, rowTop + 1, _panelWidth, _orderHeight - 1);

            // Draw Order Line text
            await _context.SetFontAsync("bold 11px Arial");
            await _context.SetFillStyleAsync("#454545");
            await _context.FillTextAsync(order.OrderName + " / " + order.LineReference, 18, rowTop + 13);

            // Draw Order Type Swatch
            if (!string.IsNullOrWhiteSpace(order.OrderTypeColour))
            {
                await _context.SetFillStyleAsync(order.OrderTypeColour);
                await _context.FillRectAsync(6, rowTop + 21, 8, 8);
                await _context.SetStrokeStyleAsync(orderBorderColour);
                await _context.StrokeRectAsync(6.5, rowTop + 21 + 0.5, 7, 7);
            }

            // Draw Order Strip Status Swatch
            await _context.SetFillStyleAsync(orderFillColour);
            await _context.FillRectAsync(_panelWidth - 16, rowTop + 5, 8, 26);
            await _context.SetStrokeStyleAsync(orderBorderColour);
            await _context.StrokeRectAsync(_panelWidth - 15.5, rowTop + 5 + 0.5, 7, 25);

            // Draw Order Status Swatch
            if (!string.IsNullOrWhiteSpace(order.OrderStatusColour))
            {
                await _context.SetFillStyleAsync(order.OrderStatusColour);
                await _context.FillRectAsync(6, rowTop + 5, 8, 8);
                await _context.SetStrokeStyleAsync(orderBorderColour);
                await _context.StrokeRectAsync(6.5, rowTop + 5 + 0.5, 7, 7);
            }

            // Order Subtitle Text - "OrderType / OrderStatus"
            var orderSubtitleText = string.IsNullOrWhiteSpace(order.OrderType)
                ? order.OrderStatus
                : order.OrderType + " / " + order.OrderStatus;
            await _context.SetFontAsync("11px Arial");
            await _context.SetFillStyleAsync("#454545");
            await _context.FillTextAsync(orderSubtitleText, 18, rowTop + 29);
        }

        protected virtual async Task<int> DrawRowOrderTextAsync(string text, int rowTop, int startPosition, int extraWidth)
        {
            text ??= "";

            await _context.SetFontAsync("11px Arial");
            await _context.SetFillStyleAsync("#454545");

            var textPosition = 5 + startPosition + _panelWidth;
            var measuredWidth = await _context.MeasureTextAsync(text);
            while (measuredWidth.Width > 65 + extraWidth)
            {
                text = text[0..^1];
                measuredWidth = await _context.MeasureTextAsync(text);
            }

            await _context.FillTextAsync(text, textPosition, rowTop + _orderHeight - 4);

            return startPosition + 90 + extraWidth;
        }

        protected virtual async Task DrawRowTextAsync(Order order, int rowTop)
        {
            var startPosition = 0;
            startPosition = await DrawRowOrderTextAsync("", rowTop, startPosition, -50);
            startPosition = await DrawRowOrderTextAsync(order.Customer, rowTop, startPosition, 10);
            startPosition = await DrawRowOrderTextAsync(order.Style, rowTop, startPosition, 20);
            startPosition = await DrawRowOrderTextAsync(order.StyleOption, rowTop, startPosition, 20);

            var styleColours = string.Join(", ", order.StyleColours);
            startPosition = await DrawRowOrderTextAsync(styleColours, rowTop, startPosition, 20);

            var quantityText = order.Quantity.ToString() + " pcs / " + order.Minutes.ToString() + " mins";
            startPosition = await DrawRowOrderTextAsync(quantityText, rowTop, startPosition, 90);

            startPosition = await DrawRowOrderTextAsync(order.ProductionTemplate, rowTop, startPosition, 20);
            startPosition = await DrawRowOrderTextAsync(order.Destination, rowTop, startPosition, 0);
            startPosition = await DrawRowOrderTextAsync(order.PlanGroupName, rowTop, startPosition, 10);
            await DrawRowOrderTextAsync(order.PlanRowName, rowTop, startPosition, 0);
        }

        protected virtual async Task DrawRowAsync(Order order, int offset)
        {
            var rowTop = _orderHeight * offset;
            var orderFillColour = "#C0C0C0";
            if (order.ExpectedDeliveryDate > order.DeliveryDate)
            {
                orderFillColour = "#00FFFF";
            }
            else if (order.PlannedPreProductionDate > TodaysDate)
            {
                orderFillColour = "#cc3d55";
            }
            var orderBorderColour = "#000000";

            // Draw an aqua background to signify the region that is before today
            await DrawRowBackgroundAsync(rowTop);
            // Draw an aqua background to signify the region that is after the delivery date
            await DrawRowDeliveryDateAsync(order, rowTop);
            // Draw the order in the row
            await DrawOrderAsync(order, rowTop, orderFillColour, orderBorderColour);
            // Draw the final parts of the row and panel
            await DrawRowDetailAndTextAsync(order, rowTop, orderFillColour, orderBorderColour);
            // Draw Row Text
            await DrawRowTextAsync(order, rowTop);
        }

        protected virtual async Task RenderOrders()
        {
            if (_orders == null || _orders.Length == 0)
            {
                return;
            }

            _stopwatch.Reset();
            _stopwatch.Start();

            _width = (int)ordersCanvas.Width;
            _height = (int)ordersCanvas.Height;
            _panelWidth = 220;
            _dayWidth = 12;
            _orderHeight = 36;

            await _context.ClearRectAsync(0, 0, _width, _height);

            for (var i = 0; i < _orders.Length; i++)
            {
                await DrawRowAsync(_orders[i], i);
            }

            _stopwatch.Stop();
            frameTimeInMilliseconds = (int)_stopwatch.ElapsedMilliseconds;
        }
    }
}

using Blazor.CanvasDemo.Shared.ComplexCanvas;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Shared.BatchedCanvas
{
    public partial class BatchedOrders : Orders
    {
        public enum BatchMode
        {
            MethodBatch,
            RenderBatch,
            NoBatch
        }

        public BatchMode CurrentBatchMode { get; set; } = BatchMode.MethodBatch;

        protected override async Task DrawRowBackgroundAsync(int rowTop)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawRowBackgroundAsync(rowTop);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawRowBackgroundAsync(rowTop);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task DrawRowDeliveryDateAsync(Order order, int rowTop)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawRowDeliveryDateAsync(order, rowTop);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawRowDeliveryDateAsync(order, rowTop);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task DrawOrderAsync(Order order, int rowTop, string orderFillColour, string orderBorderColour)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawOrderAsync(order, rowTop, orderFillColour, orderBorderColour);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawOrderAsync(order, rowTop, orderFillColour, orderBorderColour);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task DrawRowDetailAndTextAsync(Order order, int rowTop, string orderFillColour, string orderBorderColour)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawRowDetailAndTextAsync(order, rowTop, orderFillColour, orderBorderColour);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawRowDetailAndTextAsync(order, rowTop, orderFillColour, orderBorderColour);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task<int> DrawRowOrderTextAsync(string text, int rowTop, int startPosition, int extraWidth)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                return await base.DrawRowOrderTextAsync(text, rowTop, startPosition, extraWidth);
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                var value = await base.DrawRowOrderTextAsync(text, rowTop, startPosition, extraWidth);
                await _context.EndBatchAsync();

                return value;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task DrawRowTextAsync(Order order, int rowTop)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawRowTextAsync(order, rowTop);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawRowTextAsync(order, rowTop);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task DrawRowAsync(Order order, int offset)
        {
            if (CurrentBatchMode == BatchMode.RenderBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.DrawRowAsync(order, offset);
                return;
            }

            if (CurrentBatchMode == BatchMode.MethodBatch)
            {
                await _context.BeginBatchAsync();
                await base.DrawRowAsync(order, offset);
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }

        protected override async Task RenderOrders()
        {
            if (CurrentBatchMode == BatchMode.MethodBatch || CurrentBatchMode == BatchMode.NoBatch)
            {
                await base.RenderOrders();
                return;
            }

            if (CurrentBatchMode == BatchMode.RenderBatch)
            {
                await _context.BeginBatchAsync();
                await base.RenderOrders();
                await _context.EndBatchAsync();
                return;
            }

            throw new NotImplementedException($"Render mode {CurrentBatchMode} is not supported.");
        }
    }
}

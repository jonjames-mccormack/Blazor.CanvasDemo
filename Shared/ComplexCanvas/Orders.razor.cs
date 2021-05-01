using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Shared.ComplexCanvas
{
    public partial class Orders
    {
        private Canvas2DContext _context;

        protected BECanvasComponent ordersCanvas;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            this._context = await this.ordersCanvas.CreateCanvas2DAsync();
            await this._context.SetFillStyleAsync("green");

            await this._context.FillRectAsync(0, 0, ordersCanvas.Width, ordersCanvas.Height);
        }
    }
}

using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Pages
{
    public partial class CanvasTutorial : ComponentBase
    {
        private Canvas2DContext _context;

        protected BECanvasComponent _canvasReference;
        protected int frameTimeInMilliseconds;

        protected async Task RenderHelloBlazorAsync()
        {
            _context = await _canvasReference.CreateCanvas2DAsync();

            await _context.ClearRectAsync(0, 0, _canvasReference.Width, _canvasReference.Height);

            await _context.SetFillStyleAsync("green");
            await _context.FillRectAsync(10, 100, 100, 100);

            await _context.SetFontAsync("48px serif");
            await _context.StrokeTextAsync("Hello Blazor!!!", 10, 100);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var frameTimeStopwatch = new Stopwatch();
            frameTimeStopwatch.Start();

            await RenderHelloBlazorAsync();

            frameTimeStopwatch.Stop();
            frameTimeInMilliseconds = (int)frameTimeStopwatch.ElapsedMilliseconds;
        }
    }
}

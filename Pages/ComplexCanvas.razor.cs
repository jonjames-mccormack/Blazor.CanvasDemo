using Blazor.CanvasDemo.Shared.ComplexCanvas;
using System;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Pages
{
    public partial class ComplexCanvas
    {
        protected DateTime StartDate => new DateTime(2021, 2, 27);
        protected DateTime EndDate => new DateTime(2021, 6, 14);
        protected DateTime TodaysDate => new DateTime(2021, 3, 13);
        protected Timeline timeline;
        protected Orders orders;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }
    }
}

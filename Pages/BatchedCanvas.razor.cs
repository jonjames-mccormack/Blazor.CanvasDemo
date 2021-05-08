using Blazor.CanvasDemo.Shared.ComplexCanvas;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Pages
{
    public partial class BatchedCanvas : ComponentBase
    {
        protected static DateTime StartDate => new(2021, 2, 27);
        protected static DateTime EndDate => new(2021, 6, 14);
        protected static DateTime TodaysDate => new(2021, 3, 13);

        protected Timeline timeline;
        protected Orders orders;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }
    }
}

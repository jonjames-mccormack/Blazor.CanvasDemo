using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.CanvasDemo.Shared.ComplexCanvas
{
    public partial class Timeline
    {
        [Parameter]
        public DateTime StartDate { get; set; }
        
        [Parameter]
        public DateTime EndDate { get; set; }
        
        [Parameter]
        public DateTime TodaysDate { get; set; }

        private Canvas2DContext _context;
        private int _width;
        private int _height;
        private int _panelWidth;
        private int _dayWidth;

        protected BECanvasComponent timelineCanvas;

        private async Task DrawDayBackgroundAsync(DateTime day, int drawHorizontalPosition)
        {
            // Draw a white background accross the timeline area (previous dates in light blue)
            await _context.SetFillStyleAsync(day < TodaysDate ? "#CCFFFF" : "#ffffff");
            await _context.FillRectAsync(drawHorizontalPosition, 0, _dayWidth, _height);
        }

        private async Task DrawWeekHeaderAsync(DateTime day, int drawHorizontalPosition, bool firstRow)
        {
            //If this is the 1st of the Week
            if (day.DayOfWeek == DayOfWeek.Monday) 
            {
                var weekText = day.ToShortDateString();
                var weekTextWidth = (await _context.MeasureTextAsync(weekText)).Width;
                var weekWidth = _dayWidth * 7;

                var weekTextX = weekWidth / 2 - weekTextWidth / 2 + drawHorizontalPosition;
                var ceiling = firstRow ? 0 : _height / 2;
                var floor = firstRow ? _height / 2 : _height;

                await _context.SetFontAsync("11px Arial");
                await _context.SetFillStyleAsync("#454545");
                await _context.FillTextAsync(weekText, weekTextX, floor - 3);

                //Draw the separator
                await _context.SetStrokeStyleAsync("#CCCCCC");
                await _context.MoveToAsync(drawHorizontalPosition + 0.5, ceiling);
                await _context.LineToAsync(drawHorizontalPosition + 0.5, floor);
                await _context.StrokeAsync();
            }
        }

        private async Task DrawDayHeaderAsync(DateTime day, int drawHorizontalPosition) 
        {
            //Draw the day letter on the bottom half
            var dayText = "";
            switch (day.DayOfWeek) 
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    dayText = "S";
                    break;
                case DayOfWeek.Monday:
                    dayText = "M";
                    break;
                case DayOfWeek.Tuesday:
                    dayText = "T";
                    break;
                case DayOfWeek.Wednesday:
                    dayText = "W";
                    break;
                case DayOfWeek.Thursday:
                    dayText = "T";
                    break;
                case DayOfWeek.Friday:
                    dayText = "F";
                    break;
            }

            await _context.SetFontAsync("11px Arial");
            await _context.SetFillStyleAsync("#454545");
            
            var dayTextWidth = (await _context.MeasureTextAsync(dayText)).Width;
            var dayTextX = _dayWidth / 2 - dayTextWidth / 2 + drawHorizontalPosition;
            var dayTextY = _height - 3;
            await _context.FillTextAsync(dayText, dayTextX, dayTextY);


            await _context.SetStrokeStyleAsync("#CCCCCC");
            await _context.MoveToAsync(drawHorizontalPosition + 0.5, _height / 2);
            await _context.LineToAsync(drawHorizontalPosition + 0.5, _height);
            await _context.StrokeAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _width = (int)timelineCanvas.Width;
            _height = (int)timelineCanvas.Height;
            _panelWidth = 220;
            _dayWidth = 12;
            _context = await timelineCanvas.CreateCanvas2DAsync();

            await _context.BeginPathAsync();
            await _context.MoveToAsync(1, 1);

            await _context.MoveToAsync(_panelWidth + 0.5, 0);
            await _context.LineToAsync(_panelWidth + 0.5, _height);

            await _context.MoveToAsync(_panelWidth, _height - 0.5);
            await _context.LineToAsync(_width, _height - 0.5);

            await _context.MoveToAsync(_panelWidth, _height / 2 - 0.5);
            await _context.LineToAsync(_width, _height / 2 - 0.5);

            await _context.SetLineWidthAsync(1);
            await _context.SetStrokeStyleAsync("#000000");
            await _context.StrokeAsync();

            var dateToDraw = StartDate;
            var drawingStartPosition = _panelWidth + 7;

            // First we loop and draw Background for day
            while (drawingStartPosition < _width && dateToDraw <= EndDate) 
            {
                await DrawDayBackgroundAsync(dateToDraw, drawingStartPosition);

                dateToDraw = dateToDraw.AddDays(1);
                drawingStartPosition += _dayWidth;
            }

            // Reset boundary variables
            dateToDraw = StartDate;
            drawingStartPosition = _panelWidth + 7;

            // Second loop - draw boxes and text
            while (drawingStartPosition < _width && dateToDraw <= EndDate) 
            {
                // [   WEEK   ]
                // [D][D][D][D]
                await DrawWeekHeaderAsync(dateToDraw, drawingStartPosition, true);
                await DrawDayHeaderAsync(dateToDraw, drawingStartPosition);
                
                dateToDraw = dateToDraw.AddDays(1);
                drawingStartPosition += _dayWidth;
            }
        }
    }
}

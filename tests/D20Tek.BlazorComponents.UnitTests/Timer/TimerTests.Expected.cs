namespace D20Tek.BlazorComponents.UnitTests.Timer;

public partial class TimerTests
{
    internal static class Expected
    {
        public const string Default = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">0:30</div>
  </div>
</div>
";

        public const string WithParameters = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: lightgreen""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: darkgray"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">1:00</div>
  </div>
</div>
";

        public const string WithWarningColor = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: orange"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">0:30</div>
  </div>
</div>
";

        public const string WithExpirationMessage = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""NaN 283"" class=""base-timer__path-remaining""
            style=""stroke: red"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">Test!</div>
  </div>
</div>
";

        public const string WithSizeLarge = @$"
<div role=""timer"" class=""base-timer base-timer-lg"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">0:30</div>
  </div>
</div>
";

        public const string WithSizeSmall = @$"
<div role=""timer"" class=""base-timer base-timer-sm"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">0:30</div>
  </div>
</div>
";
    }
}
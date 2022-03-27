# Release Notes

## Release v1.0.12
* Refactored code between TimerBase and RadialTimer to better support text-only CountdownTimer.
* Created initial Countdown timer to display countdown to a specific DateTimeOffset.
* Updated display formatting to include days, hours, min, sec.
* Added unit tests for CountdownTimer.
* Created CountdownTimer sample page to show off its functionality.
* Added splash screen for Sample app.
* Added simple LabelText for CountdownTimer.
* Added timer Size property rendering and css.

## Release v1.0.11
* Updated Timer component to show hours:min:sec.
* Added invalid error handling for Timer properties that deal with time intervals.
* Refactored timer code into base classes that could be used for new Timer components.
* Created SpanTimer component with TimerDurationSpan property, which takes a TimeSpan object as input.
* Updated sample project and unit tests for refactoring and new component.
* Refactored time display formatting into helper class.

## Release v1.0.10
* Created simple Timer component project and add Timer page to Sample app.
* Moved the shared project into its own Core package and change dependencies for consuming it.
* Created new BaseComponent with properties and abstract methods used for all Blazor components.
* Created basic Timer component that supports minutes and seconds with a radial display.
* Added TimeRemaining property and IDisposable implementation to cleanup system Timer.
* Added Size functionality to Timer component.
* Added background color parameter for elapsed section.
* Added color and time interval customization to TimerPage.
* Added custom expiration message to show once timer completes.

## Release v1.0.7
* Initial Spinner component project and unit tests.
* Created full Blazor sample to show usage of various components with first page about Spinners.
* Build actions for CI/CD, official build in main, and package releases.
* Add various types of spinners: ring, square, pulse, hourglass, and dual ring.

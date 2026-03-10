# Release Notes

## Release v1.10.9
* Implemented MessageBox component with service-based architecture for displaying alerts and confirmation dialogs:
  * Service-Based API - Inject IMessageBoxService to show dialogs from anywhere in the application
  * 5 Message Types - Information, Success, Warning, Error, Question with built-in SVG icons
  * 4 Button Configurations - Ok, OkCancel, YesNo, YesNoCancel with MessageBoxResult return values
  * Position Variants - Top (default), Center, Bottom of viewport
  * Async/Await Pattern - All methods return Task or Task<MessageBoxResult> for proper async flow
  * Helper Methods - ShowAsync, ConfirmAsync, ShowErrorAsync, ShowWarningAsync, ShowSuccessAsync
  * Consistent Styling - Reuses ModalDialog CSS classes with message-box specific additions
  * Sample Page - MessageBoxPage demonstrates all message types, button configurations, and positioning

## Release v1.10.8
* Implemented MarkdigRenderer (for IMarkdownRenderer) to take markdown text and render it as html.
* Implemented MarkdownView that has Markdown parameter and used IMarkdownRenderer to convert it to html.
* Implemented AddMarkdownRenderer helper to register default renderer with DI.
* Added unit tests for this component and helper classes.
* Added MarkdownViewPage to the FullSample.Wasm project to show how this component can be used.

## Release v1.10.6
* Implemented ModalFormDialog component with built-in EditForm support:
  * EditForm Integration - Wraps body and footer in EditForm for native form submission
  * Validation Support - Compatible with DataAnnotationsValidator and any Blazor validator
  * OnValidSubmit / OnInvalidSubmit - Separate callbacks for valid and invalid form submissions
  * OnCancel callback - Fires when Cancel or Close button is clicked
  * Required Model parameter - Passed directly to EditForm
  * Consistent styling - Reuses all ModalDialog CSS classes

## Release v1.10.5
* Implemented Modal component with basic modal dialog functionality:
  * Native HTML5 Dialog - Built on <dialog> element for built-in accessibility, focus trapping, and backdrop support
  * Customizable Header - Title and optional summary text
  * Flexible Content - Render any Blazor content in the modal body
  * Configurable Buttons - Show/hide Cancel and Submit buttons with custom text
  * Size Variants - Small, Medium (default), Large, ExtraLarge
  * Position Variants - Top, Center (default), Bottom of viewport
  * Event Callbacks - OnClose and OnSubmit for handling user actions
  * Keyboard Support - Native Escape key to close

## Release v1.10.1
* Updated library projects to support .net 9 & 10.
* Upgraded all dependent packages to latest versions.
* Updated build scripts to require .net 10.

## Release v1.9.4
* Updated all dependent packages to latest versions.

## Release v1.9.3
* Updated all dependent packages to latest versions.

## Release v1.9.1
* Upgrades component and sample projects to .NET 9.
* Updated all dependent packages to latest versions.

## Release v1.0.13
* Created initial ToggleSwitch project.
* Implemented basic ToggleSwitch functionality.
* Created ToggleSwitchPage sample code.
* Added unit tests for ToggleSwitch.

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

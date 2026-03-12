using System.Threading.Tasks;
using BlazorComponents = D20Tek.BlazorComponents;

namespace D20Tek.BlazorComponents.UnitTests.TogglePanel;

[TestClass]
public class TogglePanelEventsTests
{
    [TestMethod]
    public async Task OnToggle_FiresWithTrueWhenOpened()
    {
        // arrange
        var ctx = new BunitContext();
        bool? toggleResult = null;
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.IsOpen, false)
                      .Add(p => p.OnToggle, (bool v) => toggleResult = v));

        // act
        await comp.Find("summary").TriggerEventAsync("onclick", new EventArgs());

        // assert
        Assert.IsTrue(toggleResult);
    }

    [TestMethod]
    public async Task OnToggle_FiresWithFalseWhenClosed()
    {
        // arrange
        var ctx = new BunitContext();
        bool? toggleResult = null;
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.IsOpen, true)
                      .Add(p => p.OnToggle, (bool v) => toggleResult = v));

        // act
        await comp.Find("summary").TriggerEventAsync("onclick", new EventArgs());

        // assert
        Assert.IsFalse(toggleResult);
    }

    [TestMethod]
    public async Task IsOpenChanged_FiresOnToggle()
    {
        // arrange
        var ctx = new BunitContext();
        bool? changedValue = null;
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.IsOpen, false)
                      .Add(p => p.IsOpenChanged, (bool v) => changedValue = v));

        // act
        await comp.Find("summary").TriggerEventAsync("onclick", new EventArgs());

        // assert
        Assert.IsTrue(changedValue);
    }

    [TestMethod]
    public async Task HandleToggle_UpdatesIsOpenState()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.IsOpen, false));

        // act
        await comp.Find("summary").TriggerEventAsync("onclick", new EventArgs());

        // assert
        Assert.IsNotNull(comp.Find("details").GetAttribute("open"));
    }

    [TestMethod]
    public async Task BothCallbacks_FireOnToggle()
    {
        // arrange
        var ctx = new BunitContext();
        bool? onToggleValue = null;
        bool? isOpenChangedValue = null;
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.IsOpen, false)
                      .Add(p => p.OnToggle, (bool v) => onToggleValue = v)
                      .Add(p => p.IsOpenChanged, (bool v) => isOpenChangedValue = v));

        // act
        await comp.Find("summary").TriggerEventAsync("onclick", new EventArgs());

        // assert
        Assert.IsTrue(onToggleValue);
        Assert.IsTrue(isOpenChangedValue);
    }
}

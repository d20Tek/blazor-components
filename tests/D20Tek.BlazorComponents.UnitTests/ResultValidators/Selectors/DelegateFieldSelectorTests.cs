using D20Tek.BlazorComponents.Selectors;
using Microsoft.AspNetCore.Components.Forms;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class DelegateFieldSelectorTests
{
    private static FieldSelectorContext CreateContext()
    {
        var model = new object();
        return new FieldSelectorContext(new EditContext(model), model.GetType());
    }

    [TestMethod]
    public void FuncErrorString_Ctor_InvokesDelegate()
    {
        // arrange
        var sut = new DelegateFieldSelector(e => "mapped_" + e.Code);

        // act
        var result = sut.GetFieldNames(Error.Validation("X", "msg"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["mapped_X"], result);
    }

    [TestMethod]
    public void FuncErrorContextString_Ctor_ReceivesContext()
    {
        // arrange
        FieldSelectorContext? seen = null;
        var sut = new DelegateFieldSelector((e, ctx) =>
        {
            seen = ctx;
            return e.Code + "!";
        });
        var context = CreateContext();

        // act
        var result = sut.GetFieldNames(Error.Validation("Y", "msg"), context).ToArray();

        // assert
        Assert.AreSame(context, seen);
        Assert.AreSequenceEqual(["Y!"], result);
    }

    [TestMethod]
    public void FuncEnumerable_Ctor_ReturnsAllValues()
    {
        // arrange
        var sut = new DelegateFieldSelector((e, _) => ["a", "b", e.Code]);

        // act
        var result = sut.GetFieldNames(Error.Validation("Z", "msg"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["a", "b", "Z"], result);
    }

    [TestMethod]
    public void FuncErrorString_NullDelegate_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new DelegateFieldSelector((Func<Error, string>)null!));
    }

    [TestMethod]
    public void FuncErrorContextString_NullDelegate_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new DelegateFieldSelector((Func<Error, FieldSelectorContext, string>)null!));
    }

    [TestMethod]
    public void FuncEnumerable_NullDelegate_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new DelegateFieldSelector((Func<Error, FieldSelectorContext, IEnumerable<string>>)null!));
    }
}

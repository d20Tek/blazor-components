# D20Tek.BlazorComponents.Vertically

Blazor UI components that integrate with the [D20Tek.Vertically](https://www.nuget.org/packages/D20Tek.Vertically)
framework.

## OffsetPager&lt;T&gt;

`OffsetPager<T>` is a strongly-typed pagination component that binds to a `PageOf<T>` paged result
and raises a page-query callback when the user navigates. It composes the content-agnostic `Pager`
from `D20Tek.BlazorComponents.Pager` and renders the pagination controls only; you render the list
of items yourself.

```razor
<OffsetPager Page="currentPage"
			 OnPageQuery="LoadPageAsync"
			 ShowFirstLast="true"
			 ShowDescription="true"
			 ShowPageSizeSelector="true" />
```

```csharp
private PageOf<Item> currentPage = PageOf<Item>.Empty(new PagedRequest());

private async Task LoadPageAsync(PagedRequest request)
{
	currentPage = await _service.GetPageAsync(request);
}
```

Because the underlying `Pager` uses scoped (isolated) CSS, no consumer stylesheet `<link>` is required.

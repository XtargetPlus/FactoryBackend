using DB.Model.ProductInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.ProductO;

public static class ProductOptions
{
    public static IQueryable<Product> ProductSearch(this IQueryable<Product> products, string text = "", SerialNumberOrTitleFilter productSearch = SerialNumberOrTitleFilter.ForSerialNumber)
    {
        if (string.IsNullOrEmpty(text))
            return products;
        return productSearch switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => products.Where(p => p.Detail!.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => products.Where(p => p.Detail!.Title.Contains(text)),
            _ => products
        };
    }

    public static IQueryable<Product> ProductOrder(this IQueryable<Product> products, ProductOrderOptions orderOptions = default, KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            ProductOrderOptions.ForTitle => kindOfOrder switch
            {
                KindOfOrder.Up => products.OrderBy(p => p.Detail!.Title),
                KindOfOrder.Down => products.OrderByDescending(p => p.Detail!.Title),
                _ => products.OrderBy(p => p.Price)
            },
            ProductOrderOptions.ForSerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => products.OrderBy(p => p.Detail!.SerialNumber),
                KindOfOrder.Down => products.OrderByDescending(p => p.Detail!.SerialNumber),
                _ => products.OrderBy(d => d.Id)
            },
            ProductOrderOptions.ForPrice => kindOfOrder switch
            {
                KindOfOrder.Up => products.OrderBy(p => p.Price),
                KindOfOrder.Down => products.OrderByDescending(p => p.Price),
                _ => products.OrderBy(d => d.Id)
            },
            _ => products.OrderBy(d => d.Id)
        };
    }
}
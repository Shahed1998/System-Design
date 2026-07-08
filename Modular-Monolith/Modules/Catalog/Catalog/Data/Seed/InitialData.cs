namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(new Guid(), "Iphone X", new List<string>() { "Category 1" }, "Abcd", "", 1000),
            Product.Create(new Guid(), "Toys", new List<string>() { "Category 2" }, "Abcd", "", 50),
            Product.Create(new Guid(), "Books", new List<string>() { "Category 3" }, "Abcd", "", 120)
        };
    }
}

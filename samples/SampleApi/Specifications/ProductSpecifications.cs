using SampleApi.Models;

namespace SampleApi.Specifications
{
    /// <summary>
    /// Collection of specifications for working with products.
    /// </summary>
    public static class ProductSpecifications
    {
        /// <summary>
        /// Gets the specification for products by category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The specification.</returns>
        public static ProductSpecification ByCategory(int categoryId)
        {
            return new ProductSpecification(p => p.CategoryId == categoryId);
        }

        /// <summary>
        /// Gets the specification for products by supplier.
        /// </summary>
        /// <param name="supplierId">The supplier identifier.</param>
        /// <returns>The specification.</returns>
        public static ProductSpecification BySupplier(int supplierId)
        {
            return new ProductSpecification(p => p.SupplierId == supplierId);
        }

        /// <summary>
        /// Gets the specification for products by price range.
        /// </summary>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <returns>The specification.</returns>
        public static ProductSpecification ByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return new ProductSpecification(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        /// <summary>
        /// Gets the specification for products with stock below threshold.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <returns>The specification.</returns>
        public static ProductSpecification LowStock(int threshold)
        {
            return new ProductSpecification(p => p.StockQuantity < threshold);
        }

        /// <summary>
        /// Gets the specification for products with included relationships.
        /// </summary>
        /// <returns>The specification.</returns>
        public static ProductSpecification WithRelationships()
        {
            var spec = new ProductSpecification();
            spec.AddInclude(p => p.Category);
            spec.AddInclude(p => p.Supplier);
            return spec;
        }

        /// <summary>
        /// Gets the specification for products by name search.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The specification.</returns>
        public static ProductSpecification ByNameSearch(string searchTerm)
        {
            return new ProductSpecification(
                p => p.Name.Contains(searchTerm) || 
                     (p.Description != null && p.Description.Contains(searchTerm)));
        }
    }
}
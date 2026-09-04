namespace ProductsApi.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; private set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Product() { }

        public Product(string name, string description, decimal price, int initialStock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = initialStock;
        }

        // Regla de negocio: el stock nunca puede quedar negativo.
        // Vive AQUÍ (en el dominio) y no en el controller, porque es una regla
        // que debe cumplirse sin importar quién llame a este método.
        public void AdjustStock(int quantity)
        {
            var newStock = Stock + quantity;
            if (newStock < 0)
                throw new InvalidOperationException(
                    $"Stock insuficiente. Stock actual: {Stock}, se intentó ajustar en {quantity}.");

            Stock = newStock;
        }
    }
}
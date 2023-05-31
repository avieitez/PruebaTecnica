using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        // Leer los archivos CSV y generar la lista de categorías y productos
        List<Category> categories = ReadCategories(@"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Catalog\Categories.csv");
        List<Product> products = ReadProducts(@"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Catalog\Products.csv");

        // Generar el catálogo en formato JSON
        string json = GenerateCatalogJSON(categories, products);

        // Guardar el catálogo en un archivo JSON
        SaveCatalogAsJSON(json, @"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Fuentes\Catalog\Catalog.json");

        // Generar el catálogo en formato XML
        XDocument xml = GenerateCatalogXML(categories, products);

        // Guardar el catálogo en un archivo XML
        SaveCatalogAsXML(xml, @"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Fuentes\Catalog\Catalog.xml");

        Console.WriteLine("Catálogo generado correctamente.");
        Console.ReadLine();
    }

    static List<Category> ReadCategories(string filePath)
    {
        List<Category> categories = new List<Category>();

        string[] lines = File.ReadAllLines(filePath);

        int contador = 0;

        foreach (string line in lines)
        {
            if (contador > 0 )
            {
                string[] data = line.Split(';');

                Category category = new Category
                {
                    Id = data[0],
                    Name = data[1],
                    Description = data[2]
                };

                categories.Add(category);
            }

            contador++;
        }

        return categories;
    }

    static List<Product> ReadProducts(string filePath)
    {
        List<Product> products = new List<Product>();

        string[] lines = File.ReadAllLines(filePath);

        int contador = 0;

        foreach (string line in lines)
        {
            if (contador > 0)
            {
                string[] data = line.Split(';');

                Product product = new Product
                {
                    Id = data[0],
                    CategoryId = data[1],
                    Name = data[2],
                    Price = data[3]
                };

                products.Add(product);
            }

            contador++;
        }

        return products;
    }

    static string GenerateCatalogJSON(List<Category> categories, List<Product> products)
    {
        List<CatalogCategory> catalogCategories = new List<CatalogCategory>();

        foreach (Category category in categories)
        {
            List<Product> categoryProducts = products.FindAll(p => p.CategoryId == category.Id);

            CatalogCategory catalogCategory = new CatalogCategory
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Products = categoryProducts
            };

            catalogCategories.Add(catalogCategory);
        }

        return JsonConvert.SerializeObject(catalogCategories, Formatting.Indented);
    }

    static void SaveCatalogAsJSON(string json, string filePath)
    {
        File.WriteAllText(filePath, json);
    }

    static XDocument GenerateCatalogXML(List<Category> categories, List<Product> products)
    {
        XDocument xml = new XDocument(new XElement("Catalog"));

        foreach (Category category in categories)
        {
            List<Product> categoryProducts = products.FindAll(p => p.CategoryId == category.Id);

            XElement categoryElement = new XElement("Category",
                new XElement("Id", category.Id),
                new XElement("Name", category.Name),
                new XElement("Description", category.Description)
            );

            foreach (Product product in categoryProducts)
            {
                XElement productElement = new XElement("Product",
                    new XElement("Id", product.Id),
                    new XElement("Name", product.Name),
                    new XElement("Price", product.Price)
                );

                categoryElement.Add(productElement);
            }

            xml.Root.Add(categoryElement);
        }

        return xml;
    }

    static void SaveCatalogAsXML(XDocument xml, string filePath)
    {
        xml.Save(filePath);
    }
}

class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string CategoryId { get; set; }
}

class CatalogCategory
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Product> Products { get; set; }
}

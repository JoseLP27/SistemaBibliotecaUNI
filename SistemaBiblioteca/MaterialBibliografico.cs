//Necesario para el XML Serializer
using System.Xml.Serialization;

[XmlInclude(typeof(Libro))]
[XmlInclude(typeof(Revista))]
[XmlInclude(typeof(Monografia))]

//Necesario para el JSON serializer
[System.Text.Json.Serialization.JsonDerivedType(typeof(Libro), "Libro")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Revista), "Revista")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Monografia), "Monografia")]

public abstract class MaterialBibliografico
{
    private string? _codigoISBN;
    private string? _titulo;
    private string? _autor;
    private int _anioPublicacion;
    private int _stock;

    public MaterialBibliografico(string codigoisbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        CodigoISBN = codigoisbn;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        Stock = stock;
        Estado = EstadoMaterial.Disponible;
    }

    protected MaterialBibliografico() { }

    public string? CodigoISBN
    {
        get => _codigoISBN;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 13)
                throw new ArgumentException("[ERROR]: El ISBN es obligatorio.");
            _codigoISBN = value;
        }
    }

    public string? Titulo
    {
        get => _titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El título no puede estar vacío.");
            _titulo = value;
        }
    }

    public string? Autor
    {
        get => _autor;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El autor no puede estar vacío.");
            _autor = value;
        }
    }

    public int AnioPublicacion
    {
        get => _anioPublicacion;
        set
        {
            if (value < 1800 || value > 2026)
                throw new ArgumentException("[ERROR]: Año de publicación inválido.");
            _anioPublicacion = value;
        }
    }

    public EstadoMaterial Estado { get; set; }

    public int Stock
    {
        get => _stock;
        set
        {
            if (value < 0)
                throw new ArgumentException("[ERROR]: El stock no puede ser negativo.");
            _stock = value;
        }
    }

    public abstract int ObtenerDiasMaximos();
    public abstract int ObtenerTarifaMulta();

    public bool EstaDisponible()
    {
        return Estado == EstadoMaterial.Disponible && Stock > 0;
    }

    public override string ToString()
    {
        return $"ISBN: {CodigoISBN} - Titulo: {Titulo} - Autor: {Autor} - Anio de publicacion: {AnioPublicacion} - Estado: {Estado} - Stock: {Stock}";
    }

}
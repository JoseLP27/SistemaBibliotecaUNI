public abstract class Libro
{
    private string? _codigoISBN;
    private string? _titulo;
    private string? _autor;
    private int _anioPublicacion;
    private bool _disponible;
    private int _stock;

    public Libro(string codigo, string titulo, string autor, int anioPublicacion)
    {
        CodigoISBN = codigo;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        Disponible = true;
    }

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
            if (value < 1900 || value > 2026)
                throw new ArgumentException("[ERROR]: Año de publicación inválido.");
            _anioPublicacion = value;
        }
    }

    public bool Disponible
    {
        get => _disponible;
        set => _disponible = value;
    }

    public int Stock
    {
        get => _stock;
        set
        {
            if (value < 0)
                throw new ArgumentException("[ERROR]: El stock no puede ser negativo.");
        }
    }

    public abstract int ObtenerDiasMaximos();
    public abstract int ObtenerTarifaMulta();
    public override string ToString()
    {
        return $"ISBN: {CodigoISBN} - Titulo: {Titulo} - Autor: {Autor} - Anio de publicacion: {AnioPublicacion} - Estado: {Disponible} - Stock: {Stock}";
    }

}
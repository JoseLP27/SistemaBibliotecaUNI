public class Revista : Libro
{
    private int _numeroedicion;
    private int _volumen;
    private string? _periodicidad;
    private Dictionary<string, Revista> _revistas = new();

    public Revista(string codigo, string titulo, string autor, int numeroedicion, int volumen,
        string? periodicidad, int anioPublicacion) : base(codigo, titulo, autor, anioPublicacion)
    {
        NumeroEdicion  = numeroedicion;
        Volumen = volumen;
        Periodicidad = periodicidad;
    }

    public int NumeroEdicion
    {
        get => _numeroedicion;
        set
        {
            if (value <= 0)
                throw new ArgumentException("El número de edición debe ser mayor a cero.");

            _numeroedicion = value;
        }
    }
    public int Volumen
    {
        get => _volumen;
        set
        {
            if (value <= 0)
                throw new ArgumentException("El número de volumen debe ser mayor a cero.");

            _volumen = value;
        }
    }

    public string? Periodicidad
    {
        get => _periodicidad;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
                throw new ArgumentException("[ERROR]: La periodicidad no puede estar vacia.");
            _periodicidad = value;
        }
    }

    public override bool EstaDisponible()
    {
        throw new NotImplementedException();
    }

    public override int ObtenerDiasMaximos()
    {
        return 7;
    }

    public override int ObtenerTarifaMulta()
    {
        return 3;
    }
}
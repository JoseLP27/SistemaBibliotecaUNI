public class Libro : MaterialBibliografico
{
    private string? _estante;
    private string? _signaturaTopografica;
    private string? _estadoConservacion;

    public Libro(string codigoisbn, string titulo, string autor, int anioPublicacion, string estante, string signatura, string estadoconv, int stock)
        : base(codigoisbn, titulo, autor, anioPublicacion, stock)
    {
        Estante = estante;
        SignaturaTopografica = signatura;
        EstadoConservacion = estadoconv;
    }

    public string? Estante
    {
        get => _estante;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: La ubicación del estante es obligatoria.");
            _estante = value;
        }
    }

    public string? SignaturaTopografica
    {
        get => _signaturaTopografica;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: La signatura topográfica es obligatoria.");
            _signaturaTopografica = value;
        }
    }

    public string? EstadoConservacion
    {
        get => _estadoConservacion;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El estado de conservación no puede estar vacío.");
            _estadoConservacion = value;
        }
    }

    public override int ObtenerDiasMaximos()
    {
        return 7;
    }

    public override int ObtenerTarifaMulta()
    {
        return 5;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Estante: {Estante} | Lomo: {SignaturaTopografica} | Estado: {EstadoConservacion}";
    }
}
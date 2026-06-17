public class Monografia : MaterialBibliografico
{
    private string? _tutores;
    private string? _facultad;
    private string? _carrera;

    public Monografia(string codigo, string titulo, string autor, int anioPublicacion,
                      string tutores, string facultad, string carrera, int stock)
        : base(codigo, titulo, autor, anioPublicacion, stock)
    {
        Tutores = tutores;
        Facultad = facultad;
        Carrera = carrera;
    }

    public Monografia() { }

    public string? Tutores
    {
        get => _tutores;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El nombre del tutor es obligatorio.");
            _tutores = value;
        }
    }

    public string? Facultad
    {
        get => _facultad;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: La facultad es obligatoria.");
            _facultad = value;
        }
    }

    public string? Carrera
    {
        get => _carrera;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: La carrera es obligatoria.");
            _carrera = value;
        }
    }

    public override int ObtenerDiasMaximos()
    {
        return 1;
    }

    public override int ObtenerTarifaMulta()
    {
        return 50;
    }

    public override string ToString()
    {
        return $"{base.ToString()} | Facultad: {Facultad} | Carrera: {Carrera} | Tutor: {Tutores}";
    }
}
using System.Text.RegularExpressions;
public class Estudiante : Usuario
{
    private string? _carnet;
    private string? _carrera;

    private readonly string formatocarnet = @"^\d{4}-\d{4}U$";

    public Estudiante(string? nombre, string? apellido, string? carnet, string? carrera
        , string? email, bool puedeprestar) : base(nombre, apellido, email, puedeprestar)
    {
        Carnet = carnet;
        Carrera = carrera;
    }

    public string? Carnet
    {
        get => _carnet;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El carnet no puede estar vacio.");
            if (!Regex.IsMatch(value, formatocarnet))
                throw new ArgumentException("[ERROR]: El formato del carnet ingresado es invalido.");
            _carnet = value;
        }
    }

    public string? Carrera
    {
        get => _carrera;
        set
        {
            _carrera = ValidarTexto(value, "Carrera");
        }
    }

    public int AnioIngreso => _carnet != null ? int.Parse(_carnet.Substring(0, 4)) : 0;

    public override int CalcularLimitePrestamos()
    {
        return 5;
    }

    public override double CalcularMulta(int DiasRetraso)
    {
        int total = 0;
        foreach (var libro in _librosPrestados.Values)
        {
            total += libro.ObtenerTarifaMulta() * DiasRetraso;
        }
        return total;
    }

}
using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlInclude(typeof(Estudiante))]
[XmlInclude(typeof(Docente))]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Estudiante), "Estudiante")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Docente), "Docente")]

public abstract class Usuario
{
    private string? _nombre;
    private string? _apellido;
    private string? _email;
    private int _contadorPrestamos;

    [XmlIgnore]
    protected Dictionary<string, MaterialBibliografico> _librosPrestados = new();

    [XmlIgnore]
    private readonly string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public string? Nombre
    {
        get => _nombre;
        set => _nombre = ValidarTexto(value, "nombre");
    }

    public string? Apellido
    {
        get => _apellido;
        set => _apellido = ValidarTexto(value, "apellido");
    }

    [XmlIgnore]
    public string NombreCompleto => $"{Nombre} {Apellido}";

    public string? Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("[ERROR]: La entrada no puede ser nula");
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: La entrada no puede estar vacia.");
            if (!Regex.IsMatch(value, patronEmail))
                throw new ArgumentException("[ERROR]: El email ingresado no cumple con el formato de email.");
            _email = value;
        }
    }

    public bool PuedePrestar { get; set; }

    public int ContadorPrestamos
    {
        get => _contadorPrestamos;
        set => _contadorPrestamos = value;
    }

    public void IncrementarContador() => _contadorPrestamos++;

    public string ValidarTexto(string? value, string campo)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            throw new ArgumentException($"{campo} invalido.");
        return value.Trim();
    }

    public Usuario(string? nombre, string? apellido, string? email, bool puedeprestar)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        PuedePrestar = puedeprestar;
        _contadorPrestamos = 0;
    }

    protected Usuario() { }

    public abstract int CalcularLimitePrestamos();
    public abstract double CalcularMulta(int DiasRetraso);

    public override string ToString()
    {
        return $"Nombre: {NombreCompleto} | Email: {Email}";
    }

}
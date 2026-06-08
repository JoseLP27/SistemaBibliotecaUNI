using System.Text.RegularExpressions;
public abstract class Usuario
{
    private int _id;
    private string? _nombre;
    private string? _apellido;
    private string? _email;
    private bool _estadoPrestamo;

    public int Id
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("[ERROR]: El ID no puede ser negativo.");
            _id = value;
        }
    }

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

    public string NombreCompleto => $"{Nombre} {Apellido}";

    string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
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

    public bool EstadoPrestamo { get; set;}

    private string ValidarTexto(string? value, string campo)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{campo} invalido.");
        return value.Trim();
    }

    public abstract void HacerPrestamo();
    public abstract void Devolver();
    public abstract void CalcularLimitePrestamos();
    public abstract int CalcularMulta(int DiasRetraso);

    public override string ToString()
    {
        return $"ID: {Id} | Nombre: {NombreCompleto} | Email: {Email}";
    }
    public virtual void MostrarMiInfo()
    {
        Console.WriteLine(ToString());
    }

}
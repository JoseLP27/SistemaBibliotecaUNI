using System.Text.RegularExpressions;
public abstract class Usuario
{
    private string? _nombre;
    private string? _apellido;
    private string? _email;
    private bool _estadoPrestamo;
    private int _contadorPrestamos;
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

    public int ContadorPrestamos => _contadorPrestamos;

    public string ValidarTexto(string? value, string campo)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            throw new ArgumentException($"{campo} invalido.");
        return value.Trim();
    }

    public Usuario (string? nombre, string? apellido, string? email, bool estadoPrestamo)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        EstadoPrestamo = estadoPrestamo;
        _contadorPrestamos = 0;
    }

    public abstract void HacerPrestamo();
    public abstract void Devolver();
    public abstract void CalcularLimitePrestamos();
    public abstract int CalcularMulta(int DiasRetraso);

    public override string ToString()
    {
        return $"Nombre: {NombreCompleto} | Email: {Email}";
    }
    public virtual void MostrarMiInfo()
    {
        Console.WriteLine(ToString());
    }

}
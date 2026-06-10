public class Maestro : Usuario
{
    private string? _codigoDocente;
    private string? _departamento;

    public Maestro(string? nombre, string? apellido, string? email, bool estadoPrestamo, string codigodocente, string departameto)
        : base(nombre, apellido, email, estadoPrestamo)
    {
        Departamento = departameto;
        CodigoDocente = codigodocente;
    }

    public string? CodigoDocente
    {
        get => _codigoDocente;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 4)
                throw new ArgumentException("[ERROR]: El codigo de docente debe tener exactamente 4 caracteres.");
            _codigoDocente = value;
        }
    }

    public string? Departamento
    {
        get => _departamento;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("[ERROR]: El departamento no puede ser vacio.");
            if (value.Length > 20)
                throw new ArgumentException("[ERROR]: Demasiados caracteres ingresados.");
            _departamento = value;
        }
    }

    public override void CalcularLimitePrestamos()
    {
        throw new NotImplementedException();
    }

    public override int CalcularMulta(int DiasRetraso)
    {
        throw new NotImplementedException();
    }

    public override void Devolver()
    {
        throw new NotImplementedException();
    }

    public override void HacerPrestamo()
    {
        throw new NotImplementedException();
    }

}
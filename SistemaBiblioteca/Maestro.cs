using System.Text.RegularExpressions;
public class Maestro : Usuario
{
    private string? _codigoDocente;
    private string? _departamento;

    public string CodigoDocente
    {
        get => _codigoDocente;
        set
        {
            if (value)
                throw new ArgumentException("Error. Formato de identificator invalido.");
            _codigoDocente = value;
        }
    }

    string formatodept = "";
    public string? Departamento
    {
        get => _departamento;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
                throw new ArgumentException("[ERROR]: El departamento no puede ser vacio o nulo.");
            if ()
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
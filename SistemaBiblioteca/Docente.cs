using System.Xml.Serialization;

public class Docente : Usuario
{
    private string? _codigoDocente;
    private string? _departamento;

    public Docente(string? nombre, string? apellido, string? email, string? codigodocente,
        string? departamento, bool puedeprestar) : base(nombre, apellido, email, puedeprestar)
    {
        CodigoDocente = codigodocente;
        Departamento = departamento;
    }

    public Docente() { }

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

    public override int CalcularLimitePrestamos() => 10;

    public override decimal CalcularMulta(int diasRetraso, MaterialBibliografico material)
    {
        decimal total = diasRetraso * material.ObtenerTarifaMulta();
        if (diasRetraso <= 3)
            total *= 0.70m;
        return total;
    }

}
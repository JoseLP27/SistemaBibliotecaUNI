public class RegistroPrestamo
{
    public Usuario Usuario { get; set; }
    public MaterialBibliografico Material { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public bool Devuelto { get; set; }

    public RegistroPrestamo(Usuario usuario, MaterialBibliografico material)
    {
        Usuario = usuario;
        Material = material;
        FechaPrestamo = DateTime.Now;
        FechaDevolucion = DateTime.Now.AddDays(material.ObtenerDiasMaximos());
        Devuelto = false;
    }

    public RegistroPrestamo()
    {
        // requerido por xml serializer
    }

    public int CalcularDiasRetraso()
    {
        if (Devuelto) return 0;
        int dias = (DateTime.Now - FechaDevolucion).Days;
        return dias > 0 ? dias : 0;
    }

    public override string ToString()
    {
        return $"Usuario: {Usuario.NombreCompleto} | Material: {Material.Titulo} | Prestado: {FechaPrestamo:dd/MM/yyyy} | Vence: {FechaDevolucion:dd/MM/yyyy} | Devuelto: {Devuelto}";
    }
}
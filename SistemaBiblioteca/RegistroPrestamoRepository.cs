public class RegistroPrestamoRepository : IRepository<RegistroPrestamo>
{
    private readonly ISerializer<RegistroPrestamo> _serializer;
    private readonly string _archivo;

    public RegistroPrestamoRepository(
        ISerializer<RegistroPrestamo> serializer,
        string archivo)
    {
        _serializer = serializer;
        _archivo = archivo;

        if (!File.Exists(_archivo))
            _serializer.Guardar(new List<RegistroPrestamo>(), _archivo);
    }

    public void Agregar(RegistroPrestamo entidad)
    {
        List<RegistroPrestamo> prestamos = _serializer.Cargar(_archivo);
        prestamos.Add(entidad);
        _serializer.Guardar(prestamos, _archivo);
    }

    public bool Eliminar(Func<RegistroPrestamo, bool> criterio)
    {
        List<RegistroPrestamo> prestamos = _serializer.Cargar(_archivo);
        RegistroPrestamo prestamo = prestamos.FirstOrDefault(criterio)
            ?? throw new ArgumentException("[ERROR]: Préstamo no encontrado.");
        prestamos.Remove(prestamo);
        _serializer.Guardar(prestamos, _archivo);
        return true;
    }

    public RegistroPrestamo? Buscar(Func<RegistroPrestamo, bool> criterio)
    {
        return _serializer.Cargar(_archivo).FirstOrDefault(criterio);
    }

    public List<RegistroPrestamo> ObtenerTodos()
    {
        return _serializer.Cargar(_archivo);
    }

    public bool Actualizar(Func<RegistroPrestamo, bool> criterio, RegistroPrestamo nuevaEntidad)
    {
        List<RegistroPrestamo> prestamos = _serializer.Cargar(_archivo);
        int indice = prestamos.FindIndex(e => criterio(e));
        if (indice < 0) return false;
        prestamos[indice] = nuevaEntidad;
        _serializer.Guardar(prestamos, _archivo);
        return true;
    }
}
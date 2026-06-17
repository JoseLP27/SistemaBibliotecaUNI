public class MaterialRepository : IRepository<MaterialBibliografico>
{
    private readonly ISerializer<MaterialBibliografico> _serializer;
    private readonly string _archivo;

    public MaterialRepository(
        ISerializer<MaterialBibliografico> serializer,
        string archivo)
    {
        _serializer = serializer;
        _archivo = archivo;

        if (!File.Exists(_archivo))
            _serializer.Guardar(new List<MaterialBibliografico>(), _archivo);
    }

    public void Agregar(MaterialBibliografico entidad)
    {
        List<MaterialBibliografico> materiales = _serializer.Cargar(_archivo);
        materiales.Add(entidad);
        _serializer.Guardar(materiales, _archivo);
    }

    public bool Eliminar(Func<MaterialBibliografico, bool> criterio)
    {
        List<MaterialBibliografico> materiales = _serializer.Cargar(_archivo);

        MaterialBibliografico material = materiales.FirstOrDefault(criterio)
            ?? throw new ArgumentException("[ERROR]: Material no encontrado.");

        materiales.Remove(material);
        _serializer.Guardar(materiales, _archivo);
        return true;
    }

    public MaterialBibliografico? Buscar(Func<MaterialBibliografico, bool> criterio)
    {
        return _serializer.Cargar(_archivo).FirstOrDefault(criterio);
    }

    public List<MaterialBibliografico> ObtenerTodos()
    {
        return _serializer.Cargar(_archivo);
    }

    public bool Actualizar(Func<MaterialBibliografico, bool> criterio, MaterialBibliografico nuevaEntidad)
    {
        List<MaterialBibliografico> materiales = _serializer.Cargar(_archivo);
        int indice = materiales.FindIndex(e => criterio(e));

        if (indice < 0) return false;

        materiales[indice] = nuevaEntidad;
        _serializer.Guardar(materiales, _archivo);
        return true;
    }
}
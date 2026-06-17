public class UsuarioRepository : IRepository<Usuario>
{
    private readonly ISerializer<Usuario> _serializer;
    private readonly string _archivo;

    public UsuarioRepository(
        ISerializer<Usuario> serializer,
        string archivo)
    {
        _serializer = serializer;
        _archivo = archivo;

        if (!File.Exists(_archivo))
            _serializer.Guardar(new List<Usuario>(), _archivo);
    }

    public void Agregar(Usuario entidad)
    {
        List<Usuario> usuarios = _serializer.Cargar(_archivo);
        usuarios.Add(entidad);
        _serializer.Guardar(usuarios, _archivo);
    }

    public bool Eliminar(Func<Usuario, bool> criterio)
    {
        List<Usuario> usuarios = _serializer.Cargar(_archivo);
        Usuario usuario = usuarios.FirstOrDefault(criterio)
            ?? throw new ArgumentException("[ERROR]: Usuario no encontrado.");
        usuarios.Remove(usuario);
        _serializer.Guardar(usuarios, _archivo);
        return true;
    }

    public Usuario? Buscar(Func<Usuario, bool> criterio)
    {
        return _serializer.Cargar(_archivo).FirstOrDefault(criterio);
    }

    public List<Usuario> ObtenerTodos()
    {
        return _serializer.Cargar(_archivo);
    }

    public bool Actualizar(Func<Usuario, bool> criterio, Usuario nuevaEntidad)
    {
        List<Usuario> usuarios = _serializer.Cargar(_archivo);
        int indice = usuarios.FindIndex(e => criterio(e));
        if (indice < 0) return false;
        usuarios[indice] = nuevaEntidad;
        _serializer.Guardar(usuarios, _archivo);
        return true;
    }
}
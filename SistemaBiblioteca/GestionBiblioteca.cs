public class GestionBiblioteca
{
    private readonly MaterialRepository _materialRepo;
    private readonly UsuarioRepository _usuarioRepo;
    private readonly RegistroPrestamoRepository _prestamoRepo;

    public GestionBiblioteca(
        MaterialRepository materialRepo,
        UsuarioRepository usuarioRepo,
        RegistroPrestamoRepository prestamoRepo)
    {
        _materialRepo = materialRepo;
        _usuarioRepo = usuarioRepo;
        _prestamoRepo = prestamoRepo;
    }

    // ─── MATERIALES ───────────────────────────────────────
    public void RegistrarLibro(string isbn, string titulo, string autor,
        int anio, string estante, string signatura, string estadoConv, int stock)
    {
        if (_materialRepo.Buscar(m => m.CodigoISBN == isbn) != null)
            throw new InvalidOperationException("[ERROR]: Ya existe un material con ese ISBN.");
        Libro libro = new(isbn, titulo, autor, anio, estante, signatura, estadoConv, stock);
        _materialRepo.Agregar(libro);
    }

    public void RegistrarRevista(string isbn, string titulo, string autor,
        int edicion, int volumen, string periodicidad, int anio, int stock)
    {
        if (_materialRepo.Buscar(m => m.CodigoISBN == isbn) != null)
            throw new InvalidOperationException("[ERROR]: Ya existe un material con ese ISBN.");
        Revista revista = new(isbn, titulo, autor, edicion, volumen, periodicidad, anio, stock);
        _materialRepo.Agregar(revista);
    }

    public void RegistrarMonografia(string isbn, string titulo, string autor,
        int anio, string tutores, string facultad, string carrera, int stock)
    {
        if (_materialRepo.Buscar(m => m.CodigoISBN == isbn) != null)
            throw new InvalidOperationException("[ERROR]: Ya existe un material con ese ISBN.");
        Monografia mono = new(isbn, titulo, autor, anio, tutores, facultad, carrera, stock);
        _materialRepo.Agregar(mono);
    }

    public void EliminarMaterial(string isbn)
    {
        _materialRepo.Eliminar(m => m.CodigoISBN == isbn);
    }

    public List<MaterialBibliografico> ObtenerMateriales()
    {
        return _materialRepo.ObtenerTodos();
    }

    public MaterialBibliografico BuscarMaterial(string isbn)
    {
        return _materialRepo.Buscar(m => m.CodigoISBN == isbn)
            ?? throw new ArgumentException("[ERROR]: Material no encontrado.");
    }

    // ─── USUARIOS ─────────────────────────────────────────
    public void RegistrarEstudiante(string nombre, string apellido,
        string carnet, string carrera, string email)
    {
        if (_usuarioRepo.Buscar(u => u.Email == email) != null)
            throw new InvalidOperationException("[ERROR]: Ya existe un usuario con ese email.");
        Estudiante estudiante = new(nombre, apellido, carnet, carrera, email, true);
        _usuarioRepo.Agregar(estudiante);
    }

    public void RegistrarDocente(string nombre, string apellido,
        string email, string codigoDocente, string departamento)
    {
        if (_usuarioRepo.Buscar(u => u.Email == email) != null)
            throw new InvalidOperationException("[ERROR]: Ya existe un usuario con ese email.");
        Docente docente = new(nombre, apellido, email, codigoDocente, departamento, true);
        _usuarioRepo.Agregar(docente);
    }

    public void EliminarUsuario(string email)
    {
        _usuarioRepo.Eliminar(u => u.Email == email);
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _usuarioRepo.ObtenerTodos();
    }

    public Usuario BuscarUsuario(string email)
    {
        return _usuarioRepo.Buscar(u => u.Email == email)
            ?? throw new ArgumentException("[ERROR]: Usuario no encontrado.");
    }

    // ─── PRÉSTAMOS ────────────────────────────────────────
    public void PrestarMaterial(string emailUsuario, string isbn)
    {
        Usuario usuario = _usuarioRepo.Buscar(u => u.Email == emailUsuario)
            ?? throw new ArgumentException("[ERROR]: Usuario no encontrado.");

        MaterialBibliografico material = _materialRepo.Buscar(m => m.CodigoISBN == isbn)
            ?? throw new ArgumentException("[ERROR]: Material no encontrado.");

        if (!material.EstaDisponible())
            throw new InvalidOperationException("[ERROR]: El material no está disponible.");

        if (!usuario.PuedePrestar)
            throw new InvalidOperationException("[ERROR]: El usuario no puede realizar préstamos.");

        List<RegistroPrestamo> prestamosActivos = _prestamoRepo
            .ObtenerTodos()
            .Where(p => p.Usuario.Email == emailUsuario && !p.Devuelto)
            .ToList();

        if (prestamosActivos.Count >= usuario.CalcularLimitePrestamos())
            throw new InvalidOperationException("[ERROR]: El usuario alcanzó su límite de préstamos.");

        material.Stock--;
        if (material.Stock == 0)
            material.Estado = EstadoMaterial.Agotado;

        _materialRepo.Actualizar(m => m.CodigoISBN == isbn, material);

        RegistroPrestamo prestamo = new(usuario, material);
        _prestamoRepo.Agregar(prestamo);

        usuario.IncrementarContador();
        _usuarioRepo.Actualizar(u => u.Email == emailUsuario, usuario);
    }

    public double DevolverMaterial(string emailUsuario, string isbn)
    {
        RegistroPrestamo prestamo = _prestamoRepo
            .Buscar(p => p.Usuario.Email == emailUsuario
                      && p.Material.CodigoISBN == isbn
                      && !p.Devuelto)
            ?? throw new ArgumentException("[ERROR]: Préstamo activo no encontrado.");

        MaterialBibliografico material = _materialRepo.Buscar(m => m.CodigoISBN == isbn)!;
        material.Stock++;
        material.Estado = EstadoMaterial.Disponible;
        _materialRepo.Actualizar(m => m.CodigoISBN == isbn, material);

        prestamo.Devuelto = true;
        _prestamoRepo.Actualizar(
            p => p.Usuario.Email == emailUsuario
              && p.Material.CodigoISBN == isbn
              && !p.Devuelto,
            prestamo);

        Usuario usuario = _usuarioRepo.Buscar(u => u.Email == emailUsuario)!;
        int diasRetraso = prestamo.CalcularDiasRetraso();
        return diasRetraso > 0 ? usuario.CalcularMulta(diasRetraso) : 0;
    }

    public List<RegistroPrestamo> ObtenerPrestamosActivos()
    {
        return _prestamoRepo.ObtenerTodos()
            .Where(p => !p.Devuelto)
            .ToList();
    }

    public List<RegistroPrestamo> ObtenerHistorial()
    {
        return _prestamoRepo.ObtenerTodos();
    }

    public List<RegistroPrestamo> ObtenerPrestamosPorUsuario(string email)
    {
        return _prestamoRepo.ObtenerTodos()
            .Where(p => p.Usuario.Email == email && !p.Devuelto)
            .ToList();
    }

    public List<Usuario> ObtenerUsuariosConMaterial(string isbn)
    {
        return _prestamoRepo.ObtenerTodos()
            .Where(p => p.Material.CodigoISBN == isbn && !p.Devuelto)
            .Select(p => p.Usuario)
            .ToList();
    }

    public double ConsultarMulta(string email)
    {
        Usuario usuario = _usuarioRepo.Buscar(u => u.Email == email)
            ?? throw new ArgumentException("[ERROR]: Usuario no encontrado.");

        List<RegistroPrestamo> prestamosActivos = _prestamoRepo
            .ObtenerTodos()
            .Where(p => p.Usuario.Email == email && !p.Devuelto)
            .ToList();

        double total = 0;
        foreach (RegistroPrestamo prestamo in prestamosActivos)
        {
            int diasRetraso = prestamo.CalcularDiasRetraso();
            if (diasRetraso > 0)
                total += usuario.CalcularMulta(diasRetraso);
        }
        return total;

    }
}
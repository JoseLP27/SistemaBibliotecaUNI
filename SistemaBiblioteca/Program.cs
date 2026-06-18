Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("  ╔═════════════════════════════════════╗");
Console.WriteLine("  ║      SISTEMA DE BIBLIOTECA          ║");
Console.WriteLine("  ╠═════════════════════════════════════╣");
Console.WriteLine("  ║  1. JSON                            ║");
Console.WriteLine("  ║  2. XML                             ║");
Console.WriteLine("  ╚═════════════════════════════════════╝");
Console.ResetColor();
Console.Write("\n  Formato de almacenamiento: ");
string formato = Console.ReadLine()!.Trim();

ISerializer<MaterialBibliografico> materialSerializer;
ISerializer<Usuario> usuarioSerializer;
ISerializer<RegistroPrestamo> prestamoSerializer;
string TipoArchivo;

if (formato == "2")
{
    materialSerializer = new XmlFileSerializer<MaterialBibliografico>();
    usuarioSerializer = new XmlFileSerializer<Usuario>();
    prestamoSerializer = new XmlFileSerializer<RegistroPrestamo>();
    TipoArchivo = "xml";
}
else
{
    materialSerializer = new JsonFileSerializer<MaterialBibliografico>();
    usuarioSerializer = new JsonFileSerializer<Usuario>();
    prestamoSerializer = new JsonFileSerializer<RegistroPrestamo>();
    TipoArchivo = "json";
}

MaterialRepository materialRepo = new(materialSerializer, $"Registros/materiales.{TipoArchivo}");
UsuarioRepository usuarioRepo = new(usuarioSerializer, $"Registros/usuarios.{TipoArchivo}");
RegistroPrestamoRepository prestamoRepo = new(prestamoSerializer, $"Registros/prestamos.{TipoArchivo}");

GestionBiblioteca gestion = new(materialRepo, usuarioRepo, prestamoRepo);

void Exito(string mensaje)
{
    Console.ForegroundColor = ConsoleColor.Green;
    int ancho = mensaje.Length + 2;
    Console.WriteLine("\n  ╔" + new string('═', ancho) + "╗");
    Console.WriteLine($"  ║ {mensaje} ║");
    Console.WriteLine("  ╚" + new string('═', ancho) + "╝");
    Console.ResetColor();
}

void Error(string mensaje)
{
    Console.ForegroundColor = ConsoleColor.Red;
    int ancho = mensaje.Length + 2;
    Console.WriteLine("\n  ╔" + new string('═', ancho) + "╗");
    Console.WriteLine($"  ║ {mensaje} ║");
    Console.WriteLine("  ╚" + new string('═', ancho) + "╝");
    Console.ResetColor();
}

void Titulo(string texto)
{
    Console.ForegroundColor = ConsoleColor.Green;
    int ancho = texto.Length + 2;
    Console.WriteLine($"\n  ╔" + new string('═', ancho) + "╗");
    Console.WriteLine($"  ║ {texto} ║");
    Console.WriteLine($"  ╚" + new string('═', ancho) + "╝");
    Console.ResetColor();
}

string LeerTexto(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(valor))
        {
            Error("Este campo no puede estar vacío.");
            continue;
        }
        return valor.Trim();
    }
}

string LeerSoloLetras(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(valor) || !valor.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            Error("Este campo solo puede contener letras y espacios.");
            continue;
        }
        return valor.Trim();
    }
}

int LeerEntero(string prompt, int min = 1, int max = int.MaxValue)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int valor) && valor >= min && valor <= max)
            return valor;

        Error($"Ingrese un número válido entre {min} y {max}.");
    }
}

string LeerISBN(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!.Trim();
        if (string.IsNullOrWhiteSpace(valor))
        {
            Error("El ISBN no puede estar vacío.");
            continue;
        }
        if (valor.Length != 13)
        {
            Error($"[ERROR]: El ISBN debe tener exactamente 13 caracteres. Ingresaste {valor.Length}.");
            continue;
        }
        return valor;
    }
}

string LeerCarnet(string prompt)
{
    var regex = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{4}U$");
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!.Trim();
        if (string.IsNullOrWhiteSpace(valor))
        {
            Error("El carnet no puede estar vacío.");
            continue;
        }
        if (!regex.IsMatch(valor))
        {
            Error("[ERROR]: Formato de carnet inválido. Ejemplo correcto: 2024-0001U");
            continue;
        }
        return valor;
    }
}

string LeerEmail(string prompt)
{
    var regex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!.Trim();
        if (string.IsNullOrWhiteSpace(valor))
        {
            Error("El email no puede estar vacío.");
            continue;
        }
        if (!regex.IsMatch(valor))
        {
            Error("[ERROR]: El email ingresado no cumple con el formato de email.");
            continue;
        }
        return valor;
    }
}

string LeerCodigoDocente(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string valor = Console.ReadLine()!.Trim();
        if (string.IsNullOrWhiteSpace(valor))
        {
            Error("El código de docente no puede estar vacío.");
            continue;
        }
        if (valor.Length != 4)
        {
            Error($"[ERROR]: El código de docente debe tener exactamente 4 caracteres. Ingresaste {valor.Length}.");
            continue;
        }
        return valor;
    }
}

void MostrarUsuario(Usuario u)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    if (u is Estudiante est)
    {
        Console.WriteLine($"  Estudiante : {est.NombreCompleto}");
        Console.WriteLine($"  Carnet     : {est.Carnet}");
        Console.WriteLine($"  Carrera    : {est.Carrera}");
    }
    else if (u is Docente doc)
    {
        Console.WriteLine($"  Docente    : {doc.NombreCompleto}");
        Console.WriteLine($"  Código     : {doc.CodigoDocente}");
        Console.WriteLine($"  Depto      : {doc.Departamento}");
    }
    Console.WriteLine($"  Email      : {u.Email}");
    Console.WriteLine($"  Préstamos  : {u.ContadorPrestamos}");
    Console.WriteLine($"  Puede prest: {(u.PuedePrestar ? "Sí" : "No")}");
    Console.ResetColor();
    Console.WriteLine("  ─────────────────────────────────────");
}

void MostrarMaterial(MaterialBibliografico m)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    if (m is Libro l)
    {
        Console.WriteLine($"  Tipo       : Libro");
        Console.WriteLine($"  Título     : {l.Titulo}");
        Console.WriteLine($"  Autor      : {l.Autor}");
        Console.WriteLine($"  ISBN       : {l.CodigoISBN}");
        Console.WriteLine($"  Estante    : {l.Estante}");
        Console.WriteLine($"  Conserv.   : {l.EstadoConservacion}");
    }
    else if (m is Revista r)
    {
        Console.WriteLine($"  Tipo       : Revista");
        Console.WriteLine($"  Título     : {r.Titulo}");
        Console.WriteLine($"  Autor      : {r.Autor}");
        Console.WriteLine($"  ISBN       : {r.CodigoISBN}");
        Console.WriteLine($"  Edición    : {r.NumeroEdicion}");
        Console.WriteLine($"  Volumen    : {r.Volumen}");
    }
    else if (m is Monografia mono)
    {
        Console.WriteLine($"  Tipo       : Monografía");
        Console.WriteLine($"  Título     : {mono.Titulo}");
        Console.WriteLine($"  Autor      : {mono.Autor}");
        Console.WriteLine($"  ISBN       : {mono.CodigoISBN}");
        Console.WriteLine($"  Tutor      : {mono.Tutores}");
        Console.WriteLine($"  Facultad   : {mono.Facultad}");
    }
    Console.WriteLine($"  Estado     : {m.Estado}");
    Console.WriteLine($"  Stock      : {m.Stock}");
    Console.WriteLine($"  Días máx.  : {m.ObtenerDiasMaximos()}");
    Console.ResetColor();
    Console.WriteLine("  ─────────────────────────────────────");
}

void MostrarPrestamo(RegistroPrestamo p)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"  Usuario    : {p.Usuario.NombreCompleto}");
    Console.WriteLine($"  Material   : {p.Material.Titulo}");
    Console.WriteLine($"  ISBN       : {p.Material.CodigoISBN}");
    Console.WriteLine($"  Prestado   : {p.FechaPrestamo:dd/MM/yyyy}");
    Console.WriteLine($"  Vence      : {p.FechaDevolucion:dd/MM/yyyy}");
    Console.WriteLine($"  Devuelto   : {(p.Devuelto ? "Sí" : "No")}");
    Console.ResetColor();
    Console.WriteLine("  ─────────────────────────────────────");
}

bool salir = false;

while (!salir)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ╔═════════════════════════════════════╗");
    Console.WriteLine("  ║      SISTEMA DE BIBLIOTECA          ║");
    Console.WriteLine("  ╠═════════════════════════════════════╣");
    Console.WriteLine("  ║  1.  Registrar material             ║");
    Console.WriteLine("  ║  2.  Registrar usuario              ║");
    Console.WriteLine("  ║  3.  Realizar préstamo              ║");
    Console.WriteLine("  ║  4.  Devolver material              ║");
    Console.WriteLine("  ║  5.  Ver catálogo                   ║");
    Console.WriteLine("  ║  6.  Ver usuarios                   ║");
    Console.WriteLine("  ║  7.  Ver préstamos activos          ║");
    Console.WriteLine("  ║  8.  Préstamos por usuario          ║");
    Console.WriteLine("  ║  9.  Quién tiene un material        ║");
    Console.WriteLine("  ║  10. Consultar multa                ║");
    Console.WriteLine("  ║  11. Eliminar material              ║");
    Console.WriteLine("  ║  12. Eliminar usuario               ║");
    Console.WriteLine("  ║  0.  Salir                          ║");
    Console.WriteLine("  ╚═════════════════════════════════════╝");
    Console.ResetColor();
    Console.Write("\n  Opción: ");
    string opcion = Console.ReadLine()!;

    try
    {
        switch (opcion)
        {
            case "1":
                Console.Clear();
                Titulo("REGISTRAR MATERIAL");
                Console.WriteLine("  1. Libro  2. Revista  3. Monografía");
                string tipoMat = LeerTexto("\n  Tipo: ");

                int cantMat = LeerEntero("  ¿Cuántos materiales registrará? ", 1, 100);

                for (int i = 0; i < cantMat; i++)
                {
                    Console.WriteLine($"\n  ── Material {i + 1} de {cantMat} ──────────────");

                    string isbn = LeerISBN("  ISBN (13 dígitos): ");
                    string titulo = LeerTexto("  Título: ");
                    string autor = LeerSoloLetras("  Autor: ");
                    int anio = LeerEntero("  Año: ", 1900, 2026);
                    int stock = LeerEntero("  Stock: ", 1);

                    switch (tipoMat)
                    {
                        case "1":
                            string estante = LeerTexto("  Estante: ");
                            string signatura = LeerTexto("  Signatura: ");
                            string estadoConv = LeerTexto("  Estado conservación: ");
                            gestion.RegistrarLibro(isbn, titulo, autor, anio, estante, signatura, estadoConv, stock);
                            break;
                        case "2":
                            int edicion = LeerEntero("  Número edición: ", 1);
                            int volumen = LeerEntero("  Volumen: ", 1);
                            string periodicidad = LeerSoloLetras("  Periodicidad (ej. Mensual): ");
                            gestion.RegistrarRevista(isbn, titulo, autor, edicion, volumen, periodicidad, anio, stock);
                            break;
                        case "3":
                            string tutor = LeerSoloLetras("  Tutor: ");
                            string facultad = LeerSoloLetras("  Facultad: ");
                            string carreraMono = LeerSoloLetras("  Carrera: ");
                            gestion.RegistrarMonografia(isbn, titulo, autor, anio, tutor, facultad, carreraMono, stock);
                            break;
                        default:
                            Error("Tipo inválido.");
                            continue;
                    }
                    Exito($"Material {i + 1} registrado.");
                }
                Console.WriteLine("\n  Registros guardados en materiales.json");
                Console.WriteLine("  Revisa tu carpeta bin\\Debug para ver el archivo.");
                break;

            case "2":
                Console.Clear();
                Titulo("REGISTRAR USUARIO");
                Console.WriteLine("  1. Estudiante  2. Docente");
                string tipoUsu = LeerTexto("\n  Tipo: ");

                int cantUsu = LeerEntero("  ¿Cuántos usuarios registrará? ", 1, 100);

                for (int i = 0; i < cantUsu; i++)
                {
                    Console.WriteLine($"\n  ── Usuario {i + 1} de {cantUsu} ───────────────");
                    string nombre = LeerSoloLetras("  Nombre: ");
                    string apellido = LeerSoloLetras("  Apellido: ");
                    string emailUsu = LeerEmail("  Email: ");

                    if (tipoUsu == "1")
                    {
                        string carnet = LeerCarnet("  Carnet (ej. 2024-0001U): ");
                        string carreraEst = LeerSoloLetras("  Carrera: ");
                        gestion.RegistrarEstudiante(nombre, apellido, carnet, carreraEst, emailUsu);
                    }
                    else if (tipoUsu == "2")
                    {
                        string codigo = LeerCodigoDocente("  Código docente (4 caracteres): ");
                        string depto = LeerSoloLetras("  Departamento: ");
                        gestion.RegistrarDocente(nombre, apellido, emailUsu, codigo, depto);
                    }
                    else
                    {
                        Error("Tipo de usuario inválido.");
                        continue;
                    }
                    Exito($"Usuario {i + 1} registrado.");
                }
                Console.WriteLine("\n  Registros guardados en usuarios.json");
                Console.WriteLine("  Revisa tu carpeta bin\\Debug para ver el archivo.");
                break;

            case "3":
                Console.Clear();
                Titulo("REALIZAR PRÉSTAMO");
                string emailPrest = LeerEmail("  Email usuario: ");
                string isbnPrest = LeerISBN("  ISBN material: ");
                gestion.PrestarMaterial(emailPrest, isbnPrest);
                Exito("Préstamo realizado.");
                break;

            case "4":
                Console.Clear();
                Titulo("DEVOLVER MATERIAL");
                string emailDev = LeerEmail("  Email usuario: ");
                string isbnDev = LeerISBN("  ISBN material: ");
                double multa = gestion.DevolverMaterial(emailDev, isbnDev);
                if (multa > 0)
                    Error($"Devuelto con multa: C$ {multa}");
                else
                    Exito("Devuelto sin multa.");
                break;

            case "5":
                Console.Clear();
                Titulo("CATÁLOGO DE MATERIALES");
                var materiales = gestion.ObtenerMateriales();
                if (materiales.Count == 0)
                    Error("No hay materiales registrados.");
                else
                    foreach (var m in materiales)
                        MostrarMaterial(m);
                break;

            case "6":
                Console.Clear();
                Titulo("USUARIOS REGISTRADOS");
                var usuarios = gestion.ObtenerUsuarios();
                if (usuarios.Count == 0)
                    Error("No hay usuarios registrados.");
                else
                    foreach (var u in usuarios)
                        MostrarUsuario(u);
                break;

            case "7":
                Console.Clear();
                Titulo("PRÉSTAMOS ACTIVOS");
                var activos = gestion.ObtenerPrestamosActivos();
                if (activos.Count == 0)
                    Error("No hay préstamos activos.");
                else
                    foreach (var p in activos)
                        MostrarPrestamo(p);
                break;

            case "8":
                Console.Clear();
                Titulo("PRÉSTAMOS POR USUARIO");
                string emailBusc = LeerEmail("  Email usuario: ");
                var prestUsuario = gestion.ObtenerPrestamosPorUsuario(emailBusc);
                if (prestUsuario.Count == 0)
                    Error("No se encontraron préstamos para ese usuario.");
                else
                    foreach (var p in prestUsuario)
                        MostrarPrestamo(p);
                break;

            case "9":
                Console.Clear();
                Titulo("QUIÉN TIENE UN MATERIAL");
                string isbnBusc = LeerISBN("  ISBN material: ");
                var usuariosMat = gestion.ObtenerUsuariosConMaterial(isbnBusc);
                if (usuariosMat.Count == 0)
                    Error("Nadie tiene ese material prestado o no existe.");
                else
                    foreach (var u in usuariosMat)
                        MostrarUsuario(u);
                break;

            case "10":
                Console.Clear();
                Titulo("CONSULTAR MULTA");
                string emailMul = LeerEmail("  Email usuario: ");
                double multaTotal = gestion.ConsultarMulta(emailMul);
                if (multaTotal > 0)
                    Error($"Multa pendiente: C$ {multaTotal}");
                else
                    Exito("Sin multa pendiente.");
                break;

            case "11":
                Console.Clear();
                Titulo("ELIMINAR MATERIAL");
                string isbnElim = LeerISBN("  ISBN del material a eliminar: ");
                try
                {
                    var matElim = gestion.BuscarMaterial(isbnElim);
                    Console.WriteLine("\n  Material encontrado:");
                    MostrarMaterial(matElim);
                    Console.Write("  ¿Confirmar eliminación? (s/n): ");
                    string confirmMat = Console.ReadLine()!.Trim().ToLower();
                    if (confirmMat == "s")
                    {
                        gestion.EliminarMaterial(isbnElim);
                        Exito("Material eliminado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("\n  Operación cancelada.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Error(ex.Message);
                }
                break;

            case "12":
                Console.Clear();
                Titulo("ELIMINAR USUARIO");
                string emailElim = LeerEmail("  Email del usuario a eliminar: ");
                try
                {
                    var usuElim = gestion.BuscarUsuario(emailElim);
                    Console.WriteLine("\n  Usuario encontrado:");
                    MostrarUsuario(usuElim);
                    Console.Write("  ¿Confirmar eliminación? (si/no): ");
                    string confirmUsu = Console.ReadLine()!.Trim().ToLower();
                    if (confirmUsu == "si")
                    {
                        gestion.EliminarUsuario(emailElim);
                        Exito("Usuario eliminado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("\n  Operación cancelada.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Error(ex.Message);
                }
                break;

            case "0":
                salir = true;
                break;

            default:
                Error("Opción inválida.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Error(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Error(ex.Message);
    }
    catch (Exception ex)
    {
        Error($"Error inesperado: {ex.Message}");
    }

    if (!salir)
    {
        Console.Write("\n  Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n  Saliendo del sistema..");
Console.ResetColor();
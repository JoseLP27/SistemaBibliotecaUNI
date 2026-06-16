public class GestionBiblioteca
{
    private Dictionary<string, MaterialBibliografico> _catalogo = new();
    private Dictionary<string, Usuario> _usuarios = new();
    private List<Prestamo> _prestamos = new();
}
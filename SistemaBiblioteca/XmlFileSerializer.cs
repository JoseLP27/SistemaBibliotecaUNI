using System.Xml.Serialization;

public class XmlFileSerializer<T> : ISerializer<T>
{
    private static readonly XmlSerializer _serializer = new(typeof(List<T>));

    public void Guardar(List<T> datos, string rutaArchivo)
    {
        using StreamWriter writer = new(rutaArchivo);
        _serializer.Serialize(writer, datos);
    }

    public List<T> Cargar(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
            return new();

        using StreamReader reader = new(rutaArchivo);
        return _serializer.Deserialize(reader) as List<T> ?? new();
    }

}
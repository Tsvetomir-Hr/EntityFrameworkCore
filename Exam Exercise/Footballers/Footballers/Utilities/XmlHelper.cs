using System.Text;
using System.Xml.Serialization;


namespace Footballers.Utilities;

public class XmlHelper
{

    public T Deserialize<T>(string inputXml, string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

        StringReader readerXml = new StringReader(inputXml);

        T targetDto = (T)xmlSerializer.Deserialize(readerXml);

        return targetDto;
    }
    public string Serialize<T>(T dto, string rootName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        using StringWriter stringWriter = new StringWriter(stringBuilder);

        XmlRootAttribute root = new XmlRootAttribute(rootName);
        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);


        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), root);
        xmlSerializer.Serialize(stringWriter, dto, namespaces);

        return stringBuilder.ToString().TrimEnd();


    }
}

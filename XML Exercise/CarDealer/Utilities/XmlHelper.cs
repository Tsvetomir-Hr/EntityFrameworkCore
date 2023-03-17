using CarDealer.DTOs.Import;
using System.Xml.Serialization;

namespace CarDealer.Utilities;

public class XmlHelper
{

    public T Deserialize<T>(string inputXml,string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

        StringReader readerXml = new StringReader(inputXml);

        T targetDto = (T)xmlSerializer.Deserialize(readerXml);

        return targetDto;
    }
}

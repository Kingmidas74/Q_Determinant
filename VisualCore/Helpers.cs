using System.IO;

namespace VisualCore
{
    public class Helpers
    {
        static public T XamlClone<T>(T source)
        {
            var savedObject = System.Windows.Markup.XamlWriter.Save(source);
            var stringReader = new StringReader(savedObject);
            System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
            var target = (T)System.Windows.Markup.XamlReader.Load(xmlReader);
            return target;
        }
    }
}

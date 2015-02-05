using System.Diagnostics;
using Core.Atoms;
using MsgPack.Serialization;
using System.IO;

namespace Core.Converters
{
    internal class MessagePackConverter:AbstractConverter
    {
        internal override T DataToGraph<T>(string data)
        {
            var bytes = new byte[data.Length * sizeof(char)];
            System.Buffer.BlockCopy(data.ToCharArray(), 0, bytes, 0, bytes.Length);
            using (var byteStream = new MemoryStream(bytes))
            {
                var serializer = MessagePackSerializer.Get<T>();
                byteStream.Position = 0;
                //serializer.Unpack()
                var a = serializer.Unpack(byteStream);
                Debug.WriteLine(a);
                Debug.WriteLine((a as Graph).Vertices.Count);
                return serializer.Unpack(byteStream);
            }
        }

        internal override string GraphToData<T>(T graph)
        {
            using (var byteStream = new MemoryStream())
            {
                var serializer = MessagePackSerializer.Get<T>();
                serializer.Pack(byteStream, graph);
                byteStream.Position = 0;
                return new StreamReader(byteStream).ReadToEnd();
            }
        }
    }
}

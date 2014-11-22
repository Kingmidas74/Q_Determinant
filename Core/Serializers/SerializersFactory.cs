namespace Core.Serializers
{
    public static class SerializersFactory
    {
        public static AbstractSerializer GeSerializer(SerializeTypes type=SerializeTypes.Binary)
        {
            switch (type)
            {
                case SerializeTypes.Binary: return new BinarySerializer();
            }
            return new BinarySerializer();
        }
    }
}

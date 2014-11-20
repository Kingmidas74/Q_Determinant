namespace Core.Serializers
{
    public static class Factory
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

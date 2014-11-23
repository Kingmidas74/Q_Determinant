﻿using System;

namespace Core.Serializers.SerializationModels.SolutionModels
{
    [Serializable()]
    public class Function
    {
        public string Signature { get; set; }
        public ulong MinimumParameters { get; set; }
        public FunctionPriorities Priority { get; set; }
    }
}

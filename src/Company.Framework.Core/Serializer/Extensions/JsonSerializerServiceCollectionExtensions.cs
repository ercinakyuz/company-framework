﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Company.Framework.Core.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Core.Serializer.Extensions
{
    public static class JsonSerializerServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonSerializer(this IServiceCollection serviceCollection)
        {
           return serviceCollection
               .AddSingleton(new JsonSerializerOptions
               {
                   PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                   DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                   Converters = { new CoreIdJsonConverterFactory() }
               })
               .AddSingleton<IJsonSerializer, CoreJsonSerializer>();
        }
    }
}

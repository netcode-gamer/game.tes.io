﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Contoso.GameNetCore.Hosting.Internal
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection Clone(this IServiceCollection serviceCollection)
        {
            var clone = new ServiceCollection();
            foreach (var service in serviceCollection)
                clone.Add(service);
            return clone;
        }
    }
}

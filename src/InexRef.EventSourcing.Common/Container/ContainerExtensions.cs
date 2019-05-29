#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InexRef.EventSourcing.Common.Container
{
    public static class ContainerExtensions
    {
        public static void ConfigureFrom<TModule>(this IServiceCollection serviceCollection)
            where TModule : ContainerConfigurationModule, new()
        {
            var module = new TModule();
            module.ConfigureContainer(serviceCollection);
        }

        public static IServiceCollection AddDecorator<TService>(this IServiceCollection services, Func<TService, TService> decoratorFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (decoratorFactory == null)
            {
                throw new ArgumentNullException(nameof(decoratorFactory));
            }

            var serviceType = typeof(TService);

            var matchingTypeDescriptors = services.Where(d => d.ServiceType == serviceType).ToArray();

            if (matchingTypeDescriptors.Length == 0)
            {
                throw new DecoratorSetupException($"Decorator setup failed, service type not registered: {serviceType.FullName}");
            }

            if (matchingTypeDescriptors.Length > 1)
            {
                throw new DecoratorSetupException($"Decorator setup failed, multiple registrations for service type: {serviceType.FullName}");
            }

            var originalDescriptor = matchingTypeDescriptors[0];
            var redirectionOfOriginalType = typeof(RedirectedType<>).MakeGenericType(typeof(TService));
            var redirectionOfOriginalTypeDescriptor = new ServiceDescriptor(
                redirectionOfOriginalType,
                originalDescriptor.ImplementationType,
                originalDescriptor.Lifetime);

            services.Add(redirectionOfOriginalTypeDescriptor);

            var decoratorDescriptor = new ServiceDescriptor(
                serviceType,
                sp => decoratorFactory.Invoke((TService)sp.GetService(redirectionOfOriginalType)),
                originalDescriptor.Lifetime);
            services.Replace(decoratorDescriptor);

            return services;
        }

        private class RedirectedType<TOriginalType>
        {
        }

        public class DecoratorSetupException : InvalidOperationException
        {
            public DecoratorSetupException(string message) : base(message)
            {

            }
        }
    }
}
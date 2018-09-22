using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Handler;

namespace CustomerEquipmentApi.Modules
{
    public class HandlerModule:Module
    {
        /// <inheritdoc />
        /// <summary>
        ///     Load dependencies
        /// </summary>
        /// <param name="builder">Container</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EquipmentsHandler>().As<IEquipmentsHandler>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Repository;


namespace CustomerEquipmentApi.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>().InstancePerLifetimeScope();
        }
    }
}

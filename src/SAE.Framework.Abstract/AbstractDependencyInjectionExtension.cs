using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Abstract.Authorization.ABAC;
using SAE.Framework.Abstract.Builder;
using SAE.Framework.Abstract.Mediator;
using SAE.Framework.Abstract.Responsibility;
using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展
    /// </summary>
    public static class AbstractDependencyInjectionExtension
    {

        /// <summary>
        /// 添加 <seealso cref="IMediator"/>实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="assemblies">实现了<see cref="ICommandHandler{TCommand}"/>和<see cref="ICommandHandler{TCommand, TResponse}"/>接口的程序集</param>
        /// <returns><see cref="IMediatorBuilder"/></returns>
        public static IMediatorBuilder AddMediator(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            var services = builder.Services;

            if (assemblies == null || !assemblies.Any()) assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
#pragma warning disable CS0618 // Type or member is obsolete
            var mediatorHandler = typeof(IMediatorHandler);
#pragma warning restore CS0618 // Type or member is obsolete

            var descriptors = new List<CommandHandlerDescriptor>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetAssignableFrom(mediatorHandler)
                                             .Where(t => t.IsClass &&
                                                    !t.IsAbstract &&
                                                    !t.IsInterface))
                {

                    foreach (var interfaceType in type.GetInterfaces()
                                                      .Where(t => mediatorHandler.IsAssignableFrom(t) && t != mediatorHandler))
                    {
                        if (!services.Any(s => s.ServiceType == interfaceType && s.ImplementationType == type))
                        {
                            services.AddSingleton(interfaceType, type);
                            if (interfaceType.IsGenericType)
                            {
                                descriptors.Add(new CommandHandlerDescriptor(interfaceType, interfaceType.GenericTypeArguments));
                            }
                        }
                    }
                }
            }

            services.TryAddTransient<IMediator, DefaultMediator>();

            return new MediatorBuilder(services, descriptors);
        }

        /// <summary>
        /// 添加<seealso cref="IBuilder"/>实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="assemblies">实现了<see cref="IBuilder"/>接口的程序集</param>
        /// <returns><see cref="ISAEFrameworkBuilder"/></returns>
        public static ISAEFrameworkBuilder AddBuilder(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            var services = builder.Services;

            if (assemblies == null || !assemblies.Any()) assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            services.TryAddSingleton<IDirector, DefaultDirector>();

#pragma warning disable CS0618 // Type or member is obsolete
            var builderType = typeof(IBuilder);
#pragma warning restore CS0618 // Type or member is obsolete

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes()
                                             .Where(t => t.IsClass &&
                                                    !t.IsAbstract &&
                                                    !t.IsInterface &&
                                                    builderType.IsAssignableFrom(t)))
                {

                    foreach (var interfaceType in type.GetInterfaces()
                                                      .Where(t => builderType.IsAssignableFrom(t) && t != builderType))
                    {
                        if (!services.Any(s => s.ServiceType == interfaceType && s.ImplementationType == type))
                        {
                            services.AddSingleton(interfaceType, type);
                        }
                    }
                }
            }

            return builder;
        }
        /// <summary>
        /// 添加<seealso cref="IResponsibilityProvider{TResponsibilityContext}"/>实现
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>
        /// <param name="builder">服务集合</param>
        /// <returns><see cref="ISAEFrameworkBuilder"/></returns>
        public static ISAEFrameworkBuilder AddResponsibilityProvider<TContext>(this ISAEFrameworkBuilder builder) where TContext : ResponsibilityContext
        {
            var services = builder.Services;
            services.TryAddSingleton<IResponsibilityProvider<TContext>, ResponsibilityProvider<TContext>>();
            return builder;
        }

        /// <summary>
        /// 添加<seealso cref="IResponsibility{TContext}"/>实现
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>
        /// <typeparam name="TResponsibility">具体实现的链条</typeparam>
        /// <param name="builder">服务集合</param>
        /// <returns><see cref="ISAEFrameworkBuilder"/></returns>
        public static ISAEFrameworkBuilder AddResponsibility<TContext, TResponsibility>(this ISAEFrameworkBuilder builder)
            where TResponsibility : class, IResponsibility<TContext>
            where TContext : ResponsibilityContext
        {
            var services = builder.Services;

            services.AddSingleton<IResponsibility<TContext>, TResponsibility>();

            builder.AddResponsibilityProvider<TContext>();

            return builder;
        }

        /// <summary>
        /// 添加<seealso cref="IResponsibility{TContext}"/>实现
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>
        /// <typeparam name="TResponsibility">具体实现的链条</typeparam>
        /// <param name="builder">符合集合</param>
        /// <param name="responsibility">链条</param>
        /// <returns><see cref="ISAEFrameworkBuilder"/></returns>
        public static ISAEFrameworkBuilder AddResponsibility<TContext, TResponsibility>(this ISAEFrameworkBuilder builder, TResponsibility responsibility)
            where TResponsibility : class, IResponsibility<TContext>
            where TContext : ResponsibilityContext
        {
            var services = builder.Services;
            services.AddSingleton<IResponsibility<TContext>>(responsibility);
            builder.AddResponsibilityProvider<TContext>();

            return builder;
        }

        /// <summary>
        /// 添加基于ABAC授权
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// <para>
        /// 除此之外，您应当使用<seealso cref="AddRuleContextProvider{TRuleContextProvider}(ABACDependencyInjectionBuilder)"/>和
        /// <seealso cref="AddAuthorizeService{TAuthorizeService}(ABACDependencyInjectionBuilder)"/>注册默认的依赖服务。
        /// </para>
        /// <para>该服务实现分别实现<seealso cref="IRuleContextProvider"/>和<seealso cref="IAuthorizeService"/>接口。
        /// </para>
        /// </remarks>
        /// <returns><see cref="ABACDependencyInjectionBuilder"/></returns>
        public static ABACDependencyInjectionBuilder AddABACAuthorization(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;

            services.TryAddSingleton<IRuleDecoratorBuilder, DefaultRuleDecoratorBuilder>();
            services.TryAddSingleton<IRuleContextFactory, DefaultRuleContextFactory>();
            services.TryAddSingleton<IPropertyConvertor<bool>, DefaultPropertyConvertor>();
            services.TryAddSingleton<IPropertyConvertor<float>, DefaultPropertyConvertor>();
            services.TryAddSingleton<IPropertyConvertor<DateTime>, DefaultPropertyConvertor>();
            services.TryAddSingleton<IPropertyConvertor<TimeSpan>, DefaultPropertyConvertor>();
            services.TryAddSingleton<IPropertyConvertor<string>, DefaultPropertyConvertor>();
            builder.AddDefaultLogger()
                   .AddMemoryCache();

            return new ABACDependencyInjectionBuilder(services);
        }

        /// <summary>
        /// 添加ABAC<seealso cref="IRuleContextProvider"/>
        /// </summary>
        /// <typeparam name="TRuleContextProvider">
        /// <seealso cref="IRuleContextProvider"/>实现
        /// </typeparam>
        /// <returns><see cref="ABACDependencyInjectionBuilder"/></returns>
        public static ABACDependencyInjectionBuilder AddRuleContextProvider<TRuleContextProvider>(this ABACDependencyInjectionBuilder builder) where TRuleContextProvider : class, IRuleContextProvider
        {
            if (!builder.Services.IsRegister<IRuleContextProvider, TRuleContextProvider>())
            {
                builder.Services.AddSingleton<IRuleContextProvider, TRuleContextProvider>();
            }
            return builder;
        }

        /// <summary>
        /// 添加ABAC<seealso cref="IAuthorizeService"/>
        /// </summary>
        /// <typeparam name="TAuthorizeService">
        /// <seealso cref="IAuthorizeService"/>实现
        /// </typeparam>
        /// <returns><see cref="ABACDependencyInjectionBuilder"/></returns>
        public static ABACDependencyInjectionBuilder AddAuthorizeService<TAuthorizeService>(this ABACDependencyInjectionBuilder builder) where TAuthorizeService : class, IAuthorizeService
        {
            if (!builder.Services.IsRegister<IAuthorizeService, TAuthorizeService>())
            {
                builder.Services.AddSingleton<IAuthorizeService, TAuthorizeService>();
            }
            return builder;
        }
    }
}

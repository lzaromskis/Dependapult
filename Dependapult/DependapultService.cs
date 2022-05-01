// Copyright (c) 2022 Lukas Žaromskis
// Licensed under the MIT License

using System;
using Dependapult.Exceptions;
using Dependapult.Factories;

namespace Dependapult
{
    /// <summary>
    /// Provides a dependency injection service.
    /// </summary>
    public class DependapultService
    {
        private readonly IContainer _container;
        private static readonly Lazy<DependapultService> _service = new(() => new DependapultService());

        /// <summary>
        /// Gets an instance of DependapultService.
        /// </summary>
        public static DependapultService Instance
        {
            get { return _service.Value; }
        }

        internal DependapultService()
        {
            _container = new ContainerFactory().CreateContainer();
        }

        internal DependapultService(IContainer container)
        {
            _container = container;
        }

        private bool CanRegister(DependencyLifetime lifetime, bool replace, Type interfaceType)
        {
            if (replace && lifetime == DependencyLifetime.Singleton)
                throw new IllegalTypeRegistrationException("Singleton objects cannot be replaced");

            return replace || !_container.ContainsType(interfaceType);
        }

        /// <summary>
        /// Registers a dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <param name="lifetime">Lifetime of the object that will be created.</param>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependency was registered.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IllegalTypeRegistrationException"></exception>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterDependency<TIDep, TDep>(DependencyLifetime lifetime, bool replace = false) where TDep : class, TIDep
        {
            var interfaceType = typeof(TIDep);
            var implementorType = typeof(TDep);
            if (!CanRegister(lifetime, replace, interfaceType))
                return false;

            return _container.Add(lifetime, interfaceType, implementorType);
        }

        /// <summary>
        /// Registers a dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <param name="lifetime">Lifetime of the object that will be created.</param>
        /// <param name="args">Arguments to pass when creating an object of type TDep.</param>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependency was registered.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IllegalTypeRegistrationException"></exception>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterDependency<TIDep, TDep>(DependencyLifetime lifetime, object[] args, bool replace = false) where TDep : class, TIDep
        {
            var interfaceType = typeof(TIDep);
            var implementorType = typeof(TDep);
            if (!CanRegister(lifetime, replace, interfaceType))
                return false;

            return _container.Add(lifetime, interfaceType, implementorType, args);
        }

        /// <summary>
        /// Registers a dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface of base type of a dependency.</typeparam>
        /// <param name="lifetime">Lifetime of the object that will be created.</param>
        /// <param name="creatorFunction">Function which returns an object of TIDep type.</param>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependecy was registered.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IllegalTypeRegistrationException"></exception>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterDependency<TIDep>(DependencyLifetime lifetime, Func<DependapultService, TIDep> creatorFunction, bool replace = false) where TIDep : class
        {
            var interfaceType = typeof(TIDep);
            if (!CanRegister(lifetime, replace, interfaceType))
                return false;

            return _container.Add(lifetime, interfaceType, creatorFunction);
        }

        /// <summary>
        /// Registers a singleton dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <returns>True, if a dependecy was registered.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterSingleton<TIDep, TDep>() where TDep : class, TIDep
        {
            return RegisterDependency<TIDep, TDep>(DependencyLifetime.Singleton);
        }

        /// <summary>
        /// Registers a singleton dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <param name="args">Arguments to pass when creating an object of type TDep.</param>
        /// <returns>True, if a dependecy was registered.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterSingleton<TIDep, TDep>(object[] args) where TDep : class, TIDep
        {
            return RegisterDependency<TIDep, TDep>(DependencyLifetime.Singleton, args);
        }

        /// <summary>
        /// Registers a singleton dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <param name="creatorFunction">Function which returns an object of TIDep type.</param>
        /// <returns>True, if a dependecy was registered.</returns>
        public bool RegisterSingleton<TIDep>(Func<DependapultService, TIDep> creatorFunction) where TIDep : class
        {
            return RegisterDependency(DependencyLifetime.Singleton, creatorFunction, false);
        }

        /// <summary>
        /// Registers a transient dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependecy was registered.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterTransient<TIDep, TDep>(bool replace = false) where TDep : class, TIDep
        {
            return RegisterDependency<TIDep, TDep>(DependencyLifetime.Transient, replace);
        }

        /// <summary>
        /// Registers a transient dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <typeparam name="TDep">Specific implementation tyoe of a dependency.</typeparam>
        /// <param name="args">Arguments to pass when creating an object of type TDep.</param>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependecy was registered.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterTransient<TIDep, TDep>(object[] args, bool replace = false) where TDep : class, TIDep
        {
            return RegisterDependency<TIDep, TDep>(DependencyLifetime.Transient, args, replace);
        }

        /// <summary>
        /// Registers a transient dependency.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <param name="creatorFunction">Function which returns an object of TIDep type.</param>
        /// <param name="replace">Should an already registered dependency of type TIDep be replaced. Only valid for transient lifetime.</param>
        /// <returns>True, if a dependecy was registered</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        public bool RegisterTransient<TIDep>(Func<DependapultService, TIDep> creatorFunction, bool replace = false) where TIDep : class
        {
            return RegisterDependency(DependencyLifetime.Transient, creatorFunction, replace);
        }

        /// <summary>
        /// Gets a dependency object.
        /// </summary>
        /// <typeparam name="TIDep">Interface or base type of a dependency.</typeparam>
        /// <returns>Object of TIDep type.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        /// <exception cref="TypeNotRegisteredException"></exception>
        public TIDep GetDependency<TIDep>() where TIDep : class
        {
            return (TIDep)GetDependency(typeof(TIDep));
        }

        /// <summary>
        /// Gets a dependency object. No casting is applied.
        /// </summary>
        /// <param name="type">Interface of base type of a dependency.</param>
        /// <returns>Object.</returns>
        /// <exception cref="CouldNotCreateObjectOfTypeException"></exception>
        /// <exception cref="TypeNotRegisteredException"></exception>
        public object GetDependency(Type type)
        {
            return _container.Get(type);
        }

        internal Func<object> GetDependencyCreatorFunction(Type type)
        {
            return _container.GetCreatorFunction(type);
        }
    }
}

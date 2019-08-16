using System;
using System.Collections.Generic;
using System.Text;

namespace Dependous.Autofac
{
    /// <summary>
    /// Marks an implementation as a decorator of another type.
    /// </summary>
    public interface IDecorator
    {
    }

    /// <summary>
    /// Marks an implementation as a decorator of another type.
    /// </summary>
    /// <typeparam name="TDecoratee">The type of the decoratee.</typeparam>
    public interface IDecorator<TDecoratee> : IDecorator
    {
    }
}
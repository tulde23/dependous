using System;
using System.Collections.Generic;
using System.Text;

namespace Dependous
{
    /// <summary>
    /// A marker interface indicating that you wish to register a concrete instance with the
    /// container as a transient (per instance) dependency.
    /// </summary>
    public interface ISelfTransient
    {
    }
}
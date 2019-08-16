﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dependous
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Dependous.IAssemblySearchPattern" />
    public class Custom : IAssemblySearchPattern
    {
        /// <summary>
        /// The name predicate
        /// </summary>
        private Func<string, bool> namePredicate;
      
        /// <summary>
        /// Initializes a new instance of the <see cref="Custom" /> class.
        /// </summary>
        /// <param name="namePredicate">The name predicate.</param>
        public Custom(Func<string, bool> namePredicate=null)
        {
            this.namePredicate = namePredicate;
        }
        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>
        /// bool
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Match(string assemblyName)
        {
            var result =  this.namePredicate?.Invoke(assemblyName);
            return result.HasValue ? result.Value : false;
        }

        /// <summary>
        /// Matches the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>
        /// bool
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Match(AssemblyName assemblyName)
        {
            return Match(assemblyName.Name);
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Dependous.Autofac.Models;

namespace Dependous.Autofac.Contracts
{
    internal interface IRegistrationRule
    {
         RegistrationResult Register(DependencyMetadata dependencyMetadata);
    }
   
}

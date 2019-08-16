using System;
using System.Collections.Generic;
using System.Text;
using Dependous.GraceIoc.Models;

namespace Dependous.GraceIoc.Contracts
{
    internal interface IRegistrationRule
    {
         RegistrationResult Register(DependencyMetadata dependencyMetadata);
    }
   
}

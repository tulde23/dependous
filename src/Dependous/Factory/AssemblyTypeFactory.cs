namespace Dependous.Factory
{
    internal static class AssemblyTypeFactory
    {
        public static IAssemblyTypeService Resolve()
        {
            return new DefaultAssemblyTypeService();
        }
    }
}
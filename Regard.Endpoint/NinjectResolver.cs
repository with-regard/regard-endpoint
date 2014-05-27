using System.Web.Http.Dependencies;
using Ninject;

namespace Regard.Endpoint
{
    public class NinjectResolver : NinjectScope, IDependencyResolver
    {
        private readonly IKernel m_Kernel;

        public NinjectResolver(IKernel kernel) 
            : base(kernel)
        {
            m_Kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectScope(m_Kernel);
        }
    }
}
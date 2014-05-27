using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;

namespace Regard.Endpoint
{
    public class NinjectScope : IDependencyScope
    {
        protected IResolutionRoot m_ResolutionRoot;

        public NinjectScope(IResolutionRoot kernel)
        {
            m_ResolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = m_ResolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return m_ResolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = m_ResolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return m_ResolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            //Don't dispose the kernel!
            //IDisposable disposable = (IDisposable)resolutionRoot;
            //if (disposable != null) disposable.Dispose();
            //resolutionRoot = null;
        }
    }
}
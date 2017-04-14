using Ninject.Activation;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace WebCalendar.DAL.Helpers
{
    public static class NinjectHelper
    {
        public static void InScope(this IEnumerable<IHaveBindingConfiguration> bindings, Func<IContext, object> scope)
        {
            if (scope != null)
            {
                bindings.ForEach(b => b.BindingConfiguration.ScopeCallback = scope);
            }
        }
    }
}

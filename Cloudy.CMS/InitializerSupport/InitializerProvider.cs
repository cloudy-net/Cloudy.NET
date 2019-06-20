using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.InitializerSupport
{
    public class InitializerProvider : IInitializerProvider
    {
        IEnumerable<IInitializer> Instances { get; }

        public InitializerProvider(IInitializerCreator initializerCreator)
        {
            Instances = initializerCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<IInitializer> GetAll()
        {
            return Instances;
        }
    }
}

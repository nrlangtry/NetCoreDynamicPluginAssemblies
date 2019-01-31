using NetCoreDynamicPluginAssemblies.Processor;
using System;

namespace NetCoreDynamicPluginAssemblies.CustomProcessorOne
{
    public class CustomProcessor : Processor.Processor
    {
        protected override string Step1()
        {
            var result = base.Step1();

            result += " appended by CustomProcessorOne";

            return result;
        }
    }
}

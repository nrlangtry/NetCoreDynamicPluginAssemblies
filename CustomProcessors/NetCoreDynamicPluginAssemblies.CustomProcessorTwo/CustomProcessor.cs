using NetCoreDynamicPluginAssemblies.Processor;
using System;

namespace NetCoreDynamicPluginAssemblies.CustomProcessorTwo
{
    public class CustomProcessor : Processor.Processor
    {
        protected override string Step2()
        {
            var result = base.Step2();
            result += " appended by CustomProcessorTwo";

            return result;
        }

        protected override string Step3()
        {
            var result = base.Step3();
            result += " appended by CustomProcessorTwo";

            return result;
        }
    }
}

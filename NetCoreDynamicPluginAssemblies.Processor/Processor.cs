using NetCoreDynamicPluginAssemblies.Common;
using System;
using System.Text;

namespace NetCoreDynamicPluginAssemblies.Processor
{
    public class Processor : IProcessor
    {
        protected virtual string Step1()
        {
            return "Step 1";
        }

        protected virtual string Step2()
        {
            return "Step 2";
        }

        protected virtual string Step3()
        {
            return "Step 3";
        }

        public string DoWork()
        {
            var sb = new StringBuilder();

            var step1Result = Step1();
            sb.AppendLine(step1Result);

            var step2Result = Step2();
            sb.AppendLine(step2Result);

            var step3Result = Step3();
            sb.AppendLine(step3Result);

            return sb.ToString();
        }
    }
}

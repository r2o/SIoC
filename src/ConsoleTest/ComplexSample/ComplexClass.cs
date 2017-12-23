namespace ConsoleTest.ComplexSample
{
    using System;

    public class ComplexClass : IComplexInterface
    {
        private readonly IComplexInterfaceParameter parm;

        public ComplexClass(IComplexInterfaceParameter parm)
        {
            this.parm = parm;
        }

        public void SaySomething()
        {
            Console.WriteLine(parm.GetMessage());
        }
    }
}

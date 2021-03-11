using PCBuilder.Builder;

namespace PCBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pc pc = new Pc();
            // pc.BuildPcParts();
            // pc.PrintPcSpecification();

            var director = new Director();
            var builder = new Builder.Builder();

            director.Builder = builder;
            
            director.BuildComputer();
            
            builder.GetPc().PrintPcSpecification();
        }
    }
}
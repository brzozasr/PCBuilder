using System;
using PCBuilder.Builder;

namespace PCBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var director = new Director();
            var builder = new Builder.Builder();

            director.Builder = builder;
            
            director.BuildComputer();
            
            Console.Clear();
            builder.GetPc().PrintPcSpecification();

        }
    }
}
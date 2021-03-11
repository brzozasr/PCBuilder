using System.Collections.Generic;
using PCBuilder.PCParts;
using PCBuilder.PCs;

namespace PCBuilder.Builder
{
    public interface IBuilder
    {
        void BuildPc();
        Pc GetPc();
    }
}
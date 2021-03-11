using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PCBuilder.PCParts;
using PCBuilder.Utilities;
using HeatSink = PCBuilder.PCParts.HeatSink;

namespace PCBuilder.PCs
{
    public class Pc
    {
        private IList<PCPart> _parts = new List<PCPart>();


        public void BuildPcParts()
        {
            var subclassTypes = Assembly
                .GetAssembly(typeof(PCPart))?
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(PCPart)));

            if (subclassTypes != null)
            {
                foreach (var type in subclassTypes)
                {
                    var obj = Activator.CreateInstance(type) as PCPart;

                    var isPcPartAdded = _parts.Any(p => p.GetType().Name == type.Name);

                    try
                    {
                        if (!isPcPartAdded && obj is not null)
                        {
                            SetValuePart(obj);
                            _parts.Add(obj);
                        }
                        else
                        {
                            throw new ArgumentException($"{type.Name} is already in the PC");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void SetValuePart(PCPart part)
        {
            if (part is Cpu cpu)
            {
                int randCpu = Utils.Random.Next(0, Enum.GetNames(typeof(CpuList)).Length);
                part.Name = ((CpuList) randCpu).GetDescription();
                part.IsSuppliesPower = false;
                part.Power = 250;
                part.IsAbsorbHeat = false;
                part.Temperature = 120;
            }
            else if (part is Ram ram)
            {
                int randRam = Utils.Random.Next(0, Enum.GetNames(typeof(RamList)).Length);
                part.Name = ((RamList) randRam).GetDescription();
                part.IsSuppliesPower = false;
                part.Power = 150;
                part.IsAbsorbHeat = false;
                part.Temperature = 100;
            }
            else if (part is PowerSupplier powerSupplier)
            {
                int[] power = new int[Enum.GetNames(typeof(PowerSupplierList)).Length];

                int randInt = Utils.Random.Next(0, Enum.GetNames(typeof(PowerSupplierList)).Length);
                int i = 0;
                foreach (var value in Enum.GetValues(typeof(PowerSupplierList)))
                {
                    power[i] = (int) value;
                    i++;
                }

                part.Name = ((PowerSupplierList) power[randInt]).GetDescription();
                part.IsSuppliesPower = true;
                part.Power = power[randInt];
                part.IsAbsorbHeat = false;
                part.Temperature = 150;
            }
            else if (part is HeatSink heatSink)
            {
                int[] temperature = new int[Enum.GetNames(typeof(HeatSinkList)).Length];

                int randInt = Utils.Random.Next(0, Enum.GetNames(typeof(HeatSinkList)).Length);
                int i = 0;
                foreach (var value in Enum.GetValues(typeof(HeatSinkList)))
                {
                    temperature[i] = (int) value;
                    i++;
                }

                part.Name = ((HeatSinkList) temperature[randInt]).GetDescription();
                part.IsSuppliesPower = false;
                part.Power = 80;
                part.IsAbsorbHeat = true;
                part.Temperature = temperature[randInt];
            }
        }

        public bool PowerBalance()
        {
            throw new NotImplementedException();
        }

        public int TemperatureBalance()
        {
            throw new NotImplementedException();
        }

        public void PrintPcSpecification()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("== PC SET ==");
            foreach (var part in _parts)
            {
                sb.AppendLine(
                    $"{part.GetType().Name}: {part.Name}, IsSuppliesPower: {part.IsSuppliesPower}, Power: {part.Power}, IsAbsorbHeat: {part.IsAbsorbHeat}, Temperature: {part.Temperature}");
            }

            sb.AppendLine("======================================");

            Console.Write(sb.ToString());
        }
    }
}
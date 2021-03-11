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
            int[] randPower = new[] {50, 100, 150, 200, 250, 300, 350};
            int[] randTemperature = new[] {50, 100, 150, 200, 250, 300, 350};
            int randEnv = Utils.Random.Next(0, randPower.Length);
            
            if (part is Cpu cpu)
            {
                int randCpu = Utils.Random.Next(0, Enum.GetNames(typeof(CpuList)).Length);
                part.Name = ((CpuList) randCpu).GetDescription();
                part.IsSuppliesPower = false;
                part.Power = randPower[randEnv];
                part.IsAbsorbHeat = false;
                part.Temperature = randTemperature[randEnv];
            }
            else if (part is Ram ram)
            {
                int randRam = Utils.Random.Next(0, Enum.GetNames(typeof(RamList)).Length);
                part.Name = ((RamList) randRam).GetDescription();
                part.IsSuppliesPower = false;
                part.Power = randPower[randEnv];
                part.IsAbsorbHeat = false;
                part.Temperature = randTemperature[randEnv];;
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
                part.Temperature = randTemperature[randEnv];;
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
                part.Power = randPower[randEnv];
                part.IsAbsorbHeat = true;
                part.Temperature = temperature[randInt];
            }
        }

        private bool IsPowerBalance()
        {
            var powerIn = _parts.Where(ps => ps is PowerSupplier && ps.IsSuppliesPower).Sum(ps => ps.Power);
            var powerSum = _parts.Where(p => p.IsSuppliesPower == false).Sum(p => p.Power);
            
            Console.WriteLine($"Supplies power: {powerIn}, draws power: {powerSum}");

            if (powerIn >= powerSum)
            {
                return true;
            }

            return false;
        }

        public bool IsTemperatureBalance()
        {
            int maxTemperature = 100;
            
            var absorbHeat = _parts.Where(h => h.IsAbsorbHeat).Sum(h => h.Temperature);
            var emitHeat = _parts.Where(p => p.IsAbsorbHeat == false).Sum(p => p.Temperature);

            var cooledTemperature = emitHeat - absorbHeat;

            Console.WriteLine($"Max heat: {maxTemperature}, heat after cooling: {emitHeat - absorbHeat}, emit heat: {emitHeat}, absorb: {absorbHeat}");

            if (cooledTemperature < maxTemperature)
            {
                return true;
            }

            return false;
        }

        public void PrintPcSpecification()
        {
            var powerBalance = IsPowerBalance();
            var temperatureBalance = IsTemperatureBalance();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==== PC SET ====");
            foreach (var part in _parts)
            {
                sb.AppendLine(
                    $"{part.GetType().Name}: {part.Name}, IsSuppliesPower: {part.IsSuppliesPower}, Power: {part.Power}, IsAbsorbHeat: {part.IsAbsorbHeat}, Temperature: {part.Temperature}");
            }

            sb.AppendLine($"The balance of power is {powerBalance}");
            sb.AppendLine($"The balance of temperature is {temperatureBalance}");

            if (powerBalance && temperatureBalance)
            {
                sb.AppendLine($"The computer is running.");
            }
            else
            {
                sb.AppendLine($"The computer is no running.");
            }
            
            sb.AppendLine("==========================================================");

            Console.Write(sb.ToString());
        }
    }
}
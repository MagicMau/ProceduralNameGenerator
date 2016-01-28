using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicMau.ProceduralNameGenerator
{
    public class NameGenerator : Generator
    {

        public NameGenerator(IEnumerable<string> trainingData, int order, double smoothing) : base(trainingData, order, smoothing)
        {

        }

        public string GenerateName(int minLength, int maxLength, int maxDistance, string similarTo, Random rnd)
        {
            string name = Generate(rnd).Replace("#", "");
            if (name.Length < minLength || name.Length > maxLength)
            {
                Console.WriteLine("Rejected because of length: " + name);
                return null;
            }

            if (similarTo != null && LevenshteinDistance.Compute(similarTo, name) > maxDistance)
            {
                Console.WriteLine("Rejected because not similar: " + name);
                return null;
            }

            return name;
        }

        public Task<List<string>> GenerateNames(int count, int length, Random rnd)
        {
            return GenerateNames(count, length, length, 0, null, rnd);
        }

        public async Task<List<string>> GenerateNames(int count, int minLength, int maxLength, int maxDistance, string similarTo, Random rnd)
        {
            return await Task.Run(() =>
            {
                var names = new List<string>();

                while (names.Count < count)
                {
                    string name = GenerateName(minLength, maxLength, maxDistance, similarTo, rnd);
                    if (name != null)
                        names.Add(name);
                }

                return names;
            });
        }
    }
}

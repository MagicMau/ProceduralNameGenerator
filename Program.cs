using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicMau.ProceduralNameGenerator
{
    /// <summary>
    /// C# implementation of http://www.samcodes.co.uk/project/markov-namegen/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //var trainingSample = TrainingData.Waypoints.OrderBy(x => Guid.NewGuid()).Take(1000);

            var generator = new NameGenerator(TrainingData.Waypoints, 3, 0.01);

            var task = generator.GenerateNames(5, 5, new Random());
            task.Wait();
            var names = task.Result;

            foreach(var name in names)
            {
                Console.WriteLine(name);
            }

            Console.ReadKey();
        }
    }
}

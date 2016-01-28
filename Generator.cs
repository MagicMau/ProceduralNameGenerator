using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicMau.ProceduralNameGenerator
{
    /// <summary>
    /// Provides procedural generation of words using high-order Markov chains 
    /// Uses Katz's back-off model - chooses the next character based on conditional probability given the last n-characters (where model order = n) 
    /// and backs down to lower order models when higher models fail 
    /// Uses a Dirichlet prior, which is like additive smoothing and raises the chances of a "random" letter being picked instead of one that's trained in 
    /// 
    /// <seealso cref="https://github.com/Tw1ddle/MarkovNameGenerator/blob/master/src/markov/namegen/Generator.hx"/>
    /// </summary>
    public class Generator
    {
        public int Order { get; set; }
        public double Smoothing { get; set; }

        private List<Model> models;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingData">training data for the generator, array of words</param>
        /// <param name="order">number of models to use, will be of orders up to and including "order"</param>
        /// <param name="smoothing">the dirichlet prior/additive smoothing "randomness" factor</param>
        public Generator(IEnumerable<string> trainingData, int order, double smoothing)
        {
            Order = order;
            Smoothing = smoothing;
            models = new List<Model>();

            // Identify and sort the alphabet used in the training data
            var letters = new HashSet<char>();
            foreach (var word in trainingData)
            {
                foreach (char c in word)
                {
                    letters.Add(c);
                }
            }
            var domain = letters.OrderBy(c => c).ToList();
            domain.Insert(0, '#');
            
            // create models
            for (int i = 0; i < order; i++)
            {
                models.Add(new Model(trainingData, order - i, smoothing, domain));
            }
        }

        /// <summary>
        /// Generates a word
        /// </summary>
        /// <returns></returns>
        public string Generate(Random rnd)
        {
            string name = new string('#', Order);
            char letter = GetLetter(name, rnd);
            while (letter != '#' && letter != '\0')
            {
                name += letter;
                letter = GetLetter(name, rnd);
            }
            return name;
        }

        private char GetLetter(string name, Random rnd)
        {
            char letter = '\0';
            string context = name.Substring(name.Length - Order);
            foreach (var model in models)
            {
                letter = model.Generate(context, rnd);
                if (letter == '\0')
                    context = context.Substring(1);
                else
                    break;
            }
            return letter;
        }
    }
}

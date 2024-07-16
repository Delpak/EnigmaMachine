using System.Linq.Expressions;
using LinqSpecs;
namespace Enigma;

public class Instruction
{
    IList<Step> steps = [];
    public IList<Step> Steps => steps.AsReadOnly();

    public Instruction(string instructionText)
    {
        var pairs = Enumerable.Range(0, instructionText.Length / 2)
                      .Select(i => instructionText.Skip(i * 2).Take(2));

        foreach (var pair in pairs)
            steps.Add(new Step(pair.First(), pair.Last()));
    }

    public Step this[int i] => steps[i];

    public bool Applicable
    {
        get
        {
            var checkExpression = (new NotExceedsMaxSteps() && new AllStepsAreUnique()).ToExpression().Compile();

            return checkExpression.Invoke(this);
        }
    }
    public bool Applied => this.Steps.All(item => item.Done);

    public class Step
    {
        private bool done = false;
        public char? From;
        public char? To;
        public bool Done
        {
            get { return this.done; }
        }
        public void MarkDone()
        {
            this.done = true;
        }

        public Step(char? from, char? to)
        {
            if (!(char.IsAsciiLetter(from!.Value) || char.IsAsciiLetter(to!.Value)))
                throw new ArgumentException();
            this.From = from;
            this.To = to;
        }

        public override string ToString()
        {
            return $"{this.From}{this.To}";
        }
    }

    class NotExceedsMaxSteps : Specification<Instruction>
    {
        private const uint maxSteps = 10;
        public override Expression<Func<Instruction, bool>> ToExpression()
        {
            return c => c.Steps.Count() <= maxSteps;
        }
    }

    class AllStepsAreUnique : Specification<Instruction>
    {
        public override Expression<Func<Instruction, bool>> ToExpression()
        {
            return c => c.Steps.GroupBy(s => s).All(g => g.Count() == 1);
        }
    }
}
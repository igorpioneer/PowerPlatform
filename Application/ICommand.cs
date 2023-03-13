using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommand<TRequest>
    {
        void Execute(TRequest request);
    }

    public interface IQuery<TResult>
    {
        IEnumerable<TResult> Execute();
    }
}

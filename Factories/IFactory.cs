using banetexam2.Models;
using System.Collections.Generic;

namespace banetexam2.Factories
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}
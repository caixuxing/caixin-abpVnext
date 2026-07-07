using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace CaiXin.NiuMa.Domain.Employees.Specifications
{
    public class CreateEmployeeSpec : ISpecification<Employee>
    {
        public bool IsSatisfiedBy(Employee employee)
        {
            if (employee.HireDate.AddYears(1) <= DateTime.Now) throw new ArgumentException("员工入职时间不足一年，无法晋升");

            return true;
        }

        public Expression<Func<Employee, bool>> ToExpression()
        {
            return employee => employee.HireDate.AddYears(1) <= DateTime.Now;
        }
    }
}
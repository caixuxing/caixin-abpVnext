using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace CaiXin.NiuMa.Domain.Employees.Specifications
{
    public class QueryEmployeeByIdSpec : ISpecification<EmployeeAgg>
    {

        private readonly Guid _employeeId;

        // ✅ 通过构造函数传递参数
        public QueryEmployeeByIdSpec(Guid employeeId)
        {
            _employeeId = employeeId;
        }


        public bool IsSatisfiedBy(EmployeeAgg employee)
        {
            return true;
        }

        public Expression<Func<EmployeeAgg, bool>> ToExpression()
        {
            return employee => employee.Id == _employeeId;
        }
    }

}
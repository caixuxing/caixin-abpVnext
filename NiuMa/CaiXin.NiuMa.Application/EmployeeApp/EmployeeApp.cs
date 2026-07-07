using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Domain.Employees;
using CaiXin.NiuMa.Domain.Employees.Entity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Application.EmployeeApp
{
    [ExposeServices(typeof(IEmployeeApp))]
    [UnitOfWork]
    internal sealed class EmployeeApp(IRepository<Employee, Guid> employeeRepo,
                                      IRepository<SysUser, Guid> userRepo,
                                      IGuidGenerator guid) : ApplicationService, IEmployeeApp, ITransientDependency
    {
        public async Task<string> AddEmployee(CreateEmployeeCmd cmd, CancellationToken token)
        {
            Employee employee = Employee.Create(guid.Create(), "9527", cmd.FullName, cmd.Email, cmd.PhoneNumber, cmd.HireDate);
            await employeeRepo.InsertAsync(employee, cancellationToken: token);
            await userRepo.InsertAsync(employee.SysUser, cancellationToken: token);
            await CurrentUnitOfWork!.SaveChangesAsync(token);
            return employee.Id.ToString();
        }

        public async Task GetEmployeeById(Guid id)
        {
            var employeeQuery = await employeeRepo.GetQueryableAsync();
            var data = await employeeQuery.Include(e => e.SysUser).Where(e => e.Id == id).FirstOrDefaultAsync();
            if (data is null) throw new ArgumentException("Employee not found");
            Console.Write(data.Id);
            await Task.CompletedTask;
        }
    }
}
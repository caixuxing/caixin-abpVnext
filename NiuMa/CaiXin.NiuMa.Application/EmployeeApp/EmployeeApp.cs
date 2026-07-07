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
    internal sealed class EmployeeApp(
                           IRepository<Employee, Guid> emplRepo,
                           IRepository<SysUser, Guid> userRepo,
                           IGuidGenerator guidGenerator)
        : ApplicationService, IEmployeeApp, ITransientDependency
    {
        public async Task<string> AddEmployee(CreateEmployeeCmd cmd, CancellationToken token = default)
        {

            Employee employee = Employee.Create(
                guidGenerator.Create(),
                "9527",
                cmd.FullName,
                cmd.Email,
                cmd.PhoneNumber,
                cmd.HireDate);
            await emplRepo.InsertAsync(employee);
            await userRepo.InsertAsync(employee.SysUser);

            await CurrentUnitOfWork!.SaveChangesAsync(token);

            return employee.Id.ToString();
        }

        public async Task GetEmployeeById(Guid id)
        {
            var employeeQuery = await emplRepo.GetQueryableAsync();

            var query = await employeeQuery.Include(e => e.SysUser).Where(e => e.Id == id).FirstOrDefaultAsync();

            await Task.CompletedTask;
        }
    }
}

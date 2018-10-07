using Base.Services;
using Data.Entities;

namespace Data.Services
{
    public interface ITestObjectService: IBaseService<TestObject>
    {
    }

    public class TestObjectService: BaseService<TestObject>, ITestObjectService
    {

    }
}

using Base.DAL;
using Data;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiControllerBase : ApiController
    {
        private IUnitOfWork uofw;
        public IUnitOfWork UnitOfWork
        {
            get => uofw ?? new UnitOfWork(new DataContext());
        }
    }
}

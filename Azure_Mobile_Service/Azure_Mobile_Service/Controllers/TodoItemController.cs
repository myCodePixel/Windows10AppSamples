using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using Azure_Mobile_Service.DataObjects;
using Azure_Mobile_Service.Models;

namespace Azure_Mobile_Service.Controllers
{
    public class BookController : TableController<Book>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Book>(context, Request, Services);
        }

        // GET tables/Book
        public IQueryable<Book> GetAllBooks()
        {
            return Query();
        }

        // GET tables/Book/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Book> GetBook(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Book/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Book> PatchBook(string id, Delta<Book> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Book
        public async Task<IHttpActionResult> PostBook(Book item)
        {
            Book current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Book/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteBook(string id)
        {
            return DeleteAsync(id);
        }
    }
}
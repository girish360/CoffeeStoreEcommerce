using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeShop.Controllers
{
    public class WebExperienceController : Controller
    {
        // GET: WebExperienceController
        public ActionResult Index()
        {
            //cria credencial api
            var apiContext = GetApiContext();

            //cria experiance profile para colocar no payment object
            var list = WebProfile.GetList(apiContext);

            if (!list.Any())
            {
                SeedWebProfile(apiContext);
                list = WebProfile.GetList(apiContext);
            }

            return View(list);
        }

        private void SeedWebProfile(APIContext apiContext)
        {
            var BigGods = new WebProfile()
            {
                name = "Ninja",
                input_fields = new InputFields()
                {
                    no_shipping = 1
                }
            };
            WebProfile.Create(apiContext, BigGods);
        }

        //Get api paypal
        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }

    }
}
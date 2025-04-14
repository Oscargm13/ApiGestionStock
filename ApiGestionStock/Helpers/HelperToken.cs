using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiGestionStock.Helpers
{
    public class HelperToken
    {
        private IHttpContextAccessor contextAccessor;

        public HelperToken(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        //public EmpleadoModel GetEmpleado()
        //{
        //    Claim claim =
        //        this.contextAccessor.HttpContext
        //        .User.FindFirst(x => x.Type == "UserData");
        //    string json = claim.Value;
        //    string jsonEmpleado =
        //        HelperCryptography.DecryptString(json);
        //    EmpleadoModel model = JsonConvert
        //        .DeserializeObject<EmpleadoModel>(jsonEmpleado);
        //    return model;
        //}
    }
}

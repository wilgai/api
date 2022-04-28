
using api.Entities;
using api.Enums;
using api.Helper;
using System.Threading.Tasks;


namespace api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckRolesAsync();
            await CheckUserAsync("00000000000", "Admin", "Admin", "lorimerwilgay23@gmail.com","admin", "0000000000", "Santo Domingo","","Hombre",true, "Admin");
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(
        string identificacion,
        string nombre,
        string apellidos,
        string correo,
        string usuario,
        string telefono,
        string direccion,
        string foto,
        string sexo,
        bool estado,
        string tipo_usuario
            )
        {
            User user = await _userHelper.GetUserAsync(correo);
            if (user == null)
            {
                user = new User
                {
                    nombre = nombre,
                    apellidos = apellidos,
                    Email = correo,
                    UserName = usuario,
                    PhoneNumber = telefono,
                    direccion = direccion,
                    identificacion = identificacion,
                    foto=foto,
                    sexo=sexo,
                    estado=estado,
                    tipo_usuario = tipo_usuario
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, tipo_usuario.ToString());
            }

            return user;
        }
    }
}

    


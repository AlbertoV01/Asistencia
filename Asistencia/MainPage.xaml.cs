

using MongoDB.Driver;
using Plugin.Firebase.Android;
using Plugin.Firebase.CloudMessaging;
using System.Linq.Expressions;
using System.Reflection;

namespace Asistencia;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        opMongo insert = new opMongo();
       // insert.InsertarProfesores("Javier", "Paco");

	}

    //private async void Suscribe()
    //{
    //    await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
    //    await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("Bienvenido");
    //   // this.Status.Text = "Ahora estas suscrito";
    //    await DisplayAlert("Bienvenido", "Te has suscrito al tema de Bienvenido", "OK");
    //}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        opMongo insertar = new();
       // Suscribe();
        //Metodo para ingresar a los profesores
        try
        {

           //insertar.InsertarProfesores("Javier", "Paco");

        }
        catch (MongoException ex)
        {
            await DisplayAlert("error" ,ex.Message,"Ok");
        }

        if (insertar.ConsultarProfesores("Asistencia", "profesores", usuario.Text, password.Text))
        {
            try
            {
                await Navigation.PushAsync(new HomePage());
                usuario.Text = String.Empty; password.Text = String.Empty;
            }
            catch(TargetInvocationException ex)
            {
                var arr = ex.InnerException.Message;
            }
           
        }
        else
        {          
            etiquetaError.Text = "Ha ocurrido un error, el Usuario o el codigo de acceso es incorrecto";
            usuario.Text = String.Empty; password.Text = String.Empty;
        }

    }
}


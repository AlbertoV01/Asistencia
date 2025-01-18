namespace Asistencia;
public partial class Alumnos : ContentPage
{
	public Alumnos()
	{
		InitializeComponent();
    }

    opMongo objMongo = new opMongo();

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (objMongo.AltaAlumnos(studentName.Text, lastName.Text, noControl.Text))
        {
            await DisplayAlert("Bien!", "Se ha registrado correctamente el alumno", "Aceptar");
            studentName.Text = String.Empty;
            lastName.Text = String.Empty;
            noControl.Text = String.Empty;
        }
        else
        {
            await DisplayAlert("Uy!", "Ha ocurrido un error al registrar el alumno", "Aceptar");
            studentName.Text = String.Empty;
            lastName.Text = String.Empty;
            noControl.Text = String.Empty;
        }
    }
}
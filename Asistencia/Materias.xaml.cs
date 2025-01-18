namespace Asistencia;

public partial class Materias : ContentPage
{
	public Materias()
	{
		InitializeComponent();
	}
	opMongo objMongo = new opMongo();

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (objMongo.altaMateria(claveMateria.Text, subjectName.Text, credito.Text))
		{
			await DisplayAlert("Bien!", "Se ha registrado correctamente la materia", "Aceptar");
			claveMateria.Text = String.Empty;
			subjectName.Text = String.Empty;
			credito.Text = String.Empty;
		}
		else
		{
            await DisplayAlert("Uy!", "Ha ocurrido un eror al registrar la materia", "Aceptar");
            claveMateria.Text = String.Empty;
            subjectName.Text = String.Empty;
            credito.Text = String.Empty;
        }
    }
}
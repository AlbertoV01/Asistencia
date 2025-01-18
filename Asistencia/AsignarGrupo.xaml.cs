namespace Asistencia;

public partial class AsignarGrupo : ContentPage
{
	public AsignarGrupo()
	{
		InitializeComponent();
	}
	opMongo objMongo = new opMongo();
    private async void Button_Clicked(object sender, EventArgs e)
    {
		if(await objMongo.AisgnarGrupo(claveMateria.Text,noControl.Text, group.Text))
		{
			await DisplayAlert("Bien!", "Se ha asignado el grupo correctamente", "Acpetar");
			claveMateria.Text=String.Empty;
			noControl.Text=String.Empty;
			group.Text=String.Empty;
        }
		else
		{
            await DisplayAlert("Uy", "Ha ocurrido un error al asignar un grupo", "Acpetar");
            claveMateria.Text = String.Empty;
            noControl.Text = String.Empty;
            group.Text = String.Empty;

        }
    }
}
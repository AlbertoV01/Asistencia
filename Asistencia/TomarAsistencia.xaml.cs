namespace Asistencia;
using AndroidX.Lifecycle;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

public partial class TomarAsistencia : ContentPage
{
    opMongo objMongo = new opMongo();

    public List<ListaDeNombres> _listaAsistencia = new List<ListaDeNombres>();
    public TomarAsistencia()
	{
		InitializeComponent();
        
        _listaAsistencia.Add(new ListaDeNombres { Nombre=""}
            
        );
        collectionView.ItemsSource= _listaAsistencia;
    }


    private  void bton_Clicked(object sender, EventArgs e)
    {
		if (  objMongo.ObtenerListaAsistencia(nombreMateria.Text,grupo.Text).Count==0)
		{
            DisplayAlert("OOPS", "No existe la lista de alumnos", "ACEPTAR");
        }
        else
        {
            listaasistencia.IsEnabled = true;
            collectionView.ItemsSource = null;            
            collectionView.ItemsSource =
           objMongo.ObtenerListaAsistencia(nombreMateria.Text, grupo.Text);

        }
    }
    private async void ListaAsistencia_Clicked(object sender, EventArgs e)
    {
        if (_listaAsistencia.Count != 0)
            _listaAsistencia.Clear();
        // Recorrer todas las filas del ListView
        foreach (object item in collectionView.ItemsSource)
        {
            // Obtener el objeto de datos asociado a la fila
            ListaDeNombres listaDeNombres = (ListaDeNombres)item;

            // Obtener el valor del texto y el valor del checkbox
            string nombre = listaDeNombres.Nombre;
            bool asistencia = listaDeNombres.Asistencia;
            String NombreMateria = listaDeNombres.NombreMateria;
            // Hacer algo con los valores obtenidos
            _listaAsistencia.Add(new ListaDeNombres { Nombre = nombre, Asistencia = asistencia, Fecha=DateTime.Now.ToString(), Grupo=grupo.Text, NombreMateria=nombreMateria.Text });
        }

        String JsonStrign = JsonConvert.SerializeObject(_listaAsistencia);
        var document = BsonSerializer.Deserialize<BsonDocument[]>(JsonStrign);

        opMongo objMongo = new opMongo();
        if(
        objMongo.GuardarLista(document))
        {
            await DisplayAlert("BIEN!", "Se ha guardado la lista con exito!", "ACEPTAR");
            collectionView.ItemsSource = null;           
        }
        else
        {
            await DisplayAlert("UPS!", "HA OCURRIDO UN ERROR AL GUARDAR LA LISTA", "ACEPTAR");
        }
    }

    private void mostrarListaAsistencia_Clicked(object sender, EventArgs e)
    {
        if(objMongo.MostrarListaAsistencia(nombreMateria.Text, grupo.Text, fechaAsistencia.Date).Count!=0)
        {
            collectionView.ItemsSource = (objMongo.MostrarListaAsistencia(nombreMateria.Text, grupo.Text, fechaAsistencia.Date));
            listaasistencia.IsEnabled = false;       
        }
        else
        {
            DisplayAlert("OOPS","Asegurese de introducir el nombre de la materia correctamente\n el grupo correcto y la fecha en la que fue tomada la asistencia\n ","ACEPTAR");
        }
    }
}
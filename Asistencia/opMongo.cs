using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.Runtime.Documents;
using DnsClient.Protocol;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Asistencia
{
    public class opMongo
    {
        public List<String> nombres = new List<string>();


        public Boolean bALLOk = false;
        public String sLastError = "";
        public static MongoClient conexion = new MongoClient("mongodb://192.168.0.134:27017");    

        public Boolean InsertarProfesores(String nombreProfesor, String Password)
        {
           bALLOk = false;    
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var profesores = db.GetCollection<BsonDocument>("profesores");
                var doc = new BsonDocument
                {
                    {"NombreProfesor",nombreProfesor},
                    {"Password", Password },
                };

                profesores.InsertOne(doc);
                bALLOk=true;

            }
            catch (Exception e)
            {
                sLastError = e.Message;
                bALLOk=false;
                Console.WriteLine("ERROR:" + sLastError);
            }
            return bALLOk;

        }

        public Boolean ConsultarProfesores(String databaseName, String collectionName, String nombreProfesor, String Password)
        {
            bALLOk = false;
            try
            {
                var database = conexion.GetDatabase(databaseName);
                var collection = database.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("NombreProfesor", nombreProfesor);
                var filter2 = Builders<BsonDocument>.Filter.Eq("Password", Password);
                var documentoFiltrado = collection.Find(filter).FirstOrDefault().ToString();
                if (documentoFiltrado.Contains(Password))
                    bALLOk = true;
            }
            catch(Exception e)
            {
                sLastError = e.Message;
                bALLOk=false;
            }
            return bALLOk;
        }

        public Boolean altaMateria(String claveDeMateria, String nombreMateria, String Credito)
        {
            Boolean bALLOk = false;
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var ingresarMateria = db.GetCollection<BsonDocument>("Materias");
                var doc = new BsonDocument
                {
                    {"ClaveMateria",claveDeMateria },
                    {"NombreMateria",nombreMateria },
                    {"Credito",Credito }
                };

                ingresarMateria.InsertOne(doc);
                bALLOk = true;
            }catch(Exception e)
            {
                sLastError = e.Message;
                bALLOk = false;
            }

            return bALLOk;
        }

        public Boolean AltaAlumnos(String nombreAlumno, String apellidoAlumno, String noControl)
        {
            Boolean bALLOk = false;
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var registrarAlumnos = db.GetCollection<BsonDocument>("Alumnos");
                var doc = new BsonDocument
                {
                    {"NombreAlumno", nombreAlumno},
                    {"ApellidoAlumno", apellidoAlumno },
                    {"NoControl", noControl },                 
                };

                registrarAlumnos.InsertOne(doc);
                bALLOk = true;
            }
            catch (Exception e)
            {
                sLastError = e.Message;
                bALLOk = false;
            }

            return bALLOk;
        }

        public async Task<Boolean> AisgnarGrupo(String ClaveMateria,String NoControl, String Grupo)
        {
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var registrarAlumnos = db.GetCollection<BsonDocument>("Alumnos");
                var registrarMateras = db.GetCollection<BsonDocument>("Materias");
                var registrarGrupo = db.GetCollection<BsonDocument>("Grupos");
                var filter = Builders<BsonDocument>.Filter.Empty;

                List<String> list = new List<string>();

                var documentos = await registrarAlumnos.Find(filter).ToListAsync();
                var documentos2 = await registrarMateras.Find(filter).ToListAsync();

                String nombreAlumno = String.Empty;
                String claveM = String.Empty;
                String noControl = String.Empty;    

                for(int i=0; i<documentos.Count; i++)
                {
                    noControl = documentos[i].GetValue("NoControl").AsString;

                    if (noControl.Equals(NoControl))
                    {
                        list.Add(noControl);
                        nombreAlumno = documentos[i].GetValue("NombreAlumno").AsString +" "+ documentos[i].GetValue("ApellidoAlumno").AsString;
                        list.Add(nombreAlumno);
                    }
                }

                String nombreMateria = String.Empty;

                for (int i = 0; i < documentos2.Count; i++)
                {
                    claveM = documentos2[i].GetValue("ClaveMateria").AsString;

                    if (claveM.Equals(ClaveMateria))
                    {
                        list.Add(claveM);
                        nombreMateria = documentos2[i].GetValue("NombreMateria").AsString;
                        list.Add (nombreMateria);

                    }
                }

                if(claveM.Equals(ClaveMateria) && noControl.Equals(NoControl))             
                {
                    var doc = new BsonDocument
                    {
                        {"NoControl", list[0]},
                        {"NombreAlumno", list[1]},
                        {"ClaveMateria", list[2]},
                        {"NombreMateria",list[3]},
                        {"Grupo", Grupo }
                    };

                    registrarGrupo.InsertOne(doc);
                    bALLOk = true;
                    list.Clear();
                }
                else
                {
                    bALLOk = false;
                    list.Clear();
                }
                
            }
            catch(Exception e)
            {
                sLastError=e.Message;
                bALLOk = false;
            }

            return bALLOk;
        }

        public ObservableCollection<ListaDeNombres> ObtenerListaAsistencia(String NombreMateria, String Grupo)
        {
            ObservableCollection<ListaDeNombres> listaDeNombres = new ObservableCollection<ListaDeNombres> ();
            if(listaDeNombres.Count !=0)
                listaDeNombres.Clear();
            if(nombres.Count !=0)
                nombres.Clear();

            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var registrarGrupo = db.GetCollection<BsonDocument>("Grupos");
                var filter = Builders<BsonDocument>.Filter.Empty;
                String nombreMateria = String.Empty;
                String grupo = String.Empty;
                var documentos =  registrarGrupo.Find(filter).ToList();

                for (int i = 0; i < documentos.Count; i++)
                {
                    nombreMateria = documentos[i].GetValue("NombreMateria").AsString;
                    grupo = documentos[i].GetValue("Grupo").AsString;
                    
                    if (nombreMateria.Equals(NombreMateria) && grupo.Equals(Grupo))
                    {
                        String nombreAlumno = documentos[i].GetValue("NombreAlumno").AsString;
                        nombres.Add(nombreAlumno);                         
                    }
                }
                //CICLO PARA RECORRER LA LISTA DE LOS ALUMNOS Y ASIGNARSELOS AL LISTVIEW
      
                for(int i = 0;i<nombres.Count; i++)
                {
                    DateTime fechaactual = new DateTime();
                    listaDeNombres.Add(new ListaDeNombres { Nombre = nombres[i],Asistencia=false, Fecha=fechaactual.ToString()});                 
                }

                bALLOk= true;
            }
            catch(Exception e)
            {
                sLastError=e.Message;
            }
            return listaDeNombres;
        }

        public Boolean AltaGrupos(String grupo)
        {
            Boolean bALLOk = false;
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencias");
                var grupoc = db.GetCollection<BsonDocument>("Grupos");
                var doc = new BsonDocument
                {
                    {"Grupo", grupo},
                };

                grupoc.InsertOne(doc);

                bALLOk = true;
            }
            catch (Exception e)
            {
                sLastError = e.Message;
                bALLOk = false;
            }

            return bALLOk;
        }

        public Boolean GuardarLista(BsonDocument[] Lista)
        {
            Boolean bALLOk = false;
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var grupoc = db.GetCollection<BsonDocument>("ListasGuardadas");         
                grupoc.InsertMany(Lista);

                bALLOk = true;
            }
            catch (Exception e)
            {
                sLastError = e.Message;
                bALLOk = false;
            }

            return bALLOk;
        }

        public ObservableCollection<ListaDeNombres> MostrarListaAsistencia(String NombreMateria,String Grupo, DateTime Fecha)
        {
            ObservableCollection<ListaDeNombres> listaDeNombres = new ObservableCollection<ListaDeNombres>();
            if (listaDeNombres.Count != 0)
                listaDeNombres.Clear();
            if (nombres.Count != 0)
                nombres.Clear();
            try
            {
                IMongoDatabase db = conexion.GetDatabase("Asistencia");
                var registrarGrupo = db.GetCollection<BsonDocument>("ListasGuardadas");

                var filter = Builders<BsonDocument>.Filter.Empty;
                bool wow;
                String nombre = String.Empty;
                String grupo = String.Empty;
                String nombreMateria=String.Empty;
                bool asistencia;
                var documentos = registrarGrupo.Find(filter).ToList();

                for (int i = 0; i < documentos.Count(); i++)
                {
                    BsonDocument documento = documentos[i];
                    nombre = documento["Nombre"].AsString;
                    grupo = documento["Grupo"].AsString;
                    asistencia = (bool)documento["Asistencia"];
                    nombreMateria = documento["NombreMateria"].AsString;
                    String fecha1 = Fecha.ToString();
                    String fecha2 = documento["Fecha"].AsString;
                    DateTime dt1 = DateTime.Parse(fecha1);
                    DateTime dt2 = DateTime.Parse(fecha2);
                                
                    if (nombreMateria.Equals(NombreMateria) && grupo.Equals(Grupo) && dt1.Date.Equals(dt2.Date) )
                    {
                        listaDeNombres.Add(new ListaDeNombres { Nombre = nombre, Asistencia = asistencia, NombreMateria=nombreMateria,Grupo=grupo });
                    }
                }

                bALLOk = true;
            }
            catch (Exception e)
            {
                sLastError = e.Message;
            }
            return listaDeNombres;
        }
    }
}
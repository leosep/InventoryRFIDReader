using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using InventoryWebService.Models;
using InventoryWebService.BLL;

namespace InventoryWebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    
    public class Service : System.Web.Services.WebService
    {
        SqlConnection con;
        SqlDataAdapter adp;
        DataSet ds;
        string connStr = ConfigurationManager.ConnectionStrings["DBDATA"].ConnectionString;
        public AuthHeader Authentication;

        // TODO: Check from the database...
        String Username = "myuser";
        String Password = "mypassword";


        public Service()
        {
            //Uncomment the following line if using designed components  
            //InitializeComponent();  
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataTable getDataTable(string str)
        {
            con = new SqlConnection(connStr);
            DataTable dt = new DataTable();

            if(Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                    
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    cmd.Connection = con;
                    cmd.CommandText = str;
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tabla");
                    dt = ds.Tables["tabla"];
            }
            else
            {
                dt = null;
            }            

            return dt;
        }
       
        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int updateRecords(string str)
        {
            int valor = -1;

            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);          
                SqlCommand cmd = new SqlCommand();           
                cmd.Connection = con;
                cmd.CommandText = str;
                cmd.Connection.Open();
                valor = cmd.ExecuteNonQuery();
            }
            else
            {
                valor = -1;
            }     
            
            return valor;
            
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataSet Login(string usuario, string contrasena)
        {
            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT usuarioid, nombre, status from cousuarios where usuarioid = @usuario and password = @contrasena", con))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);                    
                }            
            }
            else
            {
                ds = null;
            }     
            
            return ds;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataSet getGrupos()
        {
            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);

                try
                {
                    con.Open();
                }
                catch (Exception e)
                {
                    con.Close();
                }

                //Grupos
                using (SqlCommand cmd = new SqlCommand("SELECT grupo_activo, descrip FROM afgrupos", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    ds = new DataSet();                    
                    da.Fill(ds);
                }
            }
            else
            {
                ds = null;
            }

            return ds;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataSet getActivo(int codigo)
        {
            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT CONCAT(cia,'_',activo) as cia_activo, cia, activo, cod_alterno, descrip, grupo_activo from afactivos where activo = @codigo", con))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            else
            {
                ds = null;
            }

            return ds;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataSet getActivoList(string[] codigos)
        {
            int cantidad = codigos.Length;
            string parametros = "";

            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                if (codigos.Length > 0)
                {
                    con = new SqlConnection(connStr);
                    con.Open();

                    for (int i = 0; i < cantidad; i++)
                    {
                        parametros = parametros + "@codigo" + i;
                        if (i < cantidad - 1) parametros = parametros + ",";
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT CONCAT(cia,'_',activo) as cia_activo, cia, activo, cod_alterno, descrip, grupo_activo from afactivos where activo IN (" + parametros + ")", con))
                    {
                        for (int i = 0; i < cantidad; i++)
                        {
                            cmd.Parameters.AddWithValue("@codigo" + i, Convert.ToInt32(Utils.hexToASCII(codigos[i])));
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        ds = new DataSet();
                        da.Fill(ds);
                    }
                }
                else 
                {                   
                    ds = new DataSet();
                    ds.Tables.Add("afactivos");
                }
            }
            else
            {
                ds = null;
            }

            return ds;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public DataSet getActivoDetalle(int cia, int activo)
        {
            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                ds = new DataSet();

                ds.Tables.Add("afactivos");
                ds.Tables.Add("aftipoact");
                ds.Tables.Add("afgrupos");
                
                //Detalle del activo
                using (SqlCommand cmd = new SqlCommand("SELECT cia, activo, cod_alterno, descrip, tipo_activo, grupo_activo, serie from afactivos where activo = @activo AND cia = @cia" , con))
                {
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@cia", cia);
                    da.SelectCommand = cmd;                  
                    da.Fill(ds, "afactivos");
                }
                //Tipos
                using (SqlCommand cmd = new SqlCommand("SELECT tipo_activo, descrip FROM aftipoact", con))
                {                    
                    da.SelectCommand = cmd;
                    da.Fill(ds, "aftipoact");
                }
                //Grupos
                using (SqlCommand cmd = new SqlCommand("SELECT grupo_activo, descrip FROM afgrupos", con))
                {
                    da.SelectCommand = cmd;
                    da.Fill(ds, "afgrupos");
                }
            }
            else
            {
                ds = null;
            }

            return ds;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int updateActivo(int cia, int activo, string cod_alterno, string descrip, int tipo_activo, int grupo_activo, string serie)
        {
            int valor = -1;

            if (Authentication.Username == Username &&
                Authentication.Password == Password)
            {
                con = new SqlConnection(connStr);
                con.Open();

                using (SqlCommand cmd = new SqlCommand("UPDATE afactivos SET cod_alterno=@cod_alterno, descrip=@descrip, tipo_activo=@tipo_activo, grupo_activo=@grupo_activo, serie=@serie WHERE activo = @activo AND cia = @cia", con))
                {
                    cmd.Parameters.AddWithValue("@cia", cia);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@cod_alterno", cod_alterno);
                    cmd.Parameters.AddWithValue("@descrip", descrip);
                    cmd.Parameters.AddWithValue("@tipo_activo", tipo_activo);
                    cmd.Parameters.AddWithValue("@grupo_activo", grupo_activo);
                    cmd.Parameters.AddWithValue("@serie", serie);   
                    
                    valor = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                valor = -1;
            }

            return valor;
        }      
    }
}
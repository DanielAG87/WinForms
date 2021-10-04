using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsContacts
{
    class DataAccessLayer
    {
        private SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PruebaVideo;Data Source=DESKTOP-NI3B7E0\\SQLEXPRESS");

        public void InsertContacto(Contacto contacto)
        {
            // define return value - newly inserted ID
            // int returnValue = -1;

            try
            {
                conn.Open();
                string query = @"
                    INSERT INTO Contactos (Nombre, Apellidos, Telefono, Direccion)
                    VALUES (@Nombre, @Apellidos, @Telefono, @Direccion);
                ";

                SqlParameter nombre = new SqlParameter();
                nombre.ParameterName = "@Nombre";
                nombre.Value = contacto.Nombre;
                nombre.DbType = System.Data.DbType.String;

                SqlParameter apellidos = new SqlParameter("@Apellidos", contacto.Apellidos);
                SqlParameter telefono = new SqlParameter("@Telefono", contacto.Telefono);
                SqlParameter direccion = new SqlParameter("@Direccion", contacto.Direccion);

                SqlCommand sqlCommand = new SqlCommand(query, conn);
                
                sqlCommand.Parameters.Add(nombre);
                sqlCommand.Parameters.Add(apellidos);
                sqlCommand.Parameters.Add(telefono);
                sqlCommand.Parameters.Add(direccion);

                sqlCommand.ExecuteNonQuery();
                /*
                object returnObj = sqlCommand.ExecuteScalar();
                if (returnObj != null)
                {
                    int.TryParse(returnObj.ToString(), out returnValue);
                }
                */
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            //return returnValue;
        }


        public void UpdateContacto(Contacto contacto)
        {
            try
            {
                conn.Open();
                string query = @"
                    UPDATE Contactos SET
                        Nombre = @Nombre,
                        Apellidos = @Apellidos, 
                        Telefono = @Telefono, 
                        Direccion = @Direccion
                    WHERE id = @Id
                ";

                SqlParameter id = new SqlParameter("@Id", contacto.Id);
                SqlParameter nombre = new SqlParameter("@Nombre", contacto.Nombre);
                SqlParameter apellidos = new SqlParameter("@Apellidos", contacto.Apellidos);
                SqlParameter telefono = new SqlParameter("@Telefono", contacto.Telefono);
                SqlParameter direccion = new SqlParameter("@Direccion", contacto.Direccion);

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(id);
                command.Parameters.Add(nombre);
                command.Parameters.Add(apellidos);
                command.Parameters.Add(telefono);
                command.Parameters.Add(direccion);

                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
       
        public List<Contacto> GetContactos(string searchString = null)
        {
            List<Contacto> contactos = new List<Contacto>();

            try
            {
                conn.Open();
                string query = @"
                    SELECT id, Nombre, Apellidos, Telefono, Direccion
                    FROM Contactos
                ";

                SqlCommand sqlCommand = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(searchString))
                {
                    query += @"
                        WHERE Nombre LIKE @searchString
                        OR Apellidos LIKE @searchString
                        OR Telefono LIKE @searchString
                        OR Direccion LIKE @searchString
                    ";

                    sqlCommand.Parameters.Add(new SqlParameter("@SearchString", $"%{searchString}%"));
                }

                sqlCommand.CommandText = query;
                sqlCommand.Connection = conn;

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    contactos.Add(new Contacto
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString()
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return contactos;
        }

        public void DeleteContacto(int id)
        {
            try
            {
                conn.Open();
                string query = @"DELETE FROM Contactos WHERE id = @Id";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Id", id));
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

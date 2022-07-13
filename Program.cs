using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySql_Console_2022
{
    internal class Program
    {
        public static class AcessoAoAceess
        {
            public static MySqlConnection AbrirConexao()
            {
                return new MySqlConnection("server = 127.0.0.1;database = banco2022;uid=root;pwd=;port=3306");
            }

            public static void Mostra_Dados()
            {
                MySqlConnection conexao = AbrirConexao();
                Cliente cliente = new Cliente();

                try
                {
                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = "Select * from Clientes";
                    MySqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Valores retornados:");
                        Console.WriteLine("Id\tNome\t\t\t\tE-mail");
                        while (reader.Read())
                        {
                            //Console.WriteLine("{0}\t{1}\t\t{2}", reader["Id"].ToString(), reader["nome"].ToString(), reader["email"].ToString()); 
                            //cliente.Id = int.Parse(reader["id"].ToString());
                            //cliente.Nome = reader["nome"].ToString();
                            //cliente.Email = reader["email"].ToString();
                            cliente.PreencheCliente(reader);
                            cliente.MostraCliente();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro retornado!");
                    }

                    conexao.Close();
                }
                catch (MySqlException err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            public static void Procura_Nome(string qualnome)
            {
                MySqlConnection conexao = AbrirConexao();
                Cliente cliente = new Cliente();

                try
                {
                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = "Select * from Clientes where nome like '%" + qualnome + "%'";
                    MySqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Valores retornados:");
                        Console.WriteLine("Id\tNome\t\t\t\tE-mail");
                        while (reader.Read())
                        {
                            //Console.WriteLine("{0}\t{1}\t\t{2}", reader["Id"].ToString(), reader["nome"].ToString(), reader["email"].ToString()); 
                            //cliente.Id = int.Parse(reader["id"].ToString());
                            //cliente.Nome = reader["nome"].ToString();
                            //cliente.Email = reader["email"].ToString();
                            cliente.PreencheCliente(reader);
                            cliente.MostraCliente();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro retornado!");
                    }

                    conexao.Close();
                }
                catch (MySqlException err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            public static void Apaga_Registros(string quaisregistros)
            {
                MySqlConnection conexao = AbrirConexao();
                Cliente cliente = new Cliente();

                try
                {
                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = "Select * from Clientes where nome like '%" + quaisregistros + "%'";
                    MySqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string resp = "N";
                        Console.WriteLine("Valores retornados:");
                        Console.WriteLine("Id\tNome\t\t\t\tE-mail");
                        while (reader.Read())
                        {
                            cliente.PreencheCliente(reader);
                            cliente.MostraCliente();
                        }
                        Console.WriteLine("\nQuer apagar esse(s) registro(s) (S/N): ");
                        resp = Console.ReadLine().ToUpper();
                        if (resp == "S")
                        {
                            reader.Close();
                            comando.CommandText = "Delete from Clientes where nome like '%" + quaisregistros + "%'";
                            comando.ExecuteNonQuery();
                            Console.WriteLine("Registro(s) apagado(s)");
                        }
                        else
                            Console.WriteLine("Registro(s) não apagado(s)");
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro retornado!");
                    }

                    conexao.Close();
                }
                catch (MySqlException err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            public static void Altera_registro(string qualregistro)
            {
                MySqlConnection cnn = AcessoAoAceess.AbrirConexao();
                try
                {
                    cnn.Open();
                    Cliente alterar = new Cliente();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandText = "Select * from clientes where nome like '%" + qualregistro + "%'";
                    MySqlDataReader resultado = cmd.ExecuteReader();
                    if (resultado.HasRows)
                    {
                        while (resultado.Read())
                        {
                            string resp;
                            Console.WriteLine("Nome: {0}", resultado["nome"].ToString());
                            Console.WriteLine("E-mail: {0}", resultado["email"].ToString());
                            Console.Write("Deseja alterar este registro? (S/N): ");
                            resp = Console.ReadLine();
                            if (resp == "S" || resp == "s")
                            {
                                alterar.PreencheCliente(resultado);
                                alterar.AlteraCliente();
                                MySqlCommand alteracao = new MySqlCommand();
                                alteracao.Connection = cnn;
                                alteracao.CommandText = "Update clientes set nome = '" + alterar.Nome + "',email = '" + alterar.Email + "' where id = " + alterar.Id;
                                alteracao.ExecuteNonQuery();
                                Console.WriteLine("Este registro foi alterado");
                            }
                        }
                        cnn.Close();
                    }
                    else
                    {
                        Console.WriteLine("Nenhum registro encontrado com o nome {0}", qualregistro);
                    }
                }
                catch (MySqlException err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            public static void InserirCliente(Cliente novo)
            {
                MySqlConnection conexao = AcessoAoAceess.AbrirConexao();
                try
                {
                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = "Insert into Clientes (nome, email) values ('" + novo.Nome + "','" + novo.Email + "')";
                    comando.ExecuteNonQuery();
                    conexao.Close();
                }
                catch (MySqlException err)
                {
                    Console.WriteLine(err.Message);
                }
            }

        }

        public class Cliente
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }

            public void NovoCliente()
            {
                Console.Write("Digite o nome: ");
                this.Nome = Console.ReadLine();
                Console.Write("Digite o e-mail: ");
                this.Email = Console.ReadLine();
                this.Id = 0;
            }

            public void AlteraCliente()
            {
                string entrada;
                Console.WriteLine("Nome: {0}", this.Nome);
                Console.Write("Novo nome: ");
                entrada = Console.ReadLine();
                if (entrada != "")
                    this.Nome = entrada;
                Console.WriteLine("E-mail: {0}", this.Email);
                Console.Write("Novo e-mail: ");
                entrada = Console.ReadLine();
                if (entrada != "")
                    this.Email = entrada;
            }

            public void PreencheCliente(MySqlDataReader registro)
            {
                this.Id = int.Parse(registro["Id"].ToString());
                this.Nome = registro["nome"].ToString();
                this.Email = registro["email"].ToString();

            }

            public void MostraCliente()
            {
                //Console.WriteLine("Id\tNome\t\t\tE-mail");
                Console.WriteLine("{0}\t{1}\t\t\t{2}", this.Id, this.Nome, this.Email);
            }
        }

        static void Main(string[] args)
        {
            Cliente meucliente = new Cliente();
            string op;
            do
            {
                Console.Clear();
                Console.WriteLine("Cadastro de clientes");
                Console.WriteLine("\nMenu de Opções\n\t1 - Mostrar clientes no banco");
                Console.WriteLine("\t2 - Inserir novo cliente no banco");
                Console.WriteLine("\t3 - Procurar por nome de cliente no banco");
                Console.WriteLine("\t4 - Apagar cliente no banco");
                Console.WriteLine("\t5 - Alterar cliente no banco");
                Console.Write("\n\t9 - Sair\n\nEscolha uma opção: ");
                op = Console.ReadLine();
                switch (op)
                {
                    case "1":
                        AcessoAoAceess.Mostra_Dados();
                        break;
                    case "2":
                        meucliente.NovoCliente();
                        AcessoAoAceess.InserirCliente(meucliente);
                        break;
                    case "3":
                        string nome;
                        Console.Write("Qual nome a ser procurado: ");
                        nome = Console.ReadLine();
                        AcessoAoAceess.Procura_Nome(nome);
                        break;
                    case "4":
                        string reg;
                        Console.Write("Qual nome a ser procurado: ");
                        reg = Console.ReadLine();
                        AcessoAoAceess.Apaga_Registros(reg);
                        break;
                    case "5":
                        string altera;
                        Console.Write("Qual nome a ser procurado: ");
                        altera = Console.ReadLine();
                        AcessoAoAceess.Altera_registro(altera);
                        break;
                    case "9":
                        Console.WriteLine("Até mais");
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
                Console.Write("Pressione qualquer tecla para continuar. . .");
                Console.ReadKey();
            } while (op != "9");

        }
    }
}

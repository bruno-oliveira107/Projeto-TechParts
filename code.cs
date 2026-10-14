using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ConsoleApp1
{
    struct Produto
    {
        public string Nome;
        public string Categoria;
        public int Quantidade;
        public decimal Preco;

        public override string ToString()
        {
            return "Nome: " + Nome +
                   " | Categoria: " + Categoria +
                   " | Estoque: " + Quantidade +
                   " | Preço: " + Preco;
        }
    }

    struct Venda
    {
        public string NomeProduto;
        public string Categoria;
        public int Quantidade;
        public decimal PrecoUnitario;
        public decimal Total;
        public string Data;

        public override string ToString()
        {
            return Data + " | " + NomeProduto +
                   " | Categoria: " + Categoria +
                   " | Quantidade: " + Quantidade +
                   " | Total: " + Total;
        }
    }

    internal class Program
    {
        static List<Produto> produtos = new List<Produto>();
        static List<Venda> vendas = new List<Venda>();

        static string arquivoProdutos = "produtos.txt";
        static string arquivoVendas = "vendas.txt";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            CarregarProdutos();
            CarregarVendas();

            string opcao = "";

            do
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("      ______________________________");
                Console.WriteLine("     /                              \\");
                Console.WriteLine("    |  =============================|");
                Console.WriteLine("    |                               |");
                Console.WriteLine("    |         TECHPARTS STORE       |");
                Console.WriteLine("    |   Loja de Pecas de Computador |");
                Console.WriteLine("    |                               |");
                Console.WriteLine("    |  =============================|");
                Console.WriteLine("     \\_____________________________/");
                Console.WriteLine("                  |||||");
                Console.WriteLine("                  |||||");
                Console.WriteLine("               ___|||||___");
                Console.WriteLine("              /___________\\");
                Console.WriteLine("");

                Console.ResetColor();

                Console.WriteLine("1 - Cadastrar produto");
                Console.WriteLine("2 - Listar estoque");
                Console.WriteLine("3 - Consultar produto");
                Console.WriteLine("4 - Excluir produto");
                Console.WriteLine("5 - Realizar venda");
                Console.WriteLine("6 - Relatorio de vendas");
                Console.WriteLine("0000 - Sair");

                Console.Write("\nEscolha uma opcao: ");
                opcao = Console.ReadLine();

                if (opcao == null)
                    opcao = "";

                switch (opcao)
                {
                    case "1":
                        CadastrarProduto();
                        break;

                    case "2":
                        ListarEstoque();
                        break;

                    case "3":
                        ConsultarProduto();
                        break;

                    case "4":
                        ExcluirProduto();
                        break;

                    case "5":
                        RealizarVenda();
                        break;

                    case "6":
                        RelatorioVendas();
                        break;

                    case "0000":
                        Console.WriteLine("Saindo do sistema...");
                        break;

                    default:
                        Erro("Opcao invalida.");
                        break;
                }

                if (opcao != "0000")
                    Pausar();

            } while (opcao != "0000");
        }

        // CADASTRO DE PRODUTOS

        static void CadastrarProduto()
        {
            Console.Clear();
            Titulo("CADASTRO DE PRODUTO");

            Produto novo = new Produto();

            novo.Nome = LerTexto("Nome da peca: ");
            novo.Categoria = EscolherCategoria();

            novo.Quantidade = LerInteiro(
                "Quantidade inicial: ", 0);

            novo.Preco = LerDecimal(
                "Preco unitario (R$): ", 0);

            int indice = EncontrarProduto(novo.Nome);

            // Se o produto ja existir, avisa e encerra o cadastro.
            if (indice >= 0)
            {
                Erro("Produto ja existe.");
                return;
            }

            produtos.Add(novo);
            SalvarProdutos();

            Sucesso("Produto cadastrado com sucesso!");
        }

        // ESCOLHER CATEGORIA

        static string EscolherCategoria()
        {
            Console.WriteLine("\nCategorias:");
            Console.WriteLine("1 - Processador");
            Console.WriteLine("2 - Placa de video");
            Console.WriteLine("3 - Memoria RAM");
            Console.WriteLine("4 - SSD/HD");
            Console.WriteLine("5 - Placa-mae");
            Console.WriteLine("6 - Fonte");
            Console.WriteLine("7 - Periferico");
            Console.WriteLine("8 - Outro");

            int opcao = LerInteiro("Categoria: ", 1, 8);

            switch (opcao)
            {
                case 1:
                    return "Processador";

                case 2:
                    return "Placa de video";

                case 3:
                    return "Memoria RAM";

                case 4:
                    return "SSD/HD";

                case 5:
                    return "Placa-mae";

                case 6:
                    return "Fonte";

                case 7:
                    return "Periferico";

                default:
                    return "Outro";
            }
        }

        // LISTAR ESTOQUE

        static void ListarEstoque()
        {
            Console.Clear();
            Titulo("ESTOQUE DE PRODUTOS");

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            for (int i = 0; i < produtos.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + produtos[i]); // vai percorrer um por um dos produtos em estoque e somar 1//
            }

            Console.WriteLine(
                "\nTotal de produtos cadastrados: " + produtos.Count); //vai msotrar cada produto//
        }

        // CONSULTAR PRODUTO

        static void ConsultarProduto()
        {
            Console.Clear();
            Titulo("CONSULTAR PRODUTO");

            string nome = LerTexto(
                "Digite o nome ou parte do nome: ");

            bool encontrou = false;

            for (int i = 0; i < produtos.Count; i++) //percorre a lista de produtos//
            {
                if (produtos[i].Nome.IndexOf(
                    nome, StringComparison.OrdinalIgnoreCase) >= 0) //Verifica e compara se o nome digitado bate//
                {
                    Console.WriteLine(produtos[i]); // mostra o produto//
                    encontrou = true;
                }
            }

            if (!encontrou)
                Console.WriteLine("Produto nao encontrado.");
        }

        // EXCLUIR PRODUTO

        static void ExcluirProduto()
        {
            Console.Clear();
            Titulo("EXCLUIR PRODUTO");

            string nome = LerTexto("Nome exato do produto: ");

            int indice = EncontrarProduto(nome);

            if (indice == -1)
            {
                Erro("Produto nao encontrado.");
                return;
            }

            Console.WriteLine(produtos[indice]); // mostra o produto com base no indice que ele se encontra//

            Console.Write("Confirma a exclusao? (S/N): ");

            string resposta = Console.ReadLine();

            if (resposta != null)
                resposta = resposta.Trim().ToUpper(); // Se a resposta nao estiver vazia passa tudo para maiscul 

            if (resposta == "S")
            {
                produtos.RemoveAt(indice); //Chama a unção apra remover o produto//

                SalvarProdutos();

                Sucesso("Produto excluido com sucesso.");
            }
            else
            {
                Console.WriteLine("Exclusao cancelada.");
            }
        }

        // REALIZAR VENDA

        static void RealizarVenda()
        {
            Console.Clear();
            Titulo("REALIZAR VENDA");

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nao existem produtos cadastrados.");
                return;
            }

            string nome = LerTexto("Nome exato do produto: ");

            int indice = EncontrarProduto(nome);  //Busca o produto com a função

            if (indice == -1)
            {
                Erro("Produto nao encontrado.");
                return;
            }

            Produto produto = produtos[indice];

            Console.WriteLine("\nProduto: " + produto.Nome); // Mostra o nome do produto
            Console.WriteLine("Estoque: " + produto.Quantidade); //Mostra a quantidade 
            Console.WriteLine("Preco: " + produto.Preco.ToString("C")); // Mostra o preço

            int quantidade = LerInteiro(
                "Quantidade a vender: ", 1);

            if (quantidade > produto.Quantidade)   // Se a quantidade de venda for maior que a quantidade que tem vai dar errro
            {
                Erro("Estoque insuficiente. Venda cancelada.");
                return;
            }

            decimal total = produto.Preco * quantidade;    //preço * quantidade para mostrar o total

            Console.WriteLine("Total da venda: " + total.ToString("C"));

            Console.Write("Confirmar venda? (S/N): ");

            string resposta = Console.ReadLine();

            if (resposta != null)
                resposta = resposta.Trim().ToUpper(); // passa a resposta para maisculo

            if (resposta != "S")
            {
                Console.WriteLine("Venda cancelada."); // aparece venda cancelada
                return;
            }

            //Bruno !!!!!!!!!

            // Atualiza o estoque.

            produto.Quantidade -= quantidade;
            produtos[indice] = produto;

            // Cria o registro da venda.

            Venda novaVenda = new Venda();

            novaVenda.NomeProduto = produto.Nome;
            novaVenda.Categoria = produto.Categoria;
            novaVenda.Quantidade = quantidade;
            novaVenda.PrecoUnitario = produto.Preco;
            novaVenda.Total = total;
            novaVenda.Data = DateTime.Now.ToString(
                "dd/MM/yyyy HH:mm");

            vendas.Add(novaVenda);

            // Salva os dados.

            SalvarProdutos();
            SalvarVendas();

            Sucesso("Venda realizada com sucesso!");
        }

        // RELATORIO DE VENDAS

        static void RelatorioVendas()
        {
            Console.Clear();
            Titulo("RELATORIO DE VENDAS");

            if (vendas.Count == 0)
            {
                Console.WriteLine("Nenhuma venda realizada.");
                return;
            }

            decimal totalGeral = 0;
            List<string> categorias = new List<string>();

            Console.WriteLine("VENDAS REGISTRADAS:");

            for (int i = 0; i < vendas.Count; i++)
            {
                Console.WriteLine(vendas[i]);

                totalGeral += vendas[i].Total;

                if (!categorias.Contains(vendas[i].Categoria))
                {
                    categorias.Add(vendas[i].Categoria);
                }
            }

            Console.WriteLine("\nTOTAL POR CATEGORIA:");

            for (int i = 0; i < categorias.Count; i++)
            {
                decimal totalCategoria = 0;

                for (int j = 0; j < vendas.Count; j++)
                {
                    if (vendas[j].Categoria == categorias[i])
                    {
                        totalCategoria += vendas[j].Total;
                    }
                }

                Console.WriteLine(
                    categorias[i] + ": " +
                    totalCategoria.ToString("C"));
            }

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(
                "\nTOTAL GERAL: " + totalGeral.ToString("C"));

            Console.ResetColor();
        }

        // LOCALIZAR PRODUTO

        static int EncontrarProduto(string nome)
        {
            for (int i = 0; i < produtos.Count; i++)
            {
                if (string.Equals(
                    produtos[i].Nome,
                    nome,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        // SALVAR PRODUTOS NO ARQUIVO

        static void SalvarProdutos()
        {
            using (StreamWriter escritor =
                new StreamWriter(arquivoProdutos, false))
            {
                for (int i = 0; i < produtos.Count; i++)
                {
                    Produto p = produtos[i];

                    escritor.WriteLine(
                        LimparCampo(p.Nome) + "|" +
                        LimparCampo(p.Categoria) + "|" +
                        p.Quantidade + "|" +
                        p.Preco.ToString(
                            CultureInfo.InvariantCulture));
                }
            }
        }

        // CARREGAR PRODUTOS DO ARQUIVO

        static void CarregarProdutos()
        {
            if (!File.Exists(arquivoProdutos))
                return;

            string[] linhas = File.ReadAllLines(arquivoProdutos);

            for (int i = 0; i < linhas.Length; i++)
            {
                string[] partes = linhas[i].Split('|');

                int quantidade;
                decimal preco;

                if (partes.Length == 4 &&
                    int.TryParse(partes, out quantidade) &&
                    decimal.TryParse(
                        partes,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out preco))
                {
                    Produto p = new Produto();

                    p.Nome = partes;
                    p.Categoria = partes;
                    p.Quantidade = quantidade;
                    p.Preco = preco;

                    produtos.Add(p);
                }
            }
        }

        // SALVAR VENDAS NO ARQUIVO

        static void SalvarVendas()
        {
            using (StreamWriter escritor =
                new StreamWriter(arquivoVendas, false))
            {
                for (int i = 0; i < vendas.Count; i++)
                {
                    Venda v = vendas[i];

                    escritor.WriteLine(
                        LimparCampo(v.NomeProduto) + "|" +
                        LimparCampo(v.Categoria) + "|" +
                        v.Quantidade + "|" +
                        v.PrecoUnitario.ToString(
                            CultureInfo.InvariantCulture) + "|" +
                        v.Total.ToString(
                            CultureInfo.InvariantCulture) + "|" +
                        v.Data);
                }
            }
        }

        // CARREGAR VENDAS DO ARQUIVO

        static void CarregarVendas()
        {
            if (!File.Exists(arquivoVendas))
                return;

            string[] linhas = File.ReadAllLines(arquivoVendas);

            for (int i = 0; i < linhas.Length; i++)
            {
                string[] partes = linhas[i].Split('|');

                int quantidade;
                decimal preco;
                decimal total;

                if (partes.Length == 6 &&
                    int.TryParse(partes, out quantidade) &&
                    decimal.TryParse(
                        partes,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out preco) &&
                    decimal.TryParse(
                        partes,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out total))
                {
                    Venda v = new Venda();

                    v.NomeProduto = partes;
                    v.Categoria = partes;
                    v.Quantidade = quantidade;
                    v.PrecoUnitario = preco;
                    v.Total = total;
                    v.Data = partes;

                    vendas.Add(v);
                }
            }
        }

        // EVITA PROBLEMAS COM O SEPARADOR DO ARQUIVO

        static string LimparCampo(string texto)
        {
            return texto.Replace("|", "/")
                        .Replace("\r", " ")
                        .Replace("\n", " ");
        }

        // LER TEXTO

        static string LerTexto(string mensagem)
        {
            string texto = "";

            do
            {
                Console.Write(mensagem);
                texto = Console.ReadLine();

                if (texto == null)
                    texto = "";

                texto = texto.Trim();

                if (texto == "")
                    Erro("O campo nao pode ficar vazio.");

            } while (texto == "");

            return texto;
        }

        // LER NUMEROS INTEIROS

        static int LerInteiro(
            string mensagem,
            int minimo,
            int maximo = int.MaxValue)
        {
            int valor;

            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out valor) &&
                    valor >= minimo &&
                    valor <= maximo)
                {
                    return valor;
                }

                Erro("Digite um numero inteiro valido.");
            }
        }

        // LER PRECOS

        static decimal LerDecimal(
            string mensagem,
            decimal minimo)
        {
            decimal valor;

            while (true)
            {
                Console.Write(mensagem);

                string entrada = Console.ReadLine();

                if (entrada == null)
                    entrada = "";

                entrada = entrada.Trim().Replace(',', '.');

                if (decimal.TryParse(
                    entrada,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out valor) &&
                    valor >= minimo)
                {
                    return valor;
                }

                Erro("Digite um preco valido.");
            }
        }

        // TITULO

        static void Titulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("====================================");
            Console.WriteLine(" " + texto);
            Console.WriteLine("====================================");

            Console.ResetColor();
        }

        // MENSAGEM DE ERRO

        static void Erro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        // MENSAGEM DE SUCESSO

        static void Sucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        // PAUSAR O SISTEMA

        static void Pausar()
        {
            Console.WriteLine(
                "\nPressione ENTER para continuar...");

            Console.ReadLine();
        }
    }
}

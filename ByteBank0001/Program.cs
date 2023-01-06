namespace byteBank0001
{
    class Program
    {
        static void showMenu()
        {
            Console.WriteLine(" -------------------------------");
            Console.WriteLine(" 1 - Detalhes da Conta.");
            Console.WriteLine(" 2 - Saldo.");
            Console.WriteLine(" 3 - Manipular Conta. ");
            Console.WriteLine(" 4 - Transações Bancárias.");
            Console.WriteLine(" 5 - Deletar Conta");
            Console.WriteLine(" 6 - Histórico");
            Console.WriteLine(" 0 - Logout");
            Console.WriteLine();
            Console.Write(" Digite Uma Opção: ");
        }
        static int login(List<string> cpfs, List<string> senhas)
        {
            Console.Write("Informe o CPF: ");
            string cpf_user = Console.ReadLine();
            Console.Write("Informe a Senha: ");
            string password_user = Console.ReadLine();

            if (cpfs.Contains(cpf_user) && senhas.Contains(password_user))
                return cpfs.IndexOf(cpf_user);
            return -1;
        }
        static int createAccount(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos, List<string> historico)
        {
            Console.Write("Informe o CPF: ");
            string cpf = Console.ReadLine();
            if (!cpfs.Contains(cpf))
            {
                cpfs.Add(cpf);
                Console.WriteLine();
                Console.Write("Informe o Nome do Titular: ");
                titulares.Add(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Cadastre Uma Senha.: ");
                senhas.Add(Console.ReadLine());

                historico.Add("");
                saldos.Add(0);

                Console.Clear();
                Console.WriteLine("\nUsuário Cadastrado Com Sucesso!!\n");
                return cpfs.IndexOf(cpf);
            }
            Console.Clear();
            Console.WriteLine("\nEste CPF Já Está Registrado!\n");
            return -1;
        }
        static void detailUser(List<string> cpfs, List<string> titulares, List<double> saldos, int indexCPFLogado)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Nome do Titular: {titulares[indexCPFLogado]}");
            Console.WriteLine($"CPF do Titular: {cpfs[indexCPFLogado]}");
            Console.WriteLine($"Saldo em Conta: {saldos[indexCPFLogado]}");

            Console.Write("\nPressione Enter Para Voltar ao Menu...");
            Console.ReadLine();
            Console.Clear();
        }
        static void showBalance(List<double> saldos, int indexCPFLogado)
        {
            Console.WriteLine($"Saldo: {saldos[indexCPFLogado]}");
        }
        static void manipulateAccount(List<string> cpfs, List<string> titulares, List<double> saldos, int indexCpfLogado, List<string> historico)
        {
            int option;
            do
            {
                Console.WriteLine();

                Console.WriteLine(" 1 - Sacar.");
                Console.WriteLine(" 2 - Depositar.");
                Console.WriteLine(" 3 - Transferir.");
                Console.WriteLine(" 0 - Sair.");
                Console.Write("\n Informe uma Opção: ");

                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Clear();

                        Console.Write("Informe o Valor Do Saque: ");
                        double valorSaque = double.Parse(Console.ReadLine());

                        if (sacar(saldos, indexCpfLogado, valorSaque, historico))
                            Console.WriteLine("\nSaque Efetuado Com Sucesso!");
                        else
                            Console.WriteLine("\nSaldo Insuficiente!");

                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Informe o Valor do Depósito: ");
                        double valorDeposito = double.Parse(Console.ReadLine());

                        depositar(saldos, indexCpfLogado, valorDeposito, historico);
                        Console.WriteLine("\nDepósito Efetuado Com Sucesso!");

                        break;
                    case 3:
                        Console.Clear();
                        transferencia(saldos, titulares, cpfs, indexCpfLogado, historico);
                        break;
                }
            } while (option != 0);
            Console.Clear();
        }
        static bool sacar(List<double> saldos, int cpfIndex, double valorSaque, List<string> historico)
        {
            if (valorSaque <= saldos[cpfIndex])
            {
                saldos[cpfIndex] -= valorSaque;
                cadastrarTransacao($"Saque de Valor R$ R$ {valorSaque:N2} Efetuado Com Sucesso\n", historico, cpfIndex);
                return true;
            }
            cadastrarTransacao($"Saque de Valor R$ {valorSaque:N2} Cancelado. Saldo Insuficiente!\n", historico, cpfIndex);
            return false;
        }
        static bool depositar(List<double> saldos, int cpfIndex, double valorDeposito, List<string> historico)
        {
            saldos[cpfIndex] += valorDeposito;
            cadastrarTransacao($"Depósito de Valor R$ {valorDeposito:N2} Efetuado Com Sucesso!\n", historico, cpfIndex);
            return true;
        }
        static void transferencia(List<double> saldos, List<string> titulares, List<string> cpfs, int indexCpfOrigem, List<string> historico)
        {
            Console.Write("Informe o Valor Para Transferência: ");
            double valorTransferencia = double.Parse(Console.ReadLine());

            Console.Write("Qual o CPF Do destinatário: ");
            string cpfDestino = Console.ReadLine();

            int indexCpfDestino = cpfs.IndexOf(cpfDestino);

            if (indexCpfDestino != -1)
            {
                if (valorTransferencia <= saldos[indexCpfOrigem])
                {
                    saldos[indexCpfOrigem] -= valorTransferencia;
                    cadastrarTransacao($"Transferência Para o Titular {titulares[indexCpfOrigem]} de Valor R$ {valorTransferencia:N2} Efetuado Com Sucesso\n", historico, indexCpfOrigem);

                    saldos[indexCpfDestino] += valorTransferencia;
                    cadastrarTransacao($"Transferência de Valor R$ R$ {valorTransferencia:N2} Recebido do Titular: {titulares[indexCpfDestino]}\n", historico, indexCpfDestino);
                }
                else
                {
                    cadastrarTransacao($"Erro na Transferência, o Valor R$ {valorTransferencia:N2} Excede o Valor Do Limite!\n", historico, indexCpfOrigem);
                }
            }
            else
            {
                Console.WriteLine("\nCPF de Destino Não Encontrado!!");
            }
        }
        static void updateAccount(List<string> cpfs, List<string> titulares, List<string> senhas, int indexCpfLogado, List<string> historico)
        {
            int option;
            do
            {
                Console.WriteLine(" 1 - Alterar Nome\n 2 - Alterar Senha\n 0 - Voltar ao Menu Principal");
                Console.Write("\n Informe uma das opções acima: ");

                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        updateName(titulares, indexCpfLogado, historico);
                        break;
                    case 2:
                        updatePassword(senhas, indexCpfLogado, historico);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opção Incorreta!\n");
                        break;
                }
            } while (option != 0);

            Console.Clear();
        }
        static void updateName(List<string> titulares, int cpfIndex, List<string> historico)
        {
            Console.Clear();
            Console.Write("Informe o Novo Nome: ");
            string newHolder = Console.ReadLine();

            titulares[cpfIndex] = newHolder;
            cadastrarTransacao($"Nome Alterado Para {newHolder}\n", historico, cpfIndex);

            Console.WriteLine("\nNome Alterado com Sucesso!!\n");
        }
        static void updatePassword(List<string> senhas, int cpfIndex, List<string> historico)
        {
            Console.Clear();
            Console.Write("Informe a Nova Senha: ");
            string newPassword = Console.ReadLine();

            senhas[cpfIndex] = newPassword;
            cadastrarTransacao($"Senha Alterada Com Sucesso\n", historico, cpfIndex);

            Console.WriteLine("\nSenha Alterada Com Sucesso!!\n");
        }
        static void deleteUser(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos, int indexCpfLogado)
        {
            cpfs.RemoveAt(indexCpfLogado);
            titulares.RemoveAt(indexCpfLogado);
            senhas.RemoveAt(indexCpfLogado);
            saldos.RemoveAt(indexCpfLogado);

            Console.Clear();
            Console.WriteLine("Conta Deletada Com Sucesso!!\n");
        }
        static void cadastrarTransacao(string transacao, List<string> historico, int cpfIndex)
        {
            historico[cpfIndex] += transacao;
        }
        static void Main()
        {
            List<string> cpfs = new();
            List<string> titulares = new();
            List<string> senhas = new();
            List<double> saldos = new();
            List<string> historico = new();
            while (true)
            {
                int optionWelcome;
                int indexCpfLogado;
                Console.WriteLine("\nBem Vindo(a) ao ByteBank! \n");
                while (true)
                {
                    Console.WriteLine("1 - Acessar\n2 - Criar Conta\n");
                    Console.Write("Informe Um Opção: ");

                    optionWelcome = int.Parse(Console.ReadLine());

                    if (optionWelcome != 1 && optionWelcome != 2)
                    {
                        Console.Clear();
                        Console.WriteLine("Opção Não Encontrada, Favor Digite Uma Opção Válida!\n");
                    }
                    if (optionWelcome == 1)
                    {
                        Console.Clear();
                        indexCpfLogado = login(cpfs, senhas);

                        Console.WriteLine();

                        if (indexCpfLogado != -1)
                        {
                            Console.Clear();
                            Console.WriteLine($"Olá {titulares[indexCpfLogado]}, Seja Bem Vindo!\n");
                            cadastrarTransacao("Usuário Logado Com Sucesso\n", historico, indexCpfLogado);
                            break;
                        }
                        Console.WriteLine("CPF ou Senha Incorretos!\n");
                    }
                    if (optionWelcome == 2)
                    {
                        Console.Clear();
                        indexCpfLogado = createAccount(cpfs, titulares, senhas, saldos, historico);
                        cadastrarTransacao("Conta Criada Com Sucesso\n", historico, indexCpfLogado);
                        break;
                    }
                }
                int option;
                do
                {
                    showMenu();
                    option = int.Parse(Console.ReadLine());

                    Console.Clear();

                    switch (option)
                    {
                        case 0:
                            Console.WriteLine("Saindo da Conta!\n");
                            cadastrarTransacao("Saindo da Conta\n", historico, indexCpfLogado);
                            break;
                        case 1:
                            detailUser(cpfs, titulares, saldos, indexCpfLogado);
                            break;
                        case 2:
                            showBalance(saldos, indexCpfLogado);
                            break;
                        case 3:
                            updateAccount(cpfs, titulares, senhas, indexCpfLogado, historico);
                            break;
                        case 4:
                            manipulateAccount(cpfs, titulares, saldos, indexCpfLogado, historico); 
                            break;
                        case 5:
                            deleteUser(cpfs, titulares, senhas, saldos, indexCpfLogado);
                            option = 0;
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine(historico[indexCpfLogado]);
                            Console.Write("\nAperte Uma Tecla Para Ir ao Menu...");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Opção Inválida\n");
                            break;
                    }
                } while (option != 0);
            }
        }
    }
}
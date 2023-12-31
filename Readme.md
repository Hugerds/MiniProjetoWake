# Mini Projeto - Wake Commerce

Este projeto consiste em uma API e um Web App para realizar operações CRUD (Create, Read, Update, Delete) de produtos. Ele permite que você crie, atualize, exclua e liste produtos, além de realizar buscas e ordenações.

## Requisitos Funcionais

O projeto foi desenvolvido com base nos seguintes requisitos funcionais:

- **Criar um produto**
  - A entidade de Produto deve conter pelo menos: Nome, Estoque e Valor.
  - O valor do produto não pode ser negativo.

- **Atualizar um produto**

- **Deletar um produto**

- **Listar os produtos**
  - Visualizar um produto específico.
  - Ordenar os produtos por diferentes campos.
  - Buscar produto pelo nome.

## Requisitos Não Funcionais

O projeto foi desenvolvido com base nos seguintes requisitos funcionais:

- **Utilizar .NET 6**

- **Utilizar o Entity Framework ou Dapper**
    - Utilizei Entity Framework com code-first

## Bônus

- **Utilizar padrões de projeto**
  - Especificado mais adianta na seção Estrutura do Projeto
    
- **Utilizar github actions para os testes**
  - Configurado para commits na branch master, executa o comando dotnet test
    
- **Incluir testes unitários**
  - Inclusos e especificados mais adiante na seção Estrutura do Projeto
    
- **Incluir testes de integração**
  - Inclusos e especificados mais adiante na seção Estrutura do Projeto

## Estrutura do Projeto

O projeto está dividido em três partes principais:

- **MiniProjetoWakeCore:** Contém os repositories, migrations e context do banco de dados. Nesta camada, utilizei o padrão de projeto Repository para separar a lógica de acesso a dados do resto do código. Também faço uso da injeção de dependência para os repositories.

- **MiniProjetoWakeAPI:** Responsável por fornecer as APIs para as operações CRUD de produtos, é utilizada no projeto para os testes de integração

- **MiniProjetoWakeWEB:** Uma interface web que permite interagir com o sistema, realizando as operações CRUD de produtos com as validações especificadas. Utilizei o padrão de projeto MVC (Model-View-Controller) nesta camada para separar a lógica de negócios (Model), a apresentação (View) e o controle (Controller) do WebApp. Também faço uso da injeção de dependência para os repositories.

- **MiniProjetoWakeTests:** Testes unitários e de integração validando os requisitos funcionais especificados. Para os testes unitários tem um repositório mockado que utiliza a memória runtime da aplicação como armazenamento, para os testes de integração a comunicação é feita com o banco de dados do projeto.

## Pré-requisitos

Antes de executar o projeto, certifique-se de que você tenha os seguintes pré-requisitos instalados:

- [.NET 6](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0)

## Instalação e Uso

1. Clone o repositório para a sua máquina.

2. Abra o arquivo MiniProjetoWake.sln

3. Certifique-se de que o Visual Studio está com o projeto de inicialização definido como MiniProjetoWakeWEB
   
4. Inicie o projeto

5. Acesse o WebApp no seu navegador e comece a interagir com o sistema para criar, atualizar, excluir e listar produtos.

## Contato

Se você tiver alguma dúvida ou precisar de ajuda, entre em contato através do [hugoestevesprofissional@gmail.com](mailto:hugoestevesprofissional@gmail.com)

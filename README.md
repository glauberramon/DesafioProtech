# DesafioProtech# DesafioProtech  

Este repositório contém uma aplicação ASP.NET Core para a gestão de animes, incluindo uma API para operações CRUD e testes unitários para garantir a qualidade do código.  

## Tecnologias Utilizadas  

- **C# 12.0**  
- **.NET 8**  
- **ASP.NET Core**: Para construção da API.  
- **Entity Framework Core**: Para mapeamento objeto-relacional (ORM).  
- **Moq**: Para criação de mocks nos testes unitários.  
- **xUnit**: Para execução dos testes unitários.  
- **Swagger**: Para documentação interativa da API.  

## Funcionalidades  

### API  
A API permite realizar as seguintes operações:  
- **Listar Animes**: Com suporte a filtros e paginação.  
- **Obter Anime por ID**: Retorna os detalhes de um anime específico.  
- **Cadastrar Anime**: Adiciona um novo anime ao sistema.  
- **Atualizar Anime**: Permite a atualização parcial ou total de um anime existente.  
- **Remover Anime (Exclusão Lógica)**: Desativa um anime sem removê-lo do banco de dados.  

### Testes Unitários  
Os testes unitários cobrem os seguintes cenários:  
- **Cadastrar Anime**:  
 - Retorna `201` quando os dados são válidos.  
 - Retorna `400` quando os dados são inválidos.  
 - Retorna `500` em caso de exceção.  
- **Listar Animes**:  
 - Retorna `200` quando há animes cadastrados.  
 - Retorna `400` para parâmetros de paginação inválidos.  
 - Retorna `500` em caso de exceção.  
- **Atualizar Anime**:  
 - Retorna `204` quando a atualização é bem-sucedida.  
 - Retorna `404` quando o anime não é encontrado.  
 - Retorna `400` para dados inválidos.  
 - Retorna `500` em caso de exceção.  
- **Obter Anime por ID**:  
 - Retorna `200` quando o anime é encontrado.  
 - Retorna `404` quando o anime não é encontrado.  
 - Retorna `500` em caso de exceção.  
- **Remover Anime (Exclusão Lógica)**:  
 - Retorna `204` quando a remoção é bem-sucedida.  
 - Retorna `404` quando o anime não é encontrado.  
 - Retorna `500` em caso de exceção.  

## Estrutura do Projeto  

A solução está organizada da seguinte forma:  

- **DesafioProtech.API**: Contém os controladores, DTOs e configurações da API.  
- **DesafioProtech.Domain**: Contém as entidades, interfaces e exceções do domínio.  
- **DesafioProtech.Tests**: Contém os testes unitários para os controladores.  

## Como Executar  

### Pré-requisitos  
- .NET 8 SDK instalado.  
- Visual Studio ou outro IDE compatível.  

### Passos  
1. Clone o repositório:
2. Restaure as dependências:
3. Execute a aplicação:
4. Acesse a documentação da API no navegador:



## Licença  

Este projeto está licenciado sob a [MIT License](LICENSE).  

---  

**Autor:** Glauber Ramon Bezerra de Souza 
**Contato:** [glauber.ramon@gmail.com](mailto:glauber.ramon@gmail.com)

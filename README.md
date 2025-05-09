# DesafioProtech# DesafioProtech  

Este reposit�rio cont�m uma aplica��o ASP.NET Core para a gest�o de animes, incluindo uma API para opera��es CRUD e testes unit�rios para garantir a qualidade do c�digo.  

## Tecnologias Utilizadas  

- **C# 12.0**  
- **.NET 8**  
- **ASP.NET Core**: Para constru��o da API.  
- **Entity Framework Core**: Para mapeamento objeto-relacional (ORM).  
- **Moq**: Para cria��o de mocks nos testes unit�rios.  
- **xUnit**: Para execu��o dos testes unit�rios.  
- **Swagger**: Para documenta��o interativa da API.  

## Funcionalidades  

### API  
A API permite realizar as seguintes opera��es:  
- **Listar Animes**: Com suporte a filtros e pagina��o.  
- **Obter Anime por ID**: Retorna os detalhes de um anime espec�fico.  
- **Cadastrar Anime**: Adiciona um novo anime ao sistema.  
- **Atualizar Anime**: Permite a atualiza��o parcial ou total de um anime existente.  
- **Remover Anime (Exclus�o L�gica)**: Desativa um anime sem remov�-lo do banco de dados.  

### Testes Unit�rios  
Os testes unit�rios cobrem os seguintes cen�rios:  
- **Cadastrar Anime**:  
 - Retorna `201` quando os dados s�o v�lidos.  
 - Retorna `400` quando os dados s�o inv�lidos.  
 - Retorna `500` em caso de exce��o.  
- **Listar Animes**:  
 - Retorna `200` quando h� animes cadastrados.  
 - Retorna `400` para par�metros de pagina��o inv�lidos.  
 - Retorna `500` em caso de exce��o.  
- **Atualizar Anime**:  
 - Retorna `204` quando a atualiza��o � bem-sucedida.  
 - Retorna `404` quando o anime n�o � encontrado.  
 - Retorna `400` para dados inv�lidos.  
 - Retorna `500` em caso de exce��o.  
- **Obter Anime por ID**:  
 - Retorna `200` quando o anime � encontrado.  
 - Retorna `404` quando o anime n�o � encontrado.  
 - Retorna `500` em caso de exce��o.  
- **Remover Anime (Exclus�o L�gica)**:  
 - Retorna `204` quando a remo��o � bem-sucedida.  
 - Retorna `404` quando o anime n�o � encontrado.  
 - Retorna `500` em caso de exce��o.  

## Estrutura do Projeto  

A solu��o est� organizada da seguinte forma:  

- **DesafioProtech.API**: Cont�m os controladores, DTOs e configura��es da API.  
- **DesafioProtech.Domain**: Cont�m as entidades, interfaces e exce��es do dom�nio.  
- **DesafioProtech.Tests**: Cont�m os testes unit�rios para os controladores.  

## Como Executar  

### Pr�-requisitos  
- .NET 8 SDK instalado.  
- Visual Studio ou outro IDE compat�vel.  

### Passos  
1. Clone o reposit�rio:
2. Restaure as depend�ncias:
3. Execute a aplica��o:
4. Acesse a documenta��o da API no navegador:



## Licen�a  

Este projeto est� licenciado sob a [MIT License](LICENSE).  

---  

**Autor:** Glauber Ramon Bezerra de Souza 
**Contato:** [glauber.ramon@gmail.com](mailto:glauber.ramon@gmail.com)
